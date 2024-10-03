using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Data.DTOs;
using Services;
using Services.Interface;

namespace Micracle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderProductController : ControllerBase
    {
        private readonly IOrderProductServices _services;
        public OrderProductController(IOrderProductServices services)
        {
            _services = services;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrderProduct()
        {
            try
            {
                var results = await _services.GetAllOrderProducts();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetOrderProductById(string orderId)
        {
            try
            {
                if (orderId == null)
                {
                    return BadRequest("Please enter orderId");
                }
                var results = await _services.GetByOrderProductById(orderId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddOrderProduct(OrderProductRequest orderProductRequest)
        {
            try
            {
                var reuslt = await _services.AddOrderProduct(orderProductRequest);
                return Ok(reuslt);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrderProduct(string OrderProductId, OrderProductRequestDtos orderProductRequest)
        {
            try
            {
                var results = await _services.UpdateOrderProduct(OrderProductId, orderProductRequest);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOrderProduct(string orderProductId)
        {
            try
            {
                if (orderProductId == null)
                {
                    return BadRequest("Please enter orderProductId");
                }
                await _services.DeleteOrderProduct(orderProductId);
                return Ok("Delete success");
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
