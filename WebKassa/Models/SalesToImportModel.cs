using System.Collections.Generic;

namespace WebKassa.Models
{
	public class SalesToImportModel
	{
		public int? SaleId { get; set; }
		public string? CashierName { get; set; }
		public int? CashierId { get; set; }
		public double? TotalSumCash { get; set; }
		public double? TotalSumNoCash { get; set; }
		public double? TotalSumOther { get; set; }
		public int? EmployeeId { get; set; }
		public List<SaledGood> SaledGoods { get; set; } = new();
	}

	public class SaledGood
	{
		public int? GoodId { get; set; }
		public string? Article { get; set; }
		public string? Name { get; set; }
		public double? Quantity { get; set; }
		public double? BuyPrice { get; set; }
		public double? Discount { get; set; }
		public double? SellPrice { get; set; }
		public double? Sum { get; set; }
	}
}
