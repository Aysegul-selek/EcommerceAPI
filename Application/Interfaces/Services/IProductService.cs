using Application.Dtos.Product;
using Application.Dtos.Product;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<Product?> GetByIdAsync(long id);
        Task<Product?> GetBySkuAsync(int sku);
        Task<IEnumerable<ProductReadDto>> GetAllActiveAsync();
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(long id);

        //sayfalama + ürünler bir arada dönecek
        Task<ProductSearchResponseDto> SearchProductsAsync(ProductSearchRequestDto request);

        // --- Slug guard ---
        Task<string> GenerateUniqueSlugAsync(string slug);
        Task<ProductReadDto> GetProductByIdsAsync(long id);

        //Tüm ürünleri pagination ile getirir
        Task<PagedResponse<ProductDto>> GetAllPagedAsync(int pageNumber, int pageSize);

    }
}
