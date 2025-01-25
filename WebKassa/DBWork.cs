using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using WebKassa.Models;
using WebKassa.Models.DBModel;

namespace WebKassa
{
	public class DBWork
	{
		private string _connectionString;

		public DBWork(string connectionString)
		{
			_connectionString = connectionString;
		}

		public async Task<ICollection<SingleServiceModel>> GetSingleServicesAsync(int? id = null)
		{
			using SqlConnection connection = new SqlConnection(_connectionString);

			var parametres = new Dictionary<string, object?	>
			{
				{"@pID", id }
			};
			var queryParams = new DynamicParameters(parametres);

			var singleServiceList = (await connection.QueryAsync<SingleServiceModel>("SingleServiceGet", queryParams, commandType: CommandType.StoredProcedure)).ToList();

			return singleServiceList;
		}

		public async Task<ICollection<CashboxCashier>> GetCashiersAsync()
		{
			using SqlConnection connection = new SqlConnection(_connectionString);

			var result = (await connection.QueryAsync<CashboxCashier>(
				@"select
					t.FID,
					t.FLOGIN,
					t.FUSER,
					t.PersonVisitorID
				from Tlogins t
				where 
					t.FACTIVE = 1", commandType: CommandType.Text)).ToList();
			
			return result;
		}

		public async Task UpdateSingleServicesSalesAsync(ICollection<SingleServiceUpdateModel> sales, int? idCashier, int? idCashbox, int? idEmployee)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				using DbTransaction transaction = await connection.BeginTransactionAsync();

				try
				{
					sales.ToList()
						.ForEach(s =>
						{
							int? ssuId = null;
							int? chekListId = null;
							int? cashboxPaymentId = null;

							#region SSU

							ssuId = SingleServiceUsedEdit(connection, transaction, s);

							#endregion

							#region CheckList

							if (s.CashPrice > 0)
							{
								chekListId = CashboxCheckListEdit(connection, transaction,
								new CashboxCheckListEditModel
								{
									Price = s.CashPrice,
									TotalModey = s.CashPrice,
									IdCashier = idCashier, //из конфига
									CashboxId = idCashbox // из конфига
								});
							}

							#endregion

							#region CashboxPayment

							if (s.CardPrice > 0)
							{
								cashboxPaymentId = CashboxPaymentEdit(connection, transaction,
								new CashboxPaymentEditModel
								{
									CashboxPaymentTypeId = 3,
									Price = s.CardPrice,
									IdCashier = idCashier, // из конфига
									CashboxId = idCashbox // из конфига
								});
							}

							#endregion

							#region PaymentHistory

							if (chekListId != null)
							{
								PaymentHistoryEdit(connection, transaction,
									new PaymentHistoryEditModel
									{
										CheckListID = chekListId,
										CardPaymentId = null,
										CashlessPaymentID = null,
										CashboxID = idCashbox, // из конфига
										SingleServiceUsedID = ssuId,
										Count = (s.TotalPrice / s.CashPrice) * s.Count,
										PaymentPrice = s.CashPrice,
										Price = s.Price,
										TotalPrice = s.TotalPrice,
										EmployeeID = idEmployee, // из конфига
										CashierID = idCashier, // из конфига
									});
							}

							if (cashboxPaymentId != null)
							{
								PaymentHistoryEdit(connection, transaction,
									new PaymentHistoryEditModel
									{
										CheckListID = null,
										CardPaymentId = cashboxPaymentId,
										CashlessPaymentID = null,
										CashboxID = idCashbox, // из конфига
										SingleServiceUsedID = ssuId,
										Count = (s.TotalPrice / s.CardPrice) * s.Count,
										PaymentPrice = s.CardPrice,
										Price = s.Price,
										TotalPrice = s.TotalPrice,
										EmployeeID = idEmployee, // из конфига
										CashierID = idCashier, // из конфига
									});
							}

							#endregion
						});

					await transaction.CommitAsync();
				}
				catch
				{
					await transaction.RollbackAsync();

					throw;
				}
				finally
				{
					await connection.CloseAsync();
				}
			}
		}

