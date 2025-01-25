namespace WebKassa.Models.DBModel
{
	public class PaymentHistoryEditModel
	{
		public int? Mode { get; set; } = 1;
		public int? Id { get; set; } = null;
		public int? CheckListID { get; set; } //
		public int? CardPaymentId { get; set; } //
		public int? CashlessPaymentID { get; set; } //
		public int? PaymentTypeID { get; set; } = 3;
		public int? CashboxID { get; set; } //
		public bool? FiscalMode { get; set; } = true;
		public int? CertificateID { get; set; } = null;
		public int? DiscountID { get; set; } = null;
		public int? MedServReservedID { get; set; } = null;
		public int? SingleServiceReservedID { get; set; } = null;
		public int? SeasonOrderID { get; set; } = null;
		public int? CertificateReservedID { get; set; } = null;
		public int? JWSMoneyID { get; set; } = null;
		public int? JWSSingleServiceID { get; set; } = null;
		public int? CashboxOrderID { get; set; } = null;
		public int? PayCashboxOrderID { get; set; } = null;
		public int? SingleServiceUsedID { get; set; } //
		public double? Count { get; set; } //
		public double? PaymentPrice { get; set; } //
		public double? Price { get; set; } //
		public double? TotalPrice { get; set; } //
		public bool? BalanceTopUp { get; set; } = false;
		public int? PaySeasonOrderID { get; set; } = null;
		public int? EmployeeID { get; set; } = null;
		public int? ClientID { get; set; } = 1;
		public bool? WriteOff { get; set; } = false;
		public bool? Refund { get; set; } = false;
		public int? CashierID { get; set; } //
	}
}