using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace IdentityService.Extensions
{
    internal static class StringExtensions
    {
        internal static string CreateHash(this string text)
        {
            var sha256 = SHA256.Create();
            return HashToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(text)));
        }

        private static string HashToString(IEnumerable<byte> hash)
        {
            return hash.Aggregate(new StringBuilder(), (sb, b) => sb.Append(b.ToString("x2")))
                .ToString();
        }
    }
}