		public async Task UpdateSingleServicesSalesAsync(
			ICollection<SalesToImportModel> salesToImport,
			ICollection<SingleServiceModel> dbSingleServices,
			int? idCashier,
			int? idCashbox,
			int? idEmployee
		)
		{
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				using DbTransaction transaction = await connection.BeginTransactionAsync();

				try
				{
					foreach (var importSale in salesToImport)
					{
						List<SingleServiceUpdateModel> sales = new List<SingleServiceUpdateModel>();
						int? idOrder = CashboxOrderEdit(transaction, new CashboxOrderEditModel());

						importSale?.SaledGoods?.ForEach(s =>
						{
							sales.Add(
								new SingleServiceUpdateModel
								{
									IdOrder = idOrder ?? default,
									SingleServiceId = dbSingleServices?.FirstOrDefault(x => x.Code == s.Article)?.ID,
									Count = s.Quantity,
									Price = s.SellPrice,
									TotalPrice = s.Sum,
									DiscountPrice = s.Discount,
									PaidPrice = s.Sum,
									PaidCount = s.Quantity,
									TerminalId = null,
									CashPrice = (importSale.TotalSumCash != null) & (importSale.TotalSumCash > 0) ? s.Sum : null,
									CardPrice = (importSale.TotalSumNoCash != null) & (importSale.TotalSumNoCash > 0) ? s.Sum : null,
								});
						});

						sales.ToList()
						.ForEach(s =>
						{
							int? ssuId = null;
							int? chekListId = null;
							int? cashboxPaymentId = null;

							#region SSU

							ssuId = SingleServiceUsedEdit(connection, transaction, s);

							#endregion

							#region CheckList

							if (s.CashPrice > 0)
							{
								chekListId = CashboxCheckListEdit(connection, transaction,
								new CashboxCheckListEditModel
								{
									IdOrder = s.IdOrder,
									Price = s.CashPrice,
									TotalModey = s.CashPrice,
									IdCashier = idCashier, //из конфига
									CashboxId = idCashbox // из конфига
								});
							}

							#endregion

							#region CashboxPayment

							if (s.CardPrice > 0)
							{
								cashboxPaymentId = CashboxPaymentEdit(connection, transaction,
								new CashboxPaymentEditModel
								{
									IdOrder = s.IdOrder,
									CashboxPaymentTypeId = 3,
									Price = s.CardPrice,
									IdCashier = idCashier, // из конфига
									CashboxId = idCashbox // из конфига
								});
							}

							#endregion

							#region PaymentHistory

							if (chekListId != null)
							{
								PaymentHistoryEdit(connection, transaction,
									new PaymentHistoryEditModel
									{
										CashboxOrderID = s.IdOrder,
										CheckListID = chekListId,
										CardPaymentId = null,
										CashlessPaymentID = null,
										CashboxID = idCashbox, // из конфига
										SingleServiceUsedID = ssuId,
										Count = (s.TotalPrice / s.CashPrice) * s.Count,
										PaymentPrice = s.CashPrice,
										Price = s.Price,
										TotalPrice = s.TotalPrice,
										EmployeeID = idEmployee, // из конфига
										CashierID = idCashier, // из конфига
									});
							}

							if (cashboxPaymentId != null)
							{
								PaymentHistoryEdit(connection, transaction,
									new PaymentHistoryEditModel
									{
										CashboxOrderID = s.IdOrder,
										CheckListID = null,
										CardPaymentId = cashboxPaymentId,
										CashlessPaymentID = null,
										CashboxID = idCashbox, // из конфига
										SingleServiceUsedID = ssuId,
										Count = (s.TotalPrice / s.CardPrice) * s.Count,
										PaymentPrice = s.CardPrice,
										Price = s.Price,
										TotalPrice = s.TotalPrice,
										EmployeeID = idEmployee, // из конфига
										CashierID = idCashier, // из конфига
									});
							}

							#endregion

						});
					}

					await transaction.CommitAsync();
				}
				catch
				{
					await transaction.RollbackAsync();
					throw;
				}
				finally
				{
					await connection.CloseAsync();
				}
			}
		}

		private int? SingleServiceUsedEdit(SqlConnection connection, DbTransaction transaction, SingleServiceUpdateModel s)
		{
			try
			{
				var parametres = new Dictionary<string, object?>
				{
					{"@pClientOrderID", s.IdOrder },
					{"@pSingleServiceID", s.SingleServiceId },
					{"@pCount", s.Count },
					{"@pPrice", s.Price },
					{"@pTotalPrice", s.TotalPrice },
					{"@pDiscountPrice", s.DiscountPrice },
					{"@pEmployeeOrderID", s.EmployeeOrderId },
					{"@pSingleServiceResevedID", s.SingleServiceResevedId },
					{"@pPaid", s.Paid },
					{"@pPaidPrice", s.PaidPrice },
					{"@pPaidCount", s.PaidCount },
					{"@pTerminalID", s.TerminalId },
					{"@pSingleServiceQueueID", null },
				};

				var dynamicParametres = new DynamicParameters(parametres);

				var ssuId = connection.Query<int?>("SingleServiceUsedEdit", dynamicParametres, transaction, commandType: CommandType.StoredProcedure).FirstOrDefault();

				return ssuId;
			}
			catch
			{
				throw;
			}
		}

