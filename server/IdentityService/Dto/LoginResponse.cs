using System;

namespace IdentityService.Dto
{
    public record LoginResponse(string AccessToken)
    {
        public int ExpiresAt => (int) TimeSpan.FromMinutes(30).TotalSeconds;
    }
}