using Maanfee.Dashboard.Domain.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LoggingPlatform.Extensions
{
	public class JwtHelpers
    {
        public JwtHelpers(IConfiguration configuration)
        {
            Configuration = configuration;
            JWtSettings = Configuration.GetSection("JwtSettings");
        }

        private readonly IConfiguration Configuration;
        private readonly IConfigurationSection JWtSettings;

        public SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(JWtSettings.GetSection("securityKey").Value);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        public List<Claim> GetClaims(JwtLoginViewModel Model)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Model.UserName)
            };

            return claims;
        }

        public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: JWtSettings.GetSection("validIssuer").Value,
                audience: JWtSettings.GetSection("validAudience").Value,
                claims: claims,
                expires: DateTime.Now.AddHours(Convert.ToDouble(JWtSettings.GetSection("ExpiryInHour").Value)),
                signingCredentials: signingCredentials);

            return tokenOptions;
        }


    }
}
