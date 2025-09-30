using Application.Dtos.Product;
using Application.Dtos.Product;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
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
            var productDtos = products.Select(p => new ProductReadDto
            {
                Id = p.Id,
                Sku = p.Sku,
                Name = p.Name,
                Slug = p.Slug,
                Price = p.Price,
                Stok = p.Stok,
                CategoryId = p.CategoryId,
                IsActive = p.IsActive,
                CreatedDate = p.CreatedDate,
                UpdatedDate = p.UpdatedDate,
                IsDeleted = p.IsDeleted,
                CreateUser = p.CreateUser,
                UpUser = p.UpUser,
                // Resim listesini de ayrıca ProductImageDto'ya eşle
                Images = p.Images.Select(i => new ProductImageDto
                {
                    Id = i.Id,
                    ImageUrl = i.ImageUrl
                }).ToList()
            }).ToList();
            return productDtos;
        }

        public async Task AddAsync(Product product)
        {

            // Slug üret
            if (string.IsNullOrWhiteSpace(product.Slug))
            {
                product.Slug = await GenerateUniqueSlugAsync(product.Name);
            }

            // Slug guard
            product.Slug = await GenerateUniqueSlugAsync(product.Name);


            await _productRepository.AddAsync(product);
        }

        public async Task UpdateAsync(Product product)
        {

            // Slug guard
            product.Slug = await GenerateUniqueSlugAsync(product.Name, product.Id);

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
            var (products, totalCount) = await _productRepository.SearchProductsAsync(request);

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

        // Slug Guard Helper
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

        public async Task<ProductReadDto> GetProductByIdsAsync(long id)
        {

            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                throw new NotFoundException($"{id} li product bulunamadı");
            }
            var productReadDto = new ProductReadDto
            {
                Id = product.Id,
                Sku = product.Sku,
                Name = product.Name,
                Slug = product.Slug,
                Price = product.Price,
                Stok = product.Stok,
                CategoryId = product.CategoryId,
                IsActive = product.IsActive,
                Images = product.Images.Select(img => new ProductImageDto
                {
                    Id = img.Id,
                    ImageUrl = img.ImageUrl
                }).ToList()
            };
            return productReadDto;
        }
    }
}
