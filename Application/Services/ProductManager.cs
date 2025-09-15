using Application.Dtos.Product;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductManager : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductManager(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Product?> GetByIdAsync(long id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<Product?> GetBySkuAsync(int sku)
        {
            return await _productRepository.GetBySkuAsync(sku);
        }

        public async Task<IEnumerable<Product>> GetAllActiveAsync()
        {
            return await _productRepository.GetAllActiveAsync();
        }

        public async Task AddAsync(Product product)
        {
            await _productRepository.AddAsync(product);
        }

        public async Task UpdateAsync(Product product)
        {
            await _productRepository.Update(product);
        }

        public async Task DeleteAsync(long id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product != null)
            {
                product.IsDeleted = true;
                await _productRepository.Update(product);
            }
        }

        // Gelişmiş arama/filtreleme/sıralama/sayfalama
        public async Task<ProductSearchResponseDto> SearchProductsAsync(ProductSearchRequestDto request)
        {
            // Repository'den ürünleri ve toplam sayıyı alıyoruz
            var (products, totalCount) = await _productRepository.SearchProductsAsync(request);


            // Entity -> DTO
            var items = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Sku = p.Sku,
                Name = p.Name,
                Slug = p.Slug,
                Price = p.Price,
                Stok = p.Stok,
                CategoryId = p.CategoryId,
                IsActive = p.IsActive
            });

            return new ProductSearchResponseDto
            {
                Items = items,
                TotalCount = totalCount
            };
        }
    }
}
