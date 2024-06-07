namespace WebKassa.Models.DBModel
{
    public class CashboxCheckListEditModel
    {
        public int? IdOrder { get; set; } = null;
        public int? IdSeasonOrder { get; set; } = null;
        public int? CashboxPaymentTypeId { get; set; } = 3;
        public double? Price { get; set; }
        public double? TotalModey { get; set; }
        public string? TypeSystem { get; set; } = "web";
        public int? IdCashier { get; set; }
        public bool? Mode { get; set; } = true; //фискальный (1) / не фискальный (0)
        public int? CashboxId { get; set; }
    }
}
