using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos.Category;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        // Cache key isimleri
        private const string AllCategoriesCacheKey = "AllCategories";
        private const string ActiveCategoriesCacheKey = "ActiveCategories";

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, IMemoryCache cache)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            if (!_cache.TryGetValue(AllCategoriesCacheKey, out IEnumerable<CategoryDto> categoriesDto))
            {
                var categories = await _categoryRepository.GetAllAsync();
                categoriesDto = _mapper.Map<IEnumerable<CategoryDto>>(categories);

                _cache.Set(AllCategoriesCacheKey, categoriesDto, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                    SlidingExpiration = TimeSpan.FromMinutes(30)
                });
            }

            return categoriesDto;
        }

        public async Task<IEnumerable<CategoryDto>> GetActiveCategoriesAsync()
        {
            if (!_cache.TryGetValue(ActiveCategoriesCacheKey, out IEnumerable<CategoryDto> activeCategoriesDto))
            {
                var categories = await _categoryRepository.GetActiveCategoriesAsync();
                activeCategoriesDto = _mapper.Map<IEnumerable<CategoryDto>>(categories);

                _cache.Set(ActiveCategoriesCacheKey, activeCategoriesDto, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                    SlidingExpiration = TimeSpan.FromMinutes(30)
                });
            }

            return activeCategoriesDto;
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var cacheKey = $"Category_{id}";
            if (!_cache.TryGetValue(cacheKey, out CategoryDto? categoryDto))
            {
                var category = await _categoryRepository.GetByIdAsync(id);
                categoryDto = _mapper.Map<CategoryDto?>(category);

                if (categoryDto != null)
                {
                    _cache.Set(cacheKey, categoryDto, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                        SlidingExpiration = TimeSpan.FromMinutes(30)
                    });
                }
            }

            return categoryDto;
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            await _categoryRepository.AddAsync(category);

            // Cache’i temizle
            _cache.Remove(AllCategoriesCacheKey);
            _cache.Remove(ActiveCategoriesCacheKey);

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task UpdateAsync(UpdateCategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            await _categoryRepository.Update(category);

            // Cache’i temizle
            _cache.Remove(AllCategoriesCacheKey);
            _cache.Remove(ActiveCategoriesCacheKey);
            _cache.Remove($"Category_{categoryDto.Id}");
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category != null)
            {
                await _categoryRepository.Delete(category);

                // Cache’i temizle
                _cache.Remove(AllCategoriesCacheKey);
                _cache.Remove(ActiveCategoriesCacheKey);
                _cache.Remove($"Category_{id}");
            }
        }

        public async Task<CategoryDto?> GetByNameAsync(string name)
        {
            var category = await _categoryRepository.GetByNameAsync(name);
            return _mapper.Map<CategoryDto?>(category);
        }

        public async Task<IEnumerable<CategoryTreeDto>> GetCategoryTreeAsync()
        {
            var activeCategories = await GetActiveCategoriesAsync();

            // DTO listesi
            var categoryTreeList = activeCategories.Select(c => new CategoryTreeDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();

            // Id -> DTO lookup
            var lookup = categoryTreeList.ToDictionary(c => c.Id);

            foreach (var c in activeCategories)
            {
                if (c.ParentId != 0 && lookup.TryGetValue(c.ParentId, out var parent))
                {
                    parent.Children.Add(lookup[c.Id]);
                }
            }

            // Root kategoriler
            var roots = categoryTreeList
                .Where(c => activeCategories.First(ac => ac.Id == c.Id).ParentId == 0)
                .ToList();

            return roots;
        }


    }
}
