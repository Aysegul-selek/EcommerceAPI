using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.Category;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface ICategoryService
    {
        // Tum kategorileri getirir
        Task<IEnumerable<CategoryDto>> GetAllAsync();

        // Aktif kategorileri getirir
        Task<IEnumerable<CategoryDto>> GetActiveCategoriesAsync();

        // Id ile kategori getirir
        Task<CategoryDto?> GetByIdAsync(int id);

        // Yeni kategori ekler
        Task<CategoryDto> CreateAsync(CreateCategoryDto categoryDto);

        // Mevcut kategoriyi guncller
        Task UpdateAsync(UpdateCategoryDto categoryDto);

        // Kategoriyi siler
        Task DeleteAsync(int id);

        // İsim ile kategori getirir
        Task<CategoryDto?> GetByNameAsync(string name);

        // Category Tree
        Task<IEnumerable<CategoryTreeDto>> GetCategoryTreeAsync();

    }
}
