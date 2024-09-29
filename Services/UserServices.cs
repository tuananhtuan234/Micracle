using MailKit;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Repositories.Data.Entity;
using Repositories.Interface;
using Services.Helpers;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _cache;
        private readonly IEmailServices _emailServices;
        private readonly VerificationCodeManager _verificationCodeManager;
        private readonly ILogger<UserServices> _logger;


        public UserServices(IUserRepository userRepository, IMemoryCache cache, IEmailServices emailServices, 
            VerificationCodeManager verificationCodeManager, ILogger<UserServices> logger)
        {
            _userRepository = userRepository;
            _cache = cache;
            _emailServices = emailServices;
            _verificationCodeManager = verificationCodeManager;
            _logger = logger;
        }

        #region Register User
        public async Task<bool> AddUserAsync(string email, string fullName, string userName, string password)
        {
            var isValidEmail = await IsValidEmail(email);
            if (!isValidEmail)
            {
                throw new ArgumentException("Wrong email format"); // Email không hợp lệ
            }

            var existingUser = await _userRepository.GetUserByEmailAsync(email);
            if (existingUser != null)
            {
                return false; // Email đã tồn tại
            }


            var verificationCode = GenerateVerificationCode();
            //_logger.LogInformation($"Confirm code is : {verificationCode}");
            var expiration = TimeSpan.FromMinutes(10);
            //_logger.LogInformation($"Time expiration : {expiration}");

            await _verificationCodeManager.SetVerificationCodeAsync(email, verificationCode, expiration);

            await _emailServices.SendEmail_ConfirmCode(email, fullName, verificationCode);


            var user = new User
            {
                Email = email,
                FullName = fullName,
                UserName = userName,
                Password = password,
                Role = 1,
                Status = 1,
                CreatedDate = DateTime.Now
            };

            _cache.Set(verificationCode, user);
            //_logger.LogInformation($"User INFOR : {user}");
            //_logger.LogInformation($"Temporary users count in register: {_cache.Get<int>("UserCount")}");
            return true;
        }

        public async Task<bool> ConfirmUserAsync(string email, string code)
        {
            //_logger.LogInformation($"ConfirmUserAsync called with email: {email} and code: {code}");

            if (await _verificationCodeManager.ValidateVerificationCodeAsync(email, code))
            {
                //_logger.LogInformation($"Verification code for {email} is valid.");
                //_logger.LogInformation($"Temporary users count in confirm: {_cache.Get<int>("UserCount")}");

                if (_cache.TryGetValue(code, out User user))
                {
                    //_logger.LogInformation($"User found in temporary storage: {user}");

                    await _userRepository.AddUserAsync(user);
                    await _verificationCodeManager.RemoveVerificationCodeAsync(email);
                    _cache.Remove(email);
                    return true;
                }
                else
                {
                    _logger.LogWarning($"User not found in temporary storage for email: {email}");
                }
            }
            else
            {
                _logger.LogWarning($"Invalid verification code for email: {email}");
            }
            return false;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }
        #endregion

        #region Login
        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null || user.Password != password)
            {
                return null;
            }

            return user;
        }
        #endregion

        #region Sub code

        private string GenerateVerificationCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private async Task<bool> IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    var idn = new IdnMapping();

                    // Use IdnMapping class to convert Unicode domain names.
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private async Task<(string firstName, string lastName)> SplitFirstAndLastName(string fullName)
        {
            // Tách chuỗi thành các từ dựa trên khoảng trắng và loại bỏ các mục trống
            string[] words = fullName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Lấy từ đầu tiên và từ cuối cùng
            string firstName = words[0];
            string lastName = words[^1];
            Console.WriteLine(firstName + " " + lastName);

            return (firstName, lastName);
        }
        #endregion
    }
}
