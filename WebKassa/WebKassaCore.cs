using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebKassa.Models;
using WebKassa.Models.DBModel;
using WebKassa.Models.Request;
using WebKassa.Models.Response;
using WebKassa.Models.Response.Document;

namespace WebKassa
{
	/// <summary>
	/// LOL
	/// </summary>
	public class WebKassaCore : BaseMessaging
	{
		private readonly DateTime _baseDate = new DateTime(1970, 1, 1);
		private DBWork _db;
		private PPSConfig? _ppsConfig;

		public WebKassaCore(Uri baseUri, string apiToken, DBWork db) : base(baseUri, apiToken)
		{
			_db = db;
		}
		public WebKassaCore(Uri baseUri, string apiToken, DBWork db, PPSConfig ppsConfig) : base(baseUri, apiToken)
		{
			_db = db;
			_ppsConfig = ppsConfig;
		}

		#region Methods

		public async Task<ICollection<Sale>> GetSalesAsync(DateTime date, int cashRegisterId)
		{
			var dateParam = date.Date.Subtract(_baseDate).TotalMilliseconds;
			Dictionary<string, string> queryParms = new()
			{
				{"date", dateParam.ToString() },
				{"cashRegisterId", cashRegisterId.ToString() }
			};
			var result = await SendRequestAsync<List<Sale>, string>(HttpMethod.Get, null, "api/get-sales", queryParms);

			return result?? new();
		}

		public async Task<ICollection<Sale>> GetSalesAsync(DateTime date, List<int> cashRegisterIds)
		{
			List<Sale> result = new List<Sale>();
			var dateParam = date.Date.Subtract(_baseDate).TotalMilliseconds;

			foreach (var x in cashRegisterIds)
			{
				Dictionary<string, string> queryParms = new Dictionary<string, string>()
				{
					{"date", dateParam.ToString() },
					{"cashRegisterId", x.ToString() }
				};

				var answer = await SendRequestAsync<List<Sale>, string>(HttpMethod.Get, null, "api/get-sales", queryParms);

				if (answer != null)
				{
					result.AddRange(answer);
				}
			}

			return result;
		}

		public async Task<ICollection<Good>> GetGoodsAsync(WarehouseRequestOptions options)
		{

			var result = await SendRequestAsync<List<Good>, WarehouseRequestOptions>(HttpMethod.Post, options, "api/warehouse/list", null);

			return result ?? new();
		}

		public async Task<ICollection<CheckDocument>> GetCheckHistoryAsync(DateTime date, int cashRegisterId)
		{
			var dateParam = date.Date.Subtract(_baseDate).TotalMilliseconds;
			Dictionary<string, string> queryParms = new Dictionary<string, string>()
			{
				{"date", dateParam.ToString() },
				{"cashRegisterId", cashRegisterId.ToString() }
			};
			var result = await SendRequestAsync<List<CheckDocument>, string>(HttpMethod.Get, null, "api/get-check-history", queryParms);
			return result ?? new();
		}

		public async Task<CashboxDocuments> GetCashboxDocumentsAsync(DateTime date, int cashRegisterId)
		{
			var dateStartParam = date.Date.Subtract(_baseDate).TotalMilliseconds;
			var dateEndParam = (date.Date + TimeSpan.Parse("23:59:59")).Subtract(_baseDate).TotalMilliseconds;

			Dictionary<string, string> queryParms = new Dictionary<string, string>()
			{
				{"dateStart", dateStartParam.ToString() },
				{"dateEnd", dateEndParam.ToString() },
				{"cashRegisterId", cashRegisterId.ToString() }
			};
			var result = await SendRequestAsync<CashboxDocuments, string>(HttpMethod.Get, null, "api/get-cashdocs-for-period", queryParms);
			return result ?? new();
		}

