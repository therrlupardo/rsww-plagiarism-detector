using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace IdentityService.Utils
{
    public static class PasswordUtil
    {
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