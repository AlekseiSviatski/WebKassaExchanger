using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebKassa.Models.Response.Document
{
    public class CheckDocument
    {
        [JsonPropertyName("sellPointId")]
        public int? SellPointId { get; set; }

        [JsonPropertyName("sellPointName")]
        public string? SellPointName { get; set; }

        [JsonPropertyName("boxId")]
        public int? BoxId { get; set; }

        [JsonPropertyName("cashRegisterId")]
        public int? CashRegisterId { get; set; }

        [JsonPropertyName("modelId")]
        public int? ModelId { get; set; }

        [JsonPropertyName("modelName")]
        public string? ModelName { get; set; }

        [JsonPropertyName("shiftId")]
        public int? ShiftId { get; set; }

        [JsonPropertyName("shiftNumber")]
        public int? ShiftNumber { get; set; }

        [JsonPropertyName("beginDate")]
        public long? BeginDate { get; set; }

        [JsonPropertyName("endDate")]
        public long? EndDate { get; set; }

        [JsonPropertyName("cashierId")]
        public int? CashierId { get; set; }

        [JsonPropertyName("cashierName")]
        public string? CashierName { get; set; }

        [JsonPropertyName("merchantNameShort")]
        public string? MerchantNameShort { get; set; }

        [JsonPropertyName("docId")]
        public int? DocId { get; set; }

        [JsonPropertyName("docNumber")]
        public int? DocNumber { get; set; }

        [JsonPropertyName("uid")]
        public string? Uid { get; set; }

        [JsonPropertyName("status")]
        public int? Status { get; set; }

        [JsonPropertyName("statusName")]
        public string? StatusName { get; set; }

        [JsonPropertyName("docDate")]
        public long? DocDate { get; set; }

        [JsonPropertyName("docCurrency")]
        public string? DocCurrency { get; set; }

        [JsonPropertyName("totalSum")]
        public double? TotalSum { get; set; }

        [JsonPropertyName("docTypeName")]
        public string? DocTypeName { get; set; }

        [JsonPropertyName("docType")]
        public int? DocType { get; set; }

        [JsonPropertyName("totalSumCash")]
        public double? TotalSumCash { get; set; }

        [JsonPropertyName("totalSumNoCash")]
        public double? TotalSumNoCash { get; set; }

        [JsonPropertyName("merchantId")]
        public int? MerchantId { get; set; }

        [JsonPropertyName("cashierAndModel")]
        public string? CashierAndModel { get; set; }
    }
}
