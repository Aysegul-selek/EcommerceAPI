using Application.Dtos.Category;
using Application.Dtos.Pagination;
using Application.Dtos.ResponseDto;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/v1/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET /api/v1/categories
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            var categories = await _categoryService.GetAllAsync(filter);

            var response = new ApiResponseDto<PagedResponse<CategoryDto>>
            {
                Success = true,
                Message = "Categories fetched successfully",
                Data = categories
            };

            return Ok(response);
        }


        // GET /api/v1/categories/active
        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var categories = await _categoryService.GetActiveCategoriesAsync();

            var response = new ApiResponseDto<IEnumerable<CategoryDto>>
            {
                Success = true,
                Message = "Active categories fetched successfully",
                Data = categories
            };

            return Ok(response);
        }

        // GET /api/v1/categories/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound(new ApiResponseDto<CategoryDto>
                {
                    Success = false,
                    Message = $"Category with id {id} not found."
                });
            }

            var response = new ApiResponseDto<CategoryDto>
            {
                Success = true,
                Message = "Category fetched successfully",
                Data = category
            };

            return Ok(response);
        }

        // POST /api/v1/categories
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponseDto<object>
                {
                    Success = false,
                    Message = "Invalid model state"
                });
            }

            var created = await _categoryService.CreateAsync(dto);

            var response = new ApiResponseDto<CategoryDto>
            {
                Success = true,
                Message = "Category created successfully",
                Data = created
            };

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
        }

        // PUT /api/v1/categories/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(new ApiResponseDto<object>
                {
                    Success = false,
                    Message = "Id mismatch."
                });
            }

            await _categoryService.UpdateAsync(dto);

            return Ok(new ApiResponseDto<object>
            {
                Success = true,
                Message = "Category updated successfully"
            });
        }

        // DELETE /api/v1/categories/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteAsync(id);

            return Ok(new ApiResponseDto<object>
            {
                Success = true,
                Message = "Category deleted successfully"
            });
        }

        // GET /api/v1/categories/tree
        [HttpGet("tree")]
        public async Task<IActionResult> GetTree()
        {
            var categoryTree = await _categoryService.GetCategoryTreeAsync();

            var response = new ApiResponseDto<IEnumerable<CategoryTreeDto>>
            {
                Success = true,
                Message = "Category tree fetched successfully",
                Data = categoryTree
            };

            return Ok(response);
        }
    }
}
