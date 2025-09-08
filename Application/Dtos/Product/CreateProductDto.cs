using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Product
{
    public class CreateProductDto
    {
        public int Sku { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stok { get; set; }
        public int CategoryId { get; set; }
        public bool IsActive { get; set; }
    }
}
