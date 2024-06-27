using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebKassa.Models
{
    public class SalesUpdateModel
    {
        public string? Uid { get; set; }
        public int? CashierId { get; set; }
        public string? CashierName { get; set; }
        public double? TotalSum { get; set; }
        public double? TotalSumCash { get; set; }
        public double? TotalSumNoCash { get; set; }
        public List<Products>? Products { get; set; }
    }
}
