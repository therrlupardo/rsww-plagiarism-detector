using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

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
            PasswordHash = CreatePasswordHash(password);
            Id = Guid.NewGuid();
        }

        public string Login { get; set; }
        public string PasswordHash { get; set; }

        [Key] public Guid Id { get; set; }

        public static string CreatePasswordHash(string password)
        {
            var sha256 = SHA256.Create();
            return HashToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        private static string HashToString(IEnumerable<byte> hash)
        {
            var builder = new StringBuilder();
            foreach (var t in hash) builder.Append(t.ToString("x2"));
            return builder.ToString();
        }
    }
}