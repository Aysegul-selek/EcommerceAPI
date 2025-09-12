using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Category
{
    public class CategoryTreeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<CategoryTreeDto> Children { get; set; } = new List<CategoryTreeDto>();
    }
}
