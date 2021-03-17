using System.Collections.Generic;
using IdentityService.Models;

namespace IdentityService.Services
{
    public interface IIdentityService
    {
        bool TryToLogin(string login, string password, out string token);
        User CreateUser(string login, string password);
        List<User> GetAccounts();
    }
}