using BussinessObject.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DataAccess.Security
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }
        public string GenerateJwtToken(Account user, string role)
        {

            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKey = _config["JwtSettings:SecretKey"];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            var issuer = _config["JwtSettings:Issuer"];



            var tokenDescription = new SecurityTokenDescriptor
            {
                Issuer = issuer,
                Audience = issuer,
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name,user.FullName),

                    new Claim("IdCardNumber", user.IdentityCardNumber.ToString()),
                    new Claim("Id", user.Id.ToString()),


                    new Claim("TokenId", Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, role )

                }),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);

            return jwtTokenHandler.WriteToken(token);
        }

    }
}
