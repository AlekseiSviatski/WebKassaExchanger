﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebKassa.Models;
using WebKassa.Models.DBModel;
using WebKassa.Models.Request;
using WebKassa.Models.Response;

namespace WebKassa
{
    public class WebKassaCore : BaseMessaging
    {
        private DateTime _baseDate = new DateTime(1970, 1, 1);
        private DBWork _db;

        public WebKassaCore(Uri baseUri, string apiToken, DBWork db) : base(baseUri, apiToken)
        {
            _db = db;
        }

        #region Methods

        public async Task<ICollection<Sale>> GetSalesAsync(DateTime date, int cashRegisterId)
        {
            var dateParam = date.Date.Subtract(_baseDate).TotalMilliseconds;
            Dictionary<string, string> queryParms = new Dictionary<string, string>()
            {
                {"date", dateParam.ToString() },
                {"cashRegisterId", cashRegisterId.ToString() }
            };
            var result = await SendRequestAsync<List<Sale>, string>(HttpMethod.Get, null, "api/get-sales", queryParms);
            return result;
        }

        public async Task<ICollection<Good>> GetGoodListAsync(WarehouseRequestOptions options) =>
            await SendRequestAsync<List<Good>, WarehouseRequestOptions>(HttpMethod.Post, options, "api/warehouse/list");

        public async Task<bool> UpdateDB(ICollection<Sale> sales, ICollection<SingleServiceDB> dbSingleServices)
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
            var result = await _db.UpdateSingleServicesSales(updateList);
            return result;
        }

        public async Task CreateImportFile(string path)
        {
            (await _db.GetSingleServices()).CreateImportFile(path);
        }

        #endregion
    }

    #region Extentions

    public static class Extentions
    {
        internal static void NameBuild(this Sale sale)
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

        public static void CreateImportFile(this ICollection<SingleServiceDB> singleServices, string path)
        {
            using StreamWriter writer = new StreamWriter(File.Open(Path.Combine(path, $"{DateTime.Now.ToShortDateString()}_ToWebKassa.csv"), FileMode.Create), encoding: Encoding.GetEncoding(1251));
            string firstline = "\"Код (артикул)\";Наименование;Ед. измерения;Себестоимость;Цена;Количество;Категория;ТоваруслугаСертификат;Штрих-код;Использование;Код ставки НДС;Процент скидки;Секция";

            writer.WriteLine(firstline);

            singleServices.ToList().ForEach(singleService =>
            {
                string line = String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12}",
                    string.IsNullOrEmpty(singleService.Code) ? Guid.NewGuid() : singleService.Code,
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
                    null /*секция*/ );

                writer.WriteLine(line);
            });
        }
    }

    #endregion
}
