using LoggingPlatform.Extensions;
using Maanfee.Dashboard.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace LoggingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        public AccountsController(JwtHelpers _JwtHelpers)
        {
            JwtHelpers = _JwtHelpers;
        }

        private readonly JwtHelpers JwtHelpers;

        [HttpPost("Login")]
        public IActionResult Login([FromBody] JwtLoginViewModel Model)
        {
            if(string .IsNullOrEmpty(Model.UserName) || string.IsNullOrEmpty(Model.Password))
            {
                return Unauthorized(new JwtAuthenticationViewModel 
                { 
                    ErrorMessage = "Invalid Authentication" 
                });
            }

            if (Model.UserName != "Maanfee" && Model.Password != "Maanfee")
            {
                return Unauthorized(new JwtAuthenticationViewModel
                {
                    ErrorMessage = "Invalid Authentication"
                });
            }

            var signingCredentials = JwtHelpers.GetSigningCredentials();
            var claims = JwtHelpers.GetClaims(Model);
            var tokenOptions = JwtHelpers.GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return Ok(new JwtAuthenticationViewModel
            {
                IsAuthSuccessful = true,
                Token = token
            });
        }

		[HttpGet("GetStatus")]
		public string GetStatus()
		{
			return "Authorized";
		}


	}
}