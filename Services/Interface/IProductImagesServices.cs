using Repositories.Data.DTOs;
using Repositories.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IProductImagesServices
    {
        Task<List<ProductImage>> GetAllProductImages();
        
        Task<ProductImage> GetProductImageById(string id);
        Task AddProductImages(ProductImagesRequestDtos porductImagesRequestDtos);
    }
}
