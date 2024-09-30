using Repositories.Data.DTOs;
using Repositories.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IOrderServices
    {
        Task<List<Order>> GetAllOrders();
        Task<Order> GetOrderById(string orderId);
        Task<string> AddOrder(OrderDto orderDto);
        Task DeleteOrder(string orderId);
        Task<string> Update(string orderId, UpdateOrderDtos orderDto);   
    }
}
