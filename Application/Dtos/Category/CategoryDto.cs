using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Category
{
    public class CategoryDto
    {
        public int Id { get; set; } 
        public string Name { get; set; } = null!;
        public int ParentId { get; set; }
        public bool IsActive { get; set; }
    }
}
