using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebKassa.Models
{
    public class Products
    {
        public int? GoodId { get; set; }
        public string? Article {  get; set; }
        public string? Name {  get; set; }
        public string? ProductName {  get; set; }
        public double? Quantity {  get; set; }
        public double? Sum {  get; set; }
    }
}
