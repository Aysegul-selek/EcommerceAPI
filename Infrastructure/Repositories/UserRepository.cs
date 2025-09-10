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
        public async Task<User?> FindByIdAsync(long id)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
        }


        public async Task<User?> GetByEmailAsync(string email)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
            return user;
        }

        public async Task AddUserRoleAsync(long userId, long roleId)
        {
            // Duplicate kontrolü (aynı kaydı eklememek için)
            var exists = await _context.UserRoles
                .AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

            if (exists)
                return; // zaten varsa tekrar eklemiyoruz

            var userRole = new UserRole
            {
                UserId = userId,
                RoleId = roleId
            };

            await _context.UserRoles.AddAsync(userRole);
            await _context.SaveChangesAsync();
        }



    }
}