		private int? CashboxCheckListEdit(SqlConnection connection, DbTransaction transaction, CashboxCheckListEditModel c)
		{
			try
			{
				var parametres = new Dictionary<string, object?>
				{
					{"@pIdOrder", c.IdOrder},
					{"@pIdSeasonOrder", c.IdSeasonOrder},
					{"@pCashboxPaymentTypeID", c.CashboxPaymentTypeId},
					{"@pPrice", c.Price},
					{"@pTotalMoney", c.TotalModey},
					{"@pTypeSystem", c.TypeSystem},
					{"@pIdCashier", c.IdCashier},
					{"@pIdMode", c.Mode},
					{"@pCashboxID", c.CashboxId},
				};

				var dynamicParametres = new DynamicParameters(parametres);
				dynamicParametres.Add("@pIdCheck", null, DbType.Int32, ParameterDirection.InputOutput);

				var result = connection.Query("CashboxCheckListEdit", dynamicParametres, transaction, commandType: CommandType.StoredProcedure);

				int? idCheck = dynamicParametres.Get<int?>("@pIdCheck");

				return idCheck;
			}
			catch
			{
				throw;
			}
		}

		private int? CashboxPaymentEdit(SqlConnection connection, DbTransaction transaction, CashboxPaymentEditModel c)
		{
			try
			{
				var parametres = new Dictionary<string, object?>
				{
					{"@pCashboxPaymentTypeID", c.CashboxPaymentTypeId},
					{"@pPrice", c.Price},
					{"@pCodeAuthorization", c.CodeAuthorization},
					{"@pCodeCard", c.CodeCard},
					{"@pIdOrder", c.IdOrder},
					{"@pIdSeasonOrder", c.IdSeasonOrder},
					{"@pIdCashier", c.IdCashier},
					{"@pCashboxID", c.CashboxId},
					{"@pCardTypeID", c.CardTypeId},
				};

				var dynamicParametres = new DynamicParameters(parametres);
				dynamicParametres.Add("@pIdPayment", null, DbType.Int32, ParameterDirection.InputOutput);

				var result = connection.Query("CashboxPaymentEdit", dynamicParametres, transaction, commandType: CommandType.StoredProcedure);

				int? idPayment = dynamicParametres.Get<int?>("@pIdPayment");

				return idPayment;
			}
			catch
			{
				throw;
			}
		}

		private void PaymentHistoryEdit(SqlConnection connection, DbTransaction transaction, PaymentHistoryEditModel p)
		{
			try
			{
				var parametres = new Dictionary<string, object?>
				{
					{"@pMode", p.Mode},
					{"@pID", p.Id},
					{"@pCheckListID", p.CheckListID},
					{"@pCardPaymentID", p.CardPaymentId},
					{"@pCashlessPaymentID", p.CashlessPaymentID},
					{"@pPaymentTypeID", p.PaymentTypeID},
					{"@pCashboxID", p.CashboxID},
					{"@pFiscalMode", p.FiscalMode},
					{"@pCertificateID", p.CertificateID},
					{"@pDiscountID", p.DiscountID},
					{"@pMedServReservedID", p.MedServReservedID},
					{"@pSingleServiceReservedID", p.SingleServiceReservedID},
					{"@pSeasonOrderID", p.SeasonOrderID},
					{"@pCertificateReservedID", p.CertificateReservedID},
					{"@pJWSMoneyID", p.JWSMoneyID},
					{"@pJWSSingleServiceID", p.JWSSingleServiceID},
					{"@pCashboxOrderID", p.CashboxOrderID},
					{"@pPayCashboxOrderID", p.PayCashboxOrderID},
					{"@pSingleServiceUsedID", p.SingleServiceUsedID},
					{"@pCount", p.Count},
					{"@pPaymentPrice", p.PaymentPrice},
					{"@pPrice", p.Price},
					{"@pTotalPrice", p.TotalPrice},
					{"@pBalanceTopUp", p.BalanceTopUp},
					{"@pPaySeasonOrderID", p.PaySeasonOrderID},
					{"@pEmployeeID", p.EmployeeID},
					{"@pClientID", p.ClientID},
					{"@pWriteOff", p.WriteOff},
					{"@pRefund", p.Refund},
					{"@pCashierID", p.CashierID},
				};

				var dynamicParametres = new DynamicParameters(parametres);

				connection.Query("PaymentHistoryEdit", dynamicParametres, transaction, commandType: CommandType.StoredProcedure);
			}
			catch
			{
				throw;
			}
		}
		private int? CashboxOrderEdit(DbTransaction transaction, CashboxOrderEditModel c)
		{
			throw new NotImplementedException();
		}
	}
}