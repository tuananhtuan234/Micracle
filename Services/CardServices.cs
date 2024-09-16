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
    public class CardServices: ICardServices
    {
        private readonly ICardRepositories _repositories;

        public CardServices(ICardRepositories repositories)
        {
            _repositories = repositories;
        }

        public async Task<List<Product>> GetAllProduct()
        {
            return await _repositories.GetAllProducts();
        }

        public async Task<Product> GetProductById(string ProductsId)
        {
            return await _repositories.GetProductsById(ProductsId);
        }

        public async Task AddProduct(Product product)
        {
             await _repositories.AddProducts(product);
        }

        public async Task Update(Product product)
        {
            await _repositories.UpdateProducts(product);
        }

        public async Task Delete(string productId)
        {
            await _repositories.DeleteProducts(productId);
        }
    }
}
