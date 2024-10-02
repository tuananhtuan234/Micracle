using Micracle.DTOs;
using Repositories.Data.DTOs;
using Repositories.Data.Entity;
using Repositories.Enums;
using Repositories.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CardServices : ICardServices
    {
        private readonly ICardRepositories _repositories;
        private readonly IUserRepository _userRepository;

        public CardServices(ICardRepositories repositories, IUserRepository userRepository)
        {
            _repositories = repositories;
            _userRepository = userRepository;
        }

        public async Task<List<Product>> GetAllProduct(string? searchterm)
        {
            return await _repositories.GetAllProducts(searchterm);
        }

        public async Task<Product> GetProductById(string ProductsId)
        {
            return await _repositories.GetProductsById(ProductsId);
        }

        public async Task<string> AddProduct(string UserId, ProductDTO productdto)
        {
            var user = await _userRepository.GetUserById(UserId);
            if(user == null)
            {
                return "User Not found";
            }
            if (productdto == null)
            {
               return "Prodcuts do not existed";
            }
            if (string.IsNullOrWhiteSpace(productdto.ProductName) || productdto.ProductName == "string")
            {
                return "Products must be required"; 
            }
            if (productdto.Quantity < 0)
            {
                return "Quantity cannot be less than 0";
            }
            Product newProduct = new Product()
            {
                ProductName = productdto.ProductName,
                Quantity = productdto.Quantity,
                Price = productdto.Price,
                Status = ProductStatus.Active.ToString(),
                CreatedBy = user.FullName,
                UpdatedBy = null,
                CreatedDate = DateTime.Now,
                UpdatedDate = null,
                SubCategoryId = productdto.SubCategoryId,
            };

            var result = await _repositories.AddProducts(newProduct);
            return result ? "Add Successful" : "Add Failed";

        }

        public async Task<string> Update(string productId, string UserId, ProductRequestDtos product)
        {
            var user = await _userRepository.GetUserById(UserId);
            var existingCard = await _repositories.GetProductsById(productId);
            if (user == null)
            {
                return "User Not found";
            }
            else if (existingCard == null)
            {
                return "Card not found";
            }
            else if (product.Quantity < 0)
            {
                return "Quantity cannot be less than 0";
            }
            else
            {
                existingCard.ProductName = product.ProductName;
                existingCard.Quantity = product.Quantity;
                existingCard.Price = product.Price;
                existingCard.UpdatedDate = DateTime.Now;
                existingCard.UpdatedBy = user.FullName;
                if (product.Quantity == 0)
                {
                    existingCard.Status = ProductStatus.Disable.ToString();
                }
                else
                {
                    existingCard.Status = ProductStatus.Active.ToString();
                }
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
