using Repositories.Data.DTOs;
using Repositories.Data.Entity;
using Repositories.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductImageServices: IProductImagesServices
    {
        private readonly IProductImagesRepository _productsRepository;

        public ProductImageServices(IProductImagesRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        public async Task AddProductImages(ProductImagesRequestDtos porductImagesRequestDtos)
        {
            if(porductImagesRequestDtos == null)
            {
                throw new Exception(" ProductImages not found");
            }
            ProductImage productImage = new ProductImage()
            {
                Id = Guid.NewGuid().ToString(),
                ImageId = porductImagesRequestDtos.ImageId,
                ProductId = porductImagesRequestDtos.ProductId,
            };
            await _productsRepository.AddProductWithImages(productImage);
        }

        public async Task<List<ProductImage>> GetAllProductImages()
        {
            return await _productsRepository.GetAllImagesAsync();
        }

        public async Task<ProductImage> GetProductImageById(string id)
        {
            return await _productsRepository.GetByIdAsync(id);
        }
    }
}
