using System.Collections.Generic;
using IdentityService.Models;

namespace IdentityService.Services
{
    public interface IIdentityService
    {
        string Login(string login, string password);
        User CreateUser(string login, string password);
        List<User> GetAll();
    }
}