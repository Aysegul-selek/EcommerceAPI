using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        Task<Product?> GetBySkuAsync(int sku);
        Task<IEnumerable<Product>> GetAllActiveAsync();
    }
}
