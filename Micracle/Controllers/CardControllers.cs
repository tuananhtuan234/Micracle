using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Data.Entity;
using Services.Interface;

namespace Micracle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardControllers : ControllerBase
    {
        private readonly ICardServices _services;
        public CardControllers(ICardServices services)
        {
            _services = services;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var product = await _services.GetAllProduct();
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(string productId)
        {
            try
            {
                var products = await _services.GetProductById(productId);

                if (products == null)
                {
                    return NotFound();
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


    }
}
