using Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.DataBase;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        // Aktif kategorileri döndürür
        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
        {
            return await _context.Set<Category>()
                                 .AsNoTracking()
                                 .Where(c => !c.IsDeleted && c.IsActive)
                                 .ToListAsync();
        }

        // İsimle kategori getirir
        public async Task<Category?> GetByNameAsync(string name)
        {
            return await _context.Set<Category>()
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(c => !c.IsDeleted && c.Name == name);
        }
        // Id ile kategori getirir
        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Set<Category>()
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(c => !c.IsDeleted && c.Id == id);
        }
    }
}
