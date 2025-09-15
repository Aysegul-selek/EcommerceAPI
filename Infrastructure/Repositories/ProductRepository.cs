using Domain.Entities;
using Application.Interfaces.Repositories;
using Infrastructure.DataBase;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos.Product;

namespace Infrastructure.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Product?> GetBySkuAsync(int sku)
        {
            return await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Sku == sku && !p.IsDeleted && p.IsActive);
        }

        public async Task<IEnumerable<Product>> GetAllActiveAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => !p.IsDeleted && p.IsActive)
                .ToListAsync();
        }

        // Gelişmiş arama ve paging
        public async Task<(IEnumerable<Product> Products, int TotalCount)> SearchProductsAsync(ProductSearchRequestDto request)
        {
            var query = _context.Products
                .AsNoTracking()
                .Where(p => !p.IsDeleted && p.IsActive);

            // Metin araması: Name, Slug veya SKU
            if (!string.IsNullOrWhiteSpace(request.Query))
            {
                var q = request.Query.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(q)
                                      || p.Slug.ToLower().Contains(q)
                                      || p.Sku.ToString().Contains(q));
            }

            // Fiyat filtresi
            if (request.MinPrice.HasValue)
                query = query.Where(p => p.Price >= request.MinPrice.Value);
            if (request.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= request.MaxPrice.Value);

            // Kategori filtresi
            if (request.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == request.CategoryId.Value);

            // Sadece stokta olanları filtrele
            if (request.InStock.HasValue && request.InStock.Value)
                query = query.Where(p => p.Stok > 0);

            // Toplam kayıt sayısı
            var totalCount = await query.CountAsync();

            // Sıralama
            query = request.SortBy?.ToLower() switch
            {
                "price" => request.SortOrder == "desc" ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                "name" => request.SortOrder == "desc" ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
                "createddate" => request.SortOrder == "desc" ? query.OrderByDescending(p => p.CreatedDate) : query.OrderBy(p => p.CreatedDate),
                "stok" => request.SortOrder == "desc" ? query.OrderByDescending(p => p.Stok) : query.OrderBy(p => p.Stok),
                _ => query.OrderBy(p => p.Id)
            };

            // Paging
            var skip = (request.Page - 1) * request.PageSize;
            var products = await query.Skip(skip).Take(request.PageSize).ToListAsync();

            return (products, totalCount);
        }
    }
}
