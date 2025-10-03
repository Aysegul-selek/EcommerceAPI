using Application.Dtos.AuthDto;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<User> FindByIdAsync(long id);
        Task<User?> GetByEmailAsync(string email);
        Task AddUserRoleAsync(long userId, long roleId);
        Task<List<UserRole>> GetUserRolesByRoleIdAsync(long roleId);
        Task<List<User>> GettAllUser();
    }
}
