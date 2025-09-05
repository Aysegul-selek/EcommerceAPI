using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order : BaseEntity
    {
        public string OrderNo { get; set; }
        public long UserId { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
        public DateTime? PaidAt { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

    }
}
