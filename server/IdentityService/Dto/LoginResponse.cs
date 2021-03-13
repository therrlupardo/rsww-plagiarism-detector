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

        public string AccessToken { get; set; }
        public int ExpiresAt { get; set; }
    }
}