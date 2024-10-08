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
    public class OrderServices : IOrderServices
    {
        private readonly IOrderRepository _repository;
        private readonly IOrderProductRepository _orderProductRepository;
        public OrderServices(IOrderRepository repository, IOrderProductRepository orderProductRepository)
        {
            _repository = repository;
            _orderProductRepository = orderProductRepository;
        }

        public async Task<string> AddOrder(OrderDto orderDto)
        {
            if (orderDto == null)
            {
                return "Data null";
            }
            Order order = new Order()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = orderDto.UserId,
                OrderDate = DateTime.Now,
                Status = orderDto.Status,
                TotalPrice = 0,
            };
            var orderProduct = await _orderProductRepository.GetAllOrderProductByOrderId(order.Id);
            order.TotalPrice = orderProduct.Sum(op => op.Price * op.Quantity);
            var result = await _repository.AddOrder(order);
            return result ? "Add Suucess" : "Add failed";
        }

        public async Task DeleteOrder(string orderId)
        {
            await _repository.DeleteOrder(orderId);
        }

        public async Task<List<Order>> GetAllOrders()
        {
            return await _repository.GetAllOrder();
        }
        public async Task<Order> GetOrderById(string orderId)
        {
            return await _repository.GetOrderById(orderId);
        }

        public async Task<string> Update(string orderId, UpdateOrderDtos orderDto)
        {
            var existingOrder = await _repository.GetOrderById(orderId);
            var listOrderProduct = await _orderProductRepository.GetAllOrderProductByOrderId(orderId);
            
            if (existingOrder == null)
            {
                return "Order not found";
            }
            existingOrder.OrderDate = DateTime.Now;
            existingOrder.Status = orderDto.Status;
            existingOrder.TotalPrice = listOrderProduct.Sum(sc => sc.Price * sc.Quantity);

            var result = await _repository.UpdateOrder(existingOrder);
            return result ? "Update Success" : "update failed";
        }
    }
}
