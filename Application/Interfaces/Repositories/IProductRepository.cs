using Application.Dtos.Product;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        Task<Product?> GetBySkuAsync(int sku);
        Task<IEnumerable<Product>> GetAllActiveAsync();

        // Tuple dönüyoruz: (Filtrelenmiş ürünler, Toplam kayıt sayısı)
        Task<(IEnumerable<Product> Products, int TotalCount)> SearchProductsAsync(ProductSearchRequestDto request);

        // Aynı slug mevcut mu kontrol eder
        Task<bool> SlugExistsAsync(string slug);
    }
}
