using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IOrderRepository:IRepositoryBase<Order>
    {
        IQueryable<Order> GetAllQueryable();
        Task<Order?> FindByIdAsync(long id);
        Task AddOrderAsync(Order order);
        Task SaveChangesAsync();
    }

}
