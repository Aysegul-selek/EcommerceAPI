using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Order
{
    public class DiscountDto
    {
        public string Type { get; set; } = ""; // "Percentage" veya "Fixed"
        public decimal Amount { get; set; }    // yüzde için 10 => %10, fixed için TL miktarı
    }
}
