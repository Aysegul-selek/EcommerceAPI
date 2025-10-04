using Application.Dtos.Pagination;
using Application.Dtos.Product;
using Application.Exceptions;
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

        public async Task<IEnumerable<ProductReadDto>> GetAllActiveAsync()
        {
            var products = await _productRepository.GetAllActiveAsync();
            return _mapper.Map<IEnumerable<ProductReadDto>>(products);
        }

        public async Task AddAsync(CreateProductDto productDto)
        {
            // Slug üret (eğer boşsa)
            if (string.IsNullOrWhiteSpace(productDto.Slug))
            {
                productDto.Slug = await GenerateUniqueSlugAsync(productDto.Name);
            }

            var product = _mapper.Map<Product>(productDto);
            await _productRepository.AddAsync(product);
        }

        public async Task UpdateAsync(UpdateProductDto dto)
        {
            var existing = await _productRepository.GetByIdAsync(dto.Id);
            if (existing == null)
                throw new NotFoundException($"{dto.Id} li product bulunamadı");

            // Slug'ı güncelle ve benzersizleştir
            existing.Slug = await GenerateUniqueSlugAsync(dto.Slug, existing.Id);

            // Diğer alanları AutoMapper ile güncelle
            _mapper.Map(dto, existing);

            await _productRepository.Update(existing);
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

        public async Task<ProductSearchResponseDto> SearchProductsAsync(ProductSearchRequestDto request)
        {
            var (products, totalCount) = await _productRepository.SearchProductsAsync(request);

            var items = _mapper.Map<IEnumerable<ProductDto>>(products);

            return new ProductSearchResponseDto
            {
                Items = items,
                TotalCount = totalCount
            };
        }

        public async Task<ProductReadDto> GetProductByIdsAsync(long id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
                throw new NotFoundException($"{id} li product bulunamadı");

            return _mapper.Map<ProductReadDto>(product);
        }

        public async Task<PagedResponse<ProductDto>> GetAllPagedAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;

            var (products, totalCount) = await _productRepository.GetAllPagedAsync(pageNumber, pageSize);
            var productDtos = _mapper.Map<List<ProductDto>>(products);

            return new PagedResponse<ProductDto>(productDtos, totalCount, pageNumber, pageSize);
        }

        // --- Slug guard ---
        public async Task<string> GenerateUniqueSlugAsync(string slug)
        {
            var baseSlug = slug.Trim().ToLower().Replace(" ", "-");
            var uniqueSlug = baseSlug;
            int counter = 1;

            while (await _productRepository.SlugExistsAsync(uniqueSlug))
            {
                uniqueSlug = $"{baseSlug}-{counter}";
                counter++;
            }

            return uniqueSlug;
        }

        private async Task<string> GenerateUniqueSlugAsync(string name, long? excludeId = null)
        {
            string baseSlug = name.Trim().ToLower().Replace(" ", "-");
            string slug = baseSlug;
            int counter = 1;
            bool exists;

            do
            {
                exists = await _productRepository.SlugExistsAsync(slug, excludeId);

                if (exists)
                {
                    slug = $"{baseSlug}-{counter}";
                    counter++;
                }
            } while (exists);

            return slug;
        }
    }
}
