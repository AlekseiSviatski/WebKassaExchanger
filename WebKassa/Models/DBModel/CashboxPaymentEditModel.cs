namespace WebKassa.Models.DBModel
{
    public class CashboxPaymentEditModel
    {
        public int? CashboxPaymentTypeId { get; set; }
        public double? Price { get; set; }
        public int? CodeAuthorization { get; set; } = 0;
        public int? CodeCard { get; set; } = 0;
        public int? IdOrder { get; set; } = null;
        public int? IdSeasonOrder { get; set; } = null;
        public int? IdCashier { get; set; } //
        public int? CashboxId { get; set; } //
        public int? CardTypeId { get; set; } = 1;

    }
}