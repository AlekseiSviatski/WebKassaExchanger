using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebKassa.Models.Response.Document
{
	public class SaleDocument
	{
		[JsonPropertyName("docId")]
		public int? DocId { get; set; }

		[JsonPropertyName("uid")]
		public string? Uid { get; set; }

		[JsonPropertyName("docDate")]
		public double? DocDate { get; set; }

		[JsonPropertyName("docNumber")]
		public int? DocNumber { get; set; }

		[JsonPropertyName("dateIn")]
		public double? DateIn { get; set; }

		[JsonPropertyName("status")]
		public int? Status { get; set; }

		[JsonPropertyName("docType")]
		public int? DocType { get; set; }

		[JsonPropertyName("shiftId")]
		public int? ShiftId { get; set; }

		[JsonPropertyName("sale")]
		public SaleDocumentPart? Sale { get; set; }
	}

	public class SaleDocumentPart
	{
		[JsonPropertyName("saleId")]
		public int? SaleId { get; set; }

		[JsonPropertyName("subTotalSum")]
		public double? SubTotalSum { get; set; }

		[JsonPropertyName("subTotalDiscount")]
		public double? SubTotalDiscount { get; set; }

		[JsonPropertyName("subTotalAllowance")]
		public double? SubTotalAllowance { get; set; }

		[JsonPropertyName("totalSumCash")]
		public double? TotalSumCash { get; set; }

		[JsonPropertyName("totalSumNoCash")]
		public double? TotalSumNoCash { get; set; }

		[JsonPropertyName("totalSumOther")]
		public double? TotalSumOther { get; set; }

		[JsonPropertyName("saleProducts")]
		public List<SaleProduct> SaleProducts { get; set; } = new();
	}

	public class SaleProduct
	{
		[JsonPropertyName("productId")]
		public int? ProductId { get; set; }

		[JsonPropertyName("goodId")]
		public int? GoodId { get; set; }

		[JsonPropertyName("prodItemType")]
		public int? ProdItemType { get; set; }

		[JsonPropertyName("prodItemId")]
		public double? ProdItemId { get; set; }//

		[JsonPropertyName("productName")]
		public string? ProductName { get; set; }

		[JsonPropertyName("quantity")]
		public double? Quantity { get; set; }

		[JsonPropertyName("sum")]
		public double? Sum { get; set; }

		[JsonPropertyName("discount")]
		public double? Discount { get; set; }

		[JsonPropertyName("allowance")]
		public double? Allowance { get; set; }

		[JsonPropertyName("productOrder")]
		public int? ProductOrder { get; set; }

		[JsonPropertyName("markType")]
		public int? MarkType { get; set; }

		[JsonPropertyName("markValue")]
		public string? MarkValue { get; set; }

		[JsonPropertyName("uniqSequence")]
		public string? UniqSequence { get; set; }

		[JsonPropertyName("nsection")]
		public int? Nsection { get; set; }
	}
}
