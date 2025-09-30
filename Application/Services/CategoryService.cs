using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos.Category;
using Application.Dtos.Pagination;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly ILogger<CategoryService> _logger;

        // Cache key isimleri
        private const string AllCategoriesCacheKey = "AllCategories";
        private const string ActiveCategoriesCacheKey = "ActiveCategories";
        private const string CategoryTreeCacheKey = "CategoryTree";

        private readonly MemoryCacheEntryOptions _cacheOptions =
            new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                SlidingExpiration = TimeSpan.FromMinutes(30)
            };

        public CategoryService(
            ICategoryRepository categoryRepository,
            IMapper mapper,
            IMemoryCache cache,
            ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _cache = cache;
            _logger = logger;
        }

        public async Task<PagedResponse<CategoryDto>> GetAllAsync(PaginationFilter filter)
        {
            if (!_cache.TryGetValue(AllCategoriesCacheKey, out IEnumerable<CategoryDto> categoriesDto))
            {
                _logger.LogInformation("CACHE MISS: {key}", AllCategoriesCacheKey);

                var categories = await _categoryRepository.GetAllAsync();
                categoriesDto = _mapper.Map<IEnumerable<CategoryDto>>(categories);

                _cache.Set(AllCategoriesCacheKey, categoriesDto, _cacheOptions);
            }
            else
            {
                _logger.LogInformation("CACHE HIT: {key}", AllCategoriesCacheKey);
            }

            // Memory üzerinde pagination
            var totalRecords = categoriesDto.Count();
            var pagedData = categoriesDto
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            return new PagedResponse<CategoryDto>(pagedData, totalRecords, filter.PageNumber, filter.PageSize);
        }


        public async Task<IEnumerable<CategoryDto>> GetActiveCategoriesAsync()
        {
            if (!_cache.TryGetValue(ActiveCategoriesCacheKey, out IEnumerable<CategoryDto> activeCategoriesDto))
            {
                _logger.LogInformation("CACHE MISS: {key}", ActiveCategoriesCacheKey);
                var categories = await _categoryRepository.GetActiveCategoriesAsync();
                activeCategoriesDto = _mapper.Map<IEnumerable<CategoryDto>>(categories);
                _cache.Set(ActiveCategoriesCacheKey, activeCategoriesDto, _cacheOptions);
            }
            else
            {
                _logger.LogInformation("CACHE HIT: {key}", ActiveCategoriesCacheKey);
            }

            return activeCategoriesDto;
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var cacheKey = $"Category_{id}";
            if (!_cache.TryGetValue(cacheKey, out CategoryDto? categoryDto))
            {
                _logger.LogInformation("CACHE MISS: {key}", cacheKey);
                var category = await _categoryRepository.GetByIdAsync(id);
                categoryDto = _mapper.Map<CategoryDto?>(category);

                if (categoryDto != null)
                {
                    _cache.Set(cacheKey, categoryDto, _cacheOptions);
                }
            }
            else
            {
                _logger.LogInformation("CACHE HIT: {key}", cacheKey);
            }

            return categoryDto;
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            await _categoryRepository.AddAsync(category);

            InvalidateCategoryCaches(category.Id);
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task UpdateAsync(UpdateCategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            await _categoryRepository.Update(category);

            InvalidateCategoryCaches(categoryDto.Id);
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category != null)
            {
                await _categoryRepository.Delete(category);
                InvalidateCategoryCaches(id);
            }
        }

        public async Task<CategoryDto?> GetByNameAsync(string name)
        {
            var category = await _categoryRepository.GetByNameAsync(name);
            return _mapper.Map<CategoryDto?>(category);
        }

        public async Task<IEnumerable<CategoryTreeDto>> GetCategoryTreeAsync()
        {
            if (_cache.TryGetValue(CategoryTreeCacheKey, out IEnumerable<CategoryTreeDto>? cachedTree))
            {
                _logger.LogInformation("CACHE HIT: {key}", CategoryTreeCacheKey);
                return cachedTree!;
            }

            _logger.LogInformation("CACHE MISS: {key}", CategoryTreeCacheKey);
            var activeCategories = await GetActiveCategoriesAsync();

            // DTO listesi
            var categoryTreeList = activeCategories.Select(c => new CategoryTreeDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();

            // Lookup (id -> dto)
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

            _cache.Set(CategoryTreeCacheKey, roots, _cacheOptions);
            return roots;
        }

        // --- Centralized cache invalidation ---
        private void InvalidateCategoryCaches(long affectedCategoryId)
        {
            _logger.LogInformation("CACHE INVALIDATE for category id {id}", affectedCategoryId);

            _cache.Remove(AllCategoriesCacheKey);
            _cache.Remove(ActiveCategoriesCacheKey);
            _cache.Remove(CategoryTreeCacheKey);
            _cache.Remove($"Category_{affectedCategoryId}");
        }

       
    }
}
