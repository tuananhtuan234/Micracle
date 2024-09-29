using Repositories.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IUserServices
    {
        Task<User> AuthenticateUserAsync(string username, string password);
        Task RegisterUserAsync(User user);
        Task<User> GetUserByIdAsync(string id);
        Task<User> GetUserByUsernameAsync(string username);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(string id);
        Task<List<User>> GetAllUsersAsync();
    }
}
