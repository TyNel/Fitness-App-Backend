using Fitness.Models.Domain;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManagementApp.Core.Entities;

namespace Fitness.Services.Repo
{
    public class AccessTokenGenerator
    {
            private readonly AuthenticationConfiguration _configuration;
        private readonly TokenGenerator _tokenGenerator;

        public AccessTokenGenerator(AuthenticationConfiguration configuration, TokenGenerator token)
        {
            _configuration = configuration;
            _tokenGenerator = token;

        }
        public string GenerateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim("id", user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email.ToString()),
            };

            return _tokenGenerator.GenerateToken(
                _configuration.TokenSecret,
                _configuration.Issuer,
                _configuration.Audience,
                _configuration.TokenExpirationDays,
                claims);
  
        }
    }
}
