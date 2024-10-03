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
    public class OrderProductServices: IOrderProductServices
    {
        private readonly IOrderProductRepository _repository;
        private readonly ICardRepositories _cardRepositories;
        

        public OrderProductServices(IOrderProductRepository repository, ICardRepositories cardRepositories)
        {
            _repository = repository;
            _cardRepositories = cardRepositories;
        }

        public async Task<string> AddOrderProduct(OrderProductRequest orderProductRequest)
        {
            var card = await _cardRepositories.GetProductsById(orderProductRequest.ProductId);
            if (orderProductRequest == null)
            {
                return "Data not have";
            }
            if(orderProductRequest.Quantity > card.Quantity)
            {
                return "Not have enough quantity for this card";
            }
            if(card.Status == "Disable")
            {
                return "This product is not existed";
            }
            OrderProduct orderProduct = new OrderProduct()
            { 
                Id = Guid.NewGuid().ToString(),
                OrderId = orderProductRequest.OrderId,
                ProductId = orderProductRequest.ProductId,
                Quantity = orderProductRequest.Quantity,
                Price = card.Price * orderProductRequest.Quantity,
            };
            var result = await _repository.AddOrderProduct(orderProduct);
            return result ? "Add Successfull" : "Add failed";
        }

        public async Task DeleteOrderProduct(string orderProductId)
        {
          await _repository.DeleteOrderProductById(orderProductId);
        }

        public async Task<List<OrderProduct>> GetAllOrderProducts()
        {
            return await _repository.GetAllOrderProduct();
        }

        public async Task<OrderProduct> GetByOrderProductById(string orderProductId)
        {
           return await _repository.GetOrderProductById(orderProductId);
        }

        public async Task<string> UpdateOrderProduct(string orderproductId, OrderProductRequestDtos orderProductRequest)
        {
           var existingOrderProduct = await GetByOrderProductById(orderproductId);
            var card = await _cardRepositories.GetProductsById(existingOrderProduct.ProductId);
            if (existingOrderProduct != null)
            {
                return "order product not found";
            }
            if(orderProductRequest.Quantity > card.Quantity)
            {
                return "Not have enough quantity for this card";
            }
            if (card.Status == "Disable")
            {
                return "This product is not existed";
            }
            existingOrderProduct.Quantity = orderProductRequest.Quantity;
            existingOrderProduct.Price = card.Price * orderProductRequest.Quantity;

            var results = await _repository.UpdateOrderProduct(existingOrderProduct);
            return results ? "Update Success" : "Update failed";
        }
    }
}
