using Application.Dtos.AuthDto;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.DataBase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
       public UserRepository(AppDbContext context) : base(context)
        {

        }
        // _context parametresi RepositoryBaseden geliyor. Kalıtım aldığım için kullanabiliyorum.
        public async Task<User> FindByIdAsync(long id)
        {
           return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
            
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
            return user;
        }
    }
}
