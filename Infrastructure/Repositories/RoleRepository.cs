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
    public class RoleRepository : RepositoryBase<Role>,IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<string>> GetRolesByUserIdAsync(long userId)
        {
            return await (from ur in _context.UserRoles
                          join r in _context.Roles on ur.RoleId equals r.Id
                          where ur.UserId == userId
                          select r.Name).ToListAsync();
        }

        public async Task<Role?> FindByIdAsync(long id)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            var roles = await _context.Roles
                .Include(r => r.UserRoles)
                .Where(r => !r.IsDeleted)
                .ToListAsync();
            return roles;
        }

        public async Task RemoveUserRoleAsync(long UserId, long RoleId)
        {

            var userRole = await _context.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == UserId && ur.RoleId == RoleId);
            if (userRole != null)
            {
                userRole.IsDeleted = true;
                _context.UserRoles.Update(userRole);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();

        }
    }
}
