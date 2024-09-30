using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace Micracle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderControllers : ControllerBase
    {
        private readonly IOrderServices _orderServices;

        public OrderControllers(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrder()
        {
            try
            {
            var order = await _orderServices.GetAllOrders();
            if (order == null)
            {
                return NotFound("order not found");
            }
            return Ok(order);
            }catch (Exception ex)
            {
               return BadRequest(ex.Message);   
            }
        }


    }
}
