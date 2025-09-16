using Application.Dtos.OrderItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Order
{
    public class CreateOrderDto
    {
        public List<CreateOrderItemDto> Items { get; set; } = new();
        public DiscountDto? Discount { get; set; } 

    }
}
