using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.Services.Repo
{
    public class TokenGenerator
    {
        public string GenerateToken(string secretKey, string issuer, string audience, int expirationDays,  IEnumerable<Claim> claims = null)
        {
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer,
               audience,
               claims,
               DateTime.UtcNow,
               DateTime.UtcNow.AddDays(expirationDays),
               credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
