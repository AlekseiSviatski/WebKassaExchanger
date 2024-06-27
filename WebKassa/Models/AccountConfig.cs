namespace WebKassa.Models
{
    public class AccountConfig
    {
        public string BaseUri { get; set; }
        public string APIKey { get; set; }
        public List<int> ProgramCashboxIds { get; set; }
        public List<CashboxPair> CashboxPairs { get; set; }
    }
}