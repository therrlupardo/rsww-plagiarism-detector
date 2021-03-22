using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using IdentityService.Dto;
using IdentityService.Extensions;
using IdentityService.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using static System.String;

namespace IdentityService.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserContext _context;
        private readonly IOptions<Audience> _settings;

        public IdentityService(IOptions<Audience> settings, UserContext context)
        {
            _settings = settings;
            _context = context;
        }

        public bool TryToLogin(LoginRequest loginRequest, out string token)
        {
            var passwordHash = loginRequest.Password.CreateHash();
            var user = _context.Users.FirstOrDefault(u => u.Login.Equals(loginRequest.Login) && u.PasswordHash.Equals(passwordHash));
            if (user == null)
            {
                token = Empty;
                return false;
            }

            token = GenerateTokenFor(user);
            return true;
        }

        public User CreateUser(string login, string password)
        {
            if (_context.Users.FirstOrDefault(u => u.Login.Equals(login)) != null)
                throw new DuplicateNameException("User already exists");

            var user = new User(login, password);
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public List<User> GetAccounts()
        {
            return _context.Users.ToList();
        }

        private string GenerateTokenFor(User user)
        {
            var creationTime = DateTime.Now;
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_settings.Value.Secret));

            var jwt = new JwtSecurityToken(
                _settings.Value.Iss,
                _settings.Value.Aud,
                CreateClaims(user, creationTime),
                creationTime,
                creationTime.Add(TimeSpan.FromMinutes(30)),
                new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private static IEnumerable<Claim> CreateClaims(User user, DateTime time)
        {
            return new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, time.ToUniversalTime().ToString(CultureInfo.InvariantCulture),
                    ClaimValueTypes.Integer64),
                new Claim("login", user.Login)
            };
        }
    }
}