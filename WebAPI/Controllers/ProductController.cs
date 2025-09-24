using Application.Dtos.ResponseDto;
using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        /// <summary>
        /// Ürün ekle
        /// </summary>
        // Ürün ekle
        [HttpPost]
        public async Task<ActionResult<ApiResponseDto<Product>>> AddProduct([FromBody] Product product)
        {
            // Benzersiz slug üret
            product.Slug = await _productService.GenerateUniqueSlugAsync(product.Slug);

            await _productService.AddAsync(product);
            return Ok(new ApiResponseDto<Product>
            {
                Success = true,
                Message = "Ürün başarıyla eklendi",
                Data = product
            });
        }


        /// <summary>
        /// Tüm aktif ürünleri getir
        /// </summary>

        [HttpGet]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<Product>>>> GetAll()
        {
            var products = await _productService.GetAllActiveAsync();
            return Ok(new ApiResponseDto<IEnumerable<Product>>
            {
                Success = true,
                Data = products
            });
        }


        /// <summary>
        /// Ürün detayı
        /// </summary>

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseDto<Product>>> GetById(long id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound(new ApiResponseDto<Product>
                {
                    Success = false,
                    Message = "Ürün bulunamadı"
                });

            return Ok(new ApiResponseDto<Product>
            {
                Success = true,
                Data = product
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponseDto<Product>>> Update(long id, [FromBody] Product product)
        {
            var existing = await _productService.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new ApiResponseDto<Product>
                {
                    Success = false,
                    Message = "Ürün bulunamadı"
                });

            // Güncellenebilir alanları ata
            existing.Name = product.Name;


            // Slug değiştiyse benzersizleştir
            if (existing.Slug != product.Slug)
            {
                existing.Slug = await _productService.GenerateUniqueSlugAsync(product.Slug);
            }

            existing.Slug = product.Slug;
            existing.Price = product.Price;
            existing.Stok = product.Stok;
            existing.CategoryId = product.CategoryId;
            existing.IsActive = product.IsActive;

            await _productService.UpdateAsync(existing);

            return Ok(new ApiResponseDto<Product>
            {
                Success = true,
                Message = "Ürün güncellendi",
                Data = existing
            });
        }


        /// <summary>
        /// Ürün sil (soft delete)
        /// </summary>

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponseDto<Product>>> Delete(long id)
        {
            await _productService.DeleteAsync(id);
            return Ok(new ApiResponseDto<Product>
            {
                Success = true,
                Message = "Ürün silindi"
            });
        }
    }
}
