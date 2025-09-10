using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IRoleRepository : IRepositoryBase<Role>
    {
        Task<IEnumerable<string>> GetRolesByUserIdAsync(long userId);
        Task<Role?> FindByIdAsync(long id);
        Task<IEnumerable<Role>> GetAllRolesAsync();
    }
}
