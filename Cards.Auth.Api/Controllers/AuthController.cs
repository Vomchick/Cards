using Cards.Auth.Api.Interfaces.RepositoryChilds;
using Cards.Auth.Api.Models.ContextModels;
using Cards.Auth.Api.Models.ValidationModels;
using CardsAPI.Auth.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Cards.Auth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IOptions<AuthOptions> authOptions;
        private readonly IUserRepository userRepository;
        private readonly ILogger<AuthController> logger;

        public AuthController(IOptions<AuthOptions> authOptions, IUserRepository userRepository, ILogger<AuthController> logger)
        {
            this.authOptions = authOptions;
            this.userRepository = userRepository;
            this.logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] Login request)
        {
            try
            {
                var user = await userRepository.Authenticate(request.UserName, request.Password);

                if (user != null)
                {
                    var token = GenerateJWT(user);

                    //var options = new CookieOptions
                    //{
                    //    HttpOnly = true,
                    //    Secure = true,
                    //    SameSite = SameSiteMode.None,
                    //    Expires = DateTime.Now.AddMinutes(60)
                    //};

                    //Response.Cookies.Append("token", token, options);

                    return Ok(new { access_token = token });
                    //Generate JWT
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return Unauthorized();
            }
        }

        private string GenerateJWT(User user)
        {
            var authParams = authOptions.Value;

            var securityKey = authParams.GetSymmetricSecurityKey();
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim("role", user.Role.ToString())
            };

            var token = new JwtSecurityToken(authParams.Issuer,
                authParams.Audience,
                claims,
                expires: DateTime.Now.AddSeconds(authParams.TokenLifetime),
                signingCredentials: credential);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
