using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebKassa.Models.Response.Document
{
    public class CashboxDocuments
    {
        [JsonPropertyName("sales")]
        public List<SaleDocument>? Sales { get; set; }
        //public List<DepositDocument> deposits { get; set; }
        //public List<WithdrawDocument> withdraws { get; set; }
        //public List<MoneyBackDocument> moneyBacks { get; set; }
        //public List<RollBackDocument> rollBacks { get; set; }
        //public List<ZreportDocument> zreports { get; set; }
    }
}
