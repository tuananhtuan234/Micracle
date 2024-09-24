using Micracle.DTOs;
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
    public class CardServices: ICardServices
    {
        private readonly ICardRepositories _repositories;

        public CardServices(ICardRepositories repositories)
        {
            _repositories = repositories;
        }

        public async Task<List<Product>> GetAllProduct(string searchterm)
        {
            return await _repositories.GetAllProducts(searchterm);
        }

        public async Task<Product> GetProductById(string ProductsId)
        {
            return await _repositories.GetProductsById(ProductsId);
        }

        public async Task AddProduct(ProductDTO productdto)
        {
             if (productdto == null)
            {
                throw new Exception("Prodcuts do not existed");
            }if (string.IsNullOrWhiteSpace(productdto.ProductName) || productdto.ProductName == "string")
            {
                throw new Exception("Products must be required");
            }
            Product newProduct = new Product()
            {
                ProductName = productdto.ProductName,
                Quantity = productdto.Quantity,
                Price = productdto.Price,
                Status = productdto.Status,
                CreatedBy = productdto.CreatedBy,
                UpdatedBy = productdto.UpdatedBy,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                SubCategoryId = productdto.SubCategoryId,
            };

            await _repositories.AddProducts(newProduct);    

        }

        public async Task<string> Update(string productId, ProductRequestDtos product)
        {
            var existingCard = await _repositories.GetProductsById(productId);
            if (existingCard == null)
            {
               return "Card not found";
            }
            else 
            {
                existingCard.ProductName = product.ProductName;    
                existingCard.Quantity = product.Quantity;
                existingCard.Price = product.Price;  
                existingCard.Status = product.Status;
                existingCard.UpdatedDate = DateTime.Now;          
                existingCard.UpdatedBy = product.UpdatedBy;
                var result = await _repositories.UpdateProducts(existingCard);
                return result ? "Update Successful" : "Update failed";
            }

        }

        public async Task<string> Delete(string productId)
        {
           Product card = await _repositories.GetProductsById(productId);
            if (card == null)
            {
                return "Card not found";
            }
            await _repositories.DeleteProducts(productId);
            return "Delete success";
        }
    }
}
