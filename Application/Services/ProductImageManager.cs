using Application.Dtos.ResponseDto;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductImageManager : IProductImagesService
    {
        private readonly IProductImagesRepository _productImagesRepository;

        public ProductImageManager(IProductImagesRepository productImagesRepository)
        {
            _productImagesRepository = productImagesRepository;
        }

        public async Task AddProductImage(ProductImage productImage)
        {
            
            await _productImagesRepository.AddAsync(productImage);

        }
    }
}
