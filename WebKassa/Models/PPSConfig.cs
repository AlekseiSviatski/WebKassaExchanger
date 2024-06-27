namespace WebKassa.Models
{
    public class PPSConfig
    {
        public int? EmployeeId { get; set; }
        public int? CashierId { get; set; }
        public TimeSpan? AutoimportTime {  get; set; }
    }
}