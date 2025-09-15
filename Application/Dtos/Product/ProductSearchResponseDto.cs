using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Product
{
    public class ProductSearchResponseDto
    {
        public int TotalCount { get; set; }
        public IEnumerable<ProductDto> Items { get; set; } = new List<ProductDto>();
    }
}
