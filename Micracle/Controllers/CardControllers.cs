using Micracle.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Data.DTOs;
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
        public async Task<IActionResult> GetAllProducts(string? searchterm)
        {
            try
            {
                var product = await _services.GetAllProduct(searchterm);
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

        [HttpPost]
        public async Task<IActionResult> AddProducts(ProductDTO productdto)
        {
            if (productdto == null)
            {
                return BadRequest("Product data is null");
            }

            try
            {
                await _services.AddProduct(productdto);
                return Ok("Product added successfully");
            }
            catch (Exception ex)
            {
                //Trả về lỗi nếu có exception
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(string productIds, ProductRequestDtos requestDtos)
        {
            if(productIds == null)
            {
                return BadRequest("You need enter Id of product");
            }
            try
            {
                var result = await _services.Update(productIds, requestDtos);
                return Ok(result);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(string productIds)
        {
            try
            {

            if (!string.IsNullOrEmpty(productIds))
            {
                var result = await _services.Delete(productIds);
                return Ok(result);
            }
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest("Product not found");

        }

       

    }
}
