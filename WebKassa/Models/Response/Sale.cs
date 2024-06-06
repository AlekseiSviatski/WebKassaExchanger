using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebKassa.Models.Response
{
    public class Sale
    {
        [JsonPropertyName("saleId")]
        public int? Id { get; set; }

        [JsonPropertyName("goodName")]
        public string? Name { get; set; }

        [JsonPropertyName("goodId")]
        public int? GoodId { get; set; }

        [JsonPropertyName("prodItemId")]
        public int? ProductItemId { get; set; }

        [JsonPropertyName("quantity")]
        public double? Quantity { get; set; }

        [JsonPropertyName("price")]
        public double? Price { get; set; }

        [JsonPropertyName("sum")]
        public double? Sum { get; set; }

        [JsonPropertyName("discount")]
        public double? Discount { get; set; }

        [JsonPropertyName("totalCash")]
        public double? TotalCash { get; set; }

        [JsonPropertyName("totalNoCash")]
        public double? TotalNoCash { get; set; }

        [JsonPropertyName("totalOther")]
        public double? TotalOther { get; set; }

        [JsonPropertyName("vendorCode")]
        public string? Article { get; set; }

        [JsonPropertyName("barValue")]
        public string? Barcode { get; set; }

        [JsonPropertyName("buyVatId")]
        public int? BuyVatId { get; set; }

        [JsonPropertyName("sellVatId")]
        public int? SetVatId { get; set; }
    }
}