		public async Task UpdateDBAsync(
			ICollection<Sale> sales,
			ICollection<SingleServiceModel> dbSingleServices,
			int? idCashier,
			int? idCashbox,
			int? idEmployee)
		{
			var updateList = dbSingleServices.Join(sales,
				x => x.Code,
				s => s.Article,
				(a, b) => new SingleServiceUpdateModel
				{
					SingleServiceId = a.ID,
					Count = b.Quantity,
					Price = b.Price,
					TotalPrice = b.Sum,
					DiscountPrice = b.Discount,
					PaidPrice = b.Sum,
					PaidCount = b.Quantity,
					CashPrice = b.TotalCash,
					CardPrice = b.TotalNoCash
				}).ToList();
			await _db.UpdateSingleServicesSalesAsync(updateList, idCashier, idCashbox, idEmployee);
		}

		public async Task UpdateDBAsync(
			ICollection<SingleServiceModel> dbSingleServices,
			ICollection<CheckDocument> checks,
			ICollection<Good> goods,
			CashboxDocuments documents,
			int CashboxId
		)
		{
			try
			{
				var cashiers = await _db.GetCashiersAsync();

				var salesToImport = documents.Sales?.Join(
					checks,
					s => s.Uid,
					c => c.Uid,
					(a, b) => new
					{
						a.Sale?.SaleId,
						b.CashierName,
						a.Sale?.TotalSumCash,
						a.Sale?.TotalSumNoCash,
						a.Sale?.TotalSumOther,
						SaledGoods = a?.Sale?.SaleProducts?
							.Join(goods,
							sp => sp?.GoodId,
							g => g.Id,
							(w, v) => new SaledGood
							{
								GoodId = w?.GoodId,
								Article = v.Article,
								Name = v.Name,
								Quantity = w?.Quantity,
								BuyPrice = v.BuyPrice,
								Discount = w?.Discount,
								SellPrice = v.SellPrice,
								Sum = w?.Sum
							}).ToList() ?? new()
					}).GroupJoin(cashiers, z => z.CashierName, b => b.FLOGIN, (z, b) => new SalesToImportModel
					{
						SaleId = z.SaleId,
						CashierName = z.CashierName,
						CashierId = b.FirstOrDefault(cr => cr.FUSER == z.CashierName)?.FID ?? _ppsConfig?.CashierId,
						TotalSumCash = z.TotalSumCash,
						TotalSumNoCash = z.TotalSumNoCash,
						TotalSumOther = z.TotalSumOther,
						SaledGoods = z.SaledGoods,
						EmployeeId = b.FirstOrDefault(cr => cr.FLOGIN == z.CashierName)?.PersonVisitorID ?? _ppsConfig?.EmployeeId
					}).ToList() ?? new();

				foreach (var x in salesToImport)
				{
					List<SingleServiceUpdateModel> singleServiceUpdateModels = new List<SingleServiceUpdateModel>();

					x.SaledGoods?.ForEach(s =>
					{

						singleServiceUpdateModels.Add(
							new SingleServiceUpdateModel
							{
								SingleServiceId = dbSingleServices?.FirstOrDefault(x => x.Code == s.Article)?.ID,
								Count = s.Quantity,
								Price = s.SellPrice,
								TotalPrice = s.Sum,
								DiscountPrice = s.Discount,
								PaidPrice = s.Sum,
								PaidCount = s.Quantity,
								TerminalId = null,
								CashPrice = (x.TotalSumCash != null) & (x.TotalSumCash > 0) ? s.Sum : null,
								CardPrice = (x.TotalSumNoCash != null) & (x.TotalSumNoCash > 0) ? s.Sum : null,
							});
					});

					await _db.UpdateSingleServicesSalesAsync(singleServiceUpdateModels, x.CashierId, CashboxId, x.EmployeeId);
				}
			}
			catch
			{
				throw;
			}

		}

		public async Task CreateImportFileAsync(string path)
		{
			(await _db.GetSingleServicesAsync()).CreateImportFile(path);
		}

		public async Task ImportFromAccountAsync(DateTime date, List<int> cashRegisterIds, int? idCashier, int? idCashbox, int? idEmployee)
		{
			try
			{
				var sales = (await GetSalesAsync(date, cashRegisterIds)).ToList();

				var dbSingleServices = (await _db.GetSingleServicesAsync()).ToList();
				await UpdateDBAsync(sales, dbSingleServices, idCashier, idCashbox, idEmployee);
			}
			catch
			{
				throw;
			}
		}

