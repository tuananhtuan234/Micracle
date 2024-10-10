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
    public class OrderProductServices : IOrderProductServices
    {
        private readonly IOrderProductRepository _repository;
        private readonly ICardRepositories _cardRepositories;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;


        public OrderProductServices(IOrderProductRepository repository, ICardRepositories cardRepositories, IOrderRepository orderRepository, ICartRepository cartRepository)
        {
            _repository = repository;
            _cardRepositories = cardRepositories;
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
        }

        public async Task<string> AddOrderProduct(string userId, string OrderId)
        {
            var order = await _orderRepository.GetOrderById(OrderId);
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (order == null)
            {
                // tạo order khi không có order
                order = new Order()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    Status = 0, // order mới tạo
                    OrderProducts = new List<OrderProduct>(),
                    TotalPrice = 0,
                };
                await _orderRepository.AddOrder(order);
            }
            // tạo order product dựa trên cartProduct
            var orderProduct = cart.CartProducts.Select(cartProduct => new OrderProduct
            {
                OrderId = order.Id,
                ProductId = cartProduct.ProductId,
                Quantity = cartProduct.Quantity,
                Price = cartProduct.Price,
            }).ToList();
            await _repository.AddListOrderProduct(orderProduct);

            var existingOrder = await _orderRepository.GetOrderById(order.Id);

            // kiểm tra Order có OrderProduct hay không
            if(orderProduct == null || !orderProduct.Any())
            {
                await _orderRepository.DeleteOrder(existingOrder.Id);
                return "Cart dont have any product. Please add some Products";
            }

            // cập nhật tổng giá tiền khi có order Product
            existingOrder.TotalPrice = order.OrderProducts.Sum(product => product.Price);
            await _orderRepository.UpdateOrder(existingOrder);  

            // Xóa Cart product khi người dùng nhập order thành công
            await _cartRepository.RemoveCartProducts(cart.Id);
            return "Add List Order Product Successful";

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
            if (orderProductRequest.Quantity > card.Quantity)
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
