using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.OrderItem
{
    public class CreateOrderItemDto
    {
        public long ProductId { get; set; }
        public string SKU { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
