using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.RoleDto
{
    public class RoleReadDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public int UserCount { get; set; }// rolu kullanan kullanıcı sayısı
    }
}
