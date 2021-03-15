using System;
using System.ComponentModel.DataAnnotations;
using IdentityService.Utils;

namespace IdentityService.Models
{
    public class User
    {
        public User()
        {
        }

        public User(string login, string password)
        {
            Login = login;
            PasswordHash = PasswordUtil.CreatePasswordHash(password);
            Id = Guid.NewGuid();
        }

        public string Login { get; }
        public string PasswordHash { get; }

        [Key] public Guid Id { get; }

        
    }
}