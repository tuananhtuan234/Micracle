using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Data.DTOs;
using Services.Helpers;
using Services.Interface;

namespace Micracle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserControllers : ControllerBase
    {
        private readonly IUserServices _userService;
        private readonly JwtTokenHelper _jwtTokenHelper;
        private readonly IConfiguration _configuration;

        public UserControllers(IUserServices userService, JwtTokenHelper jwtTokenHelper, IConfiguration configuration)
        {
            _userService = userService;
            _jwtTokenHelper = jwtTokenHelper;
            _configuration = configuration;
        }

        #region Register User
        [HttpPost("User register")]
        public async Task<IActionResult> AddUser([FromBody] RegisterDTO userDto)
        {
            try
            {
                var result = await _userService.AddUserAsync(userDto.Email, userDto.FullName, userDto.UserName, userDto.Password);
                if (!result)
                {
                    return BadRequest("Email already exists.");
                }

                return Ok("User created successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Trả về thông báo lỗi định dạng email
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred."); // Trả về lỗi chung
            }
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> Confirm(string email, string code)
        {
            var result = await _userService.ConfirmUserAsync(email, code);
            if (result)
            {
                var user = await _userService.GetUserByEmail(email);
                return Ok(new { message = "User confirmed and registered successfully." });
            }
            return BadRequest(new { message = "Invalid verification code or email." });
        }
        #endregion

        #region
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var user = await _userService.AuthenticateAsync(loginDto.Email, loginDto.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            // Tạo token JWT và trả về cho người dùng
            var token = _jwtTokenHelper.GenerateJwtToken(user);
            return Ok(new { token });
        }
        #endregion
    }
}
