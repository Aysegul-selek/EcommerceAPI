using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProductImage : BaseEntity
    {
        public long ProductId { get; set; }
        public string ImageUrl { get; set; }  
        public Product Product { get; set; }
    }
}
