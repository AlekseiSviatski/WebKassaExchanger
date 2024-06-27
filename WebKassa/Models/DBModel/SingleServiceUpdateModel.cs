namespace WebKassa.Models.DBModel
{
    public class SingleServiceUpdateModel
    {
        public int? IdOrder { get; set; } = 1;
        public int? SingleServiceId { get; set; }
        public double? Count { get; set; }
        public double? Price { get; set; }
        public double? TotalPrice { get; set; }
        public double? DiscountPrice { get; set; } = 0;
        public int? EmployeeOrderId { get; set; } = 1;
        public int? SingleServiceResevedId { get; set; } = null;
        public bool? Paid { get; set; } = true;
        public double? PaidPrice { get; set; }
        public double? PaidCount { get; set; }
        public int? TerminalId { get; set; }

        public double? CashPrice { get; set; }
        public double? CardPrice { get; set; }
    }
}