using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.RoleDto
{
    public class DeleteRoleDto
    {
        public long userId { get; set; }
       
        public long roleId { get; set; }
    }
}
