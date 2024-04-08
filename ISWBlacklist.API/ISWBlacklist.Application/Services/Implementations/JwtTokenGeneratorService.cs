using ISWBlacklist.Application.Services.Interfaces;
using ISWBlacklist.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ISWBlacklist.Application.Services.Implementations
{
    public class JwtTokenGeneratorService : IJwtTokenGeneratorService
    {
        private readonly IConfiguration _config;

        public JwtTokenGeneratorService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(AppUser appUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_config.GetSection("JWT:Key").Value);

            List<Claim> claimList = new()
            {
                new Claim(ClaimTypes.Email, appUser.Email),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _config.GetSection("JWT:Audience").Value,
                Issuer = _config.GetSection("JWT:Issuer").Value,
                Subject = new ClaimsIdentity(claimList),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}
