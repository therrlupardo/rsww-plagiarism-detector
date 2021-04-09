using System.Collections.Generic;
using IdentityService.Dto;
using IdentityService.Models;

namespace IdentityService.Services
{
    public interface IIdentityService
    {
        bool TryToLogin(LoginRequest loginRequest, out string token);
        User CreateUser(string login, string password);
        List<User> GetAccounts();
    }
}