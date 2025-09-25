using Application.Dtos.Product;
using Application.Dtos.ResponseDto;
using Application.Interfaces.Services;
using Domain.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly S3Service _s3Service;

        public ProductController(IProductService productService, S3Service s3Service)
        {
            _productService = productService;
            _s3Service = s3Service;
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

        [HttpPost("{productId}/upload-image")]
        public async Task<ActionResult<ApiResponseDto<string>>> UploadProductImage(long productId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new ApiResponseDto<string>
                {
                    Success = false,
                    Message = "Dosya bulunamadı"
                });
            }

            var product = await _productService.GetByIdAsync(productId);
            if (product == null)
            {
                return NotFound(new ApiResponseDto<string>
                {
                    Success = false,
                    Message = "Ürün bulunamadı"
                });
            }

            // Dosya ismi ve klasörleme
            var fileName = $"products/{productId}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            // S3'e yükle
            await using var stream = file.OpenReadStream();
            var imageUrl = await _s3Service.UploadFileAsync(stream, fileName, file.ContentType);

            // ProductImage entity'sini oluştur ve ürüne ekle
            var productImage = new ProductImage
            {
                ProductId = productId,
                ImageUrl = imageUrl
            };
            product.Images.Add(productImage);

            // Ürünü güncelle
            await _productService.UpdateAsync(product);

            return Ok(new ApiResponseDto<string>
            {
                Success = true,
                Message = "Resim başarıyla yüklendi",
                Data = imageUrl
            });
        }



        /// <summary>
        /// Tüm aktif ürünleri getir
        /// </summary>

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllActiveAsync();
            return Ok(new ApiResponseDto<IEnumerable<ProductReadDto>>
            {
                Success = true,
                Data = products
            });
        }


        /// <summary>
        /// Ürün detayı
        /// </summary>

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseDto<ProductReadDto>>> GetById(long id)
        {
            var product = await _productService.GetProductByIdsAsync(id);
            if (product == null)
                return NotFound(new ApiResponseDto<Product>
                {
                    Success = false,
                    Message = "Ürün bulunamadı"
                });

            return Ok(new ApiResponseDto<ProductReadDto>
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
