using Domain.Constants;
using Domain.Models;
using Domain.Repository.Entities;
using Domain.Security;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Security
{
    public class TokenJwt : ITokenJwt
    {
        private readonly SecuritySettings _securitySettings;
        public TokenJwt(IOptions<SecuritySettings> securitySettings) {
            _securitySettings = securitySettings.Value;
        }
        
        public string GenerateToken(User user, int expiresMinutes) {
            var key = Encoding.ASCII.GetBytes(_securitySettings.Secret);
            var tokenHandler = new JwtSecurityTokenHandler();
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(TokenItems.Id, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Names),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(expiresMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(descriptor);
            return tokenHandler.WriteToken(token);
        }

        public int ValidateToken(string token) {
            if (token == null)
                return 0;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_securitySettings.Secret);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = _securitySettings.ValidateIssuer,
                    ValidateAudience = _securitySettings.ValidateAudience,
                    ValidIssuer = _securitySettings.ValidIssuer,
                    ValidAudience = _securitySettings.ValidAudience,
                    // set clockskew to zero so tokens expire exactly at token expiration time
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken) ;

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == TokenItems.Id).Value);
                return userId;
            } catch
            {
                return 0;
            }
        }
    }
}
