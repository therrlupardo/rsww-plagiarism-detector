using System;

namespace IdentityService.Dto
{
    public class LoginResponse
    {
        public LoginResponse(string token)
        {
            AccessToken = token;
            ExpiresAt = (int) TimeSpan.FromMinutes(30).TotalSeconds;
        }

        private string AccessToken { get; }
        private int ExpiresAt { get; }
    }
}