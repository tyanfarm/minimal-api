using Microsoft.IdentityModel.Tokens;
using SimpleMinimalAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimpleMinimalAPI.Services
{
    public static class JwtService
    {
        public static readonly TimeSpan DEFAULT_EXPIRE_TOKEN = TimeSpan.FromHours(24);

        public static readonly TokenValidationParameters TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSuperSecretKey123!YourSuperSecretKey123")),
        };

        private static readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        public static string GenerateToken(User user)
        {
            var token = _jwtSecurityTokenHandler.CreateToken(DescribeToken(user));
            return _jwtSecurityTokenHandler.WriteToken(token);
        }

        private static SecurityTokenDescriptor DescribeToken(User user)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSuperSecretKey123!YourSuperSecretKey123"));
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString()),
                new Claim(ClaimTypes.Role, user.Role),
            };

            return new SecurityTokenDescriptor
            {
                SigningCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256),
                Expires = DateTime.UtcNow.Add(DEFAULT_EXPIRE_TOKEN),
                Subject = new ClaimsIdentity(claims)
            };
        }
    }
}
