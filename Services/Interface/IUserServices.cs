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
        Task<bool> AddUserAsync(string email, string fullName, string userName, string password);
        Task<bool> ConfirmUserAsync(string email, string code);
        Task<User> GetUserByEmail(string email);
        Task<User> AuthenticateAsync(string email, string password);
    }
}
