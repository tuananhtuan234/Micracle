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
        Task Delete(string productId);
        Task Update(Product product);
        Task AddProduct(Product product);
        Task<Product> GetProductById(string ProductsId);
        Task<List<Product>> GetAllProduct();
    }
}
