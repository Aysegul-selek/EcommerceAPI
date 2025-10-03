using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Product
{
    public  class ProductReadDto 
    {
        public int Id { get; set; }
        public int Sku { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public decimal Price { get; set; }
        public int Stok { get; set; }
        public int CategoryId { get; set; }
        public bool IsActive { get; set; }

       
        public List<ProductImageDto> Images { get; set; } = new List<ProductImageDto>();
    }
}
