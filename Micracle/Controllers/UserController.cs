using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Repositories.Data.DTOs;
using Repositories.Data.Entity;
using Services;
using Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Repositories.Enums;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Micracle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userService;
        private readonly IConfiguration _configuration;
        public UserController(IUserServices userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var user = await _userService.AuthenticateUserAsync(model.UserName, model.Password);
            if (user == null)
            {
                return Unauthorized(); // Invalid credentials
            }

            // Generate JWT Token
            var token = GenerateJwtToken(user);

            return Ok(new { token });
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Role, ((UserRole)user.Role).ToString()) // Add roles or other claims
        }),
                Expires = DateTime.UtcNow.AddMinutes(60),
                Issuer = _configuration["Jwt:Issuer"], 
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            var user = new User
            {
                UserName = model.UserName,
                Password = model.Password, // Password should be hashed in UserService
                FullName = model.FullName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Province = model.Province,
                District = model.District,
                Address = model.Address,
                UpdatedDate = DateTime.Now,
                CreatedDate = DateTime.Now,
                Status = 1, 
                Role = 1 // 1 trong enum là Member
            };

            try
            {
                await _userService.RegisterUserAsync(user);
                return Ok("User registered successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Handle exception
            }
        }
        
        [HttpGet]
        [Authorize(Roles= "Admin,Manager")] 
        public async Task<IActionResult> GetUserByUsername(string userName)
        {
            var users = await _userService.GetUserByUsernameAsync(userName);
            return Ok(users);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")] 
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDTO model)
        {
            // Lấy thông tin người dùng từ DB
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found."); 
            }

            // mapper chay
            user.FullName = model.FullName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.Province = model.Province;
            user.District = model.District;
            user.Address = model.Address;
            user.UpdatedDate = DateTime.Now;

            try
            {
                await _userService.UpdateUserAsync(user);
                return Ok("User updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }


        [HttpGet("all")]
        [Authorize(Roles = "Admin, Manager")] 
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync(); 
            return Ok(users);
        }

        [HttpPatch("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO updateUserDto)
        {
            // Lấy ID của người dùng hiện tại từ claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized(); // Nếu người dùng không khớp, trả về Unauthorized
            }

            // Lấy thông tin người dùng hiện tại từ cơ sở dữ liệu
            var existingUser = await _userService.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                return NotFound("User not found."); // Nếu không tìm thấy người dùng
            }
            // Chỉ cập nhật các trường cho phép
            existingUser.FullName = updateUserDto.FullName ?? existingUser.FullName;
            existingUser.Email = updateUserDto.Email ?? existingUser.Email;
            existingUser.PhoneNumber = updateUserDto.PhoneNumber ?? existingUser.PhoneNumber;
            existingUser.Province = updateUserDto.Province ?? existingUser.Province;
            existingUser.District = updateUserDto.District ?? existingUser.District;
            existingUser.Address = updateUserDto.Address ?? existingUser.Address;
            try
            {
                await _userService.UpdateUserAsync(existingUser);
                return Ok("User updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
