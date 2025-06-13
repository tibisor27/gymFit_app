using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using GymFit.BE.Models;
using log4net;

namespace GymFit.BE.Services
{
    public class JwtService
    {
        private readonly IConfiguration _config;
        private readonly ILog _logger;
        public JwtService(IConfiguration config)
        {
            _config = config;
            _logger = LogManager.GetLogger(typeof(JwtService));
        }

        public string GenerateToken(User user)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = jwtSettings["Key"] ?? throw new Exception("JWT Key missing!");
            var audience = jwtSettings["Audience"] ?? throw new Exception("JWT Audience missing!");
            var issuer = jwtSettings["Issuer"] ?? throw new Exception("JWT Issuer missing!");
            int expireMinutes = int.Parse(jwtSettings["ExpireMinutes"] ?? "60");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.UserRole.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: creds
            );

            _logger.Info($"Generated JWT token for user {user.Email} (id: {user.Id})");
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}