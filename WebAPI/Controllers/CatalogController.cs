using Application.Dtos.Product;
using Application.Dtos.Product;
using Application.Interfaces.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/catalog")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public CatalogController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        // GET: api/v1/catalog?query=phone&categoryId=3&minPrice=100&maxPrice=1000&page=1&pageSize=20&sortBy=price&sortOrder=asc
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] ProductSearchRequestDto request)
        {
            // Service'den arama sonucu DTO olarak dönüyor
            var result = await _productService.SearchProductsAsync(request);

            return Ok(result);
        }
    }
}
