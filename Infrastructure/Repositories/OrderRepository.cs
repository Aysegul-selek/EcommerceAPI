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
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }

        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task<Order?> FindByIdAsync(long id)
        {
            return await _context.Orders
               .Include(o => o.Items)
               .AsNoTracking()
               .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);
        }

        public async Task<List<Order>> GetAllWithItemsAsync()
        {
            return await _context.Orders
                .Include(o => o.Items)
                .Where(o => !o.IsDeleted)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}