		public async Task ImportFromAccountAsync(DateTime date, List<CashboxPair> cashboxPairs, int? idCashier, int? idEmployee)
		{
			try
			{
				//----------------костыль для проверки ответа от веб кассы-----------------------
				var ids = cashboxPairs.Select(x => x.ProgramCashboxId).ToList();
				var sales = (await GetSalesAsync(date, ids)).ToList();
				//-------------------------------------------------------------------------------

				var dbSingleServices = (await _db.GetSingleServicesAsync()).ToList();

				foreach (var x in cashboxPairs)
				{
					await UpdateDBAsync(await GetSalesAsync(date, x.ProgramCashboxId), dbSingleServices, idCashier, x.PPSCashboxId, idEmployee);
				}
			}
			catch
			{
				throw;
			}
		}

		public async Task ImportFromAccountAsync(DateTime date, List<CashboxPair> cashboxPairs)
		{
			try
			{
				//----------------костыль для проверки ответа от веб кассы-----------------------
				var ids = cashboxPairs.Select(x => x.ProgramCashboxId).ToList();
				var sales = (await GetSalesAsync(date, ids)).ToList();
				//-------------------------------------------------------------------------------

				var dbSingleServices = (await _db.GetSingleServicesAsync()).ToList();

				foreach (var x in cashboxPairs)
				{
					var goods = await GetGoodsAsync(new WarehouseRequestOptions());
					var checks = await GetCheckHistoryAsync(date, x.ProgramCashboxId);
					var docs = await GetCashboxDocumentsAsync(date, x.ProgramCashboxId);

					await UpdateDBAsync(dbSingleServices, checks, goods, docs, x.PPSCashboxId);
				}
			}
			catch
			{
				throw;
			}
		}

		#endregion

	}

	#region Extentions

	public static class Extentions
	{
		internal static void NameBuild(this Sale sale)// о_О
		{
			if (!string.IsNullOrEmpty(sale.Name))
			{
				string name = sale.Name;
				var temp = name.Split(" ").ToList();
				temp.RemoveAt(0);
				StringBuilder sb = new StringBuilder();
				temp.ForEach(x => sb.Append(x + " "));
				var newName = sb.ToString();
				newName.Remove(newName.Length - 2);
				sale.Name = newName;
			}
		}

		public static void CreateImportFile(this ICollection<SingleServiceModel> singleServices, string path)//TODO: refactoring required!
		{
			using StreamWriter writer = new StreamWriter(File.Open(
				Path.Combine(path, @$"{DateTime.Now.ToString("dd-MM-yyyy")}_ToWebKassa.csv"),
				FileMode.Create),
				encoding: Encoding.GetEncoding(1251));

			string firstline = "\"Код (артикул)\";Наименование;Ед. измерения;Себестоимость;Цена;Количество;Категория;ТоваруслугаСертификат;Штрих-код;Использование;Код ставки НДС;Процент скидки;Секция";

			writer.WriteLine(firstline);

			singleServices.ToList().ForEach(singleService =>
			{
				string line = String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12}",
					singleService.Code,
					singleService.Name,
					0, //ед. измерения 0 - шт, 1 - кг, 2 - л, 3 - м
					singleService.PriceReceipt is null ? singleService.Price?.ToString("0.00", CultureInfo.InvariantCulture) : singleService.PriceReceipt.Value.ToString("0.00", CultureInfo.InvariantCulture),
					singleService.PriceDistribution is null ? singleService.Price?.ToString("0.00", CultureInfo.InvariantCulture) : singleService.PriceDistribution.Value.ToString("0.00", CultureInfo.InvariantCulture),
					singleService.CountRemaining?.ToString("0.00", CultureInfo.InvariantCulture),
					string.Empty, //категория
					0, // 0 - товар, 1 - услуга, 2 - сертификат
					singleService.Barcode?.Length < 13 ? string.Empty : singleService.Barcode,
					1, // 0 - товар не доступен для продажи, 1 - доступен
					3, // НДС: 0 - 20%, 1 - 10%, 3 - no nds
					0, // % скидки (только целое число)
					null //секция
					);

				writer.WriteLine(line);
			});
		}
	}

	#endregion
}