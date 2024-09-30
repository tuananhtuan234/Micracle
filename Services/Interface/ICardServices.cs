using Micracle.DTOs;
using Repositories.Data.DTOs;
using Repositories.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface ICardServices
    {
        Task<string> Update(string productId, ProductRequestDtos product);
        Task AddProduct(ProductDTO productdto);
        Task<Product> GetProductById(string ProductsId);
        Task<List<Product>> GetAllProduct(string searchterm);
        Task<string> Delete(string productId);     
    }
}
