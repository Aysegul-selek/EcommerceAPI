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

        public IQueryable<Order> GetAllQueryable()
        {
            return _context.Orders
                .Include(o => o.Items)   // Items’ları da yükle
                .AsNoTracking()
                .Where(o => !o.IsDeleted);
        }


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}
