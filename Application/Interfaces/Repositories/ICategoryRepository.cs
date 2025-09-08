using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ICategoryRepository : IRepositoryBase<Category>
    {
        // Aktif durumda olan kategorileri getirir
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();

        // Belirtilen isimdeki kategoriyi getirir
        Task<Category?> GetByNameAsync(string name);

        // Id ile kategori getirir
        Task<Category?> GetByIdAsync(int id);
    }
}
