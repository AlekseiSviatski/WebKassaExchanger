namespace WebKassa.Models.DBModel
{
	public class CashboxOrderEditModel
	{
		public int? IdCashlessOrder { get; set; }
		public int? IdSeasonOrder { get; set; }
		public int? CodeKey { get; set; }
		public long? Barcode { get; set; } = null;
		public int? MainOrder { get; set; } = -1;
		public bool? Active { get; set; } = true;
		public bool? UseAdditionalCharges { get; set; } = false;
		public int? CashboxCartOrder { get; set; }
		public int? CashboxId { get; set; }
		public int? CashierId { get; set; }
		public bool? FiscalMode { get; set; } = true;

	}
}
