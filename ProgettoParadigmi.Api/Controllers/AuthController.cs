using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProgettoParadigmi.Api.Business;
using ProgettoParadigmi.Api.Utils;
using ProgettoParadigmi.Models.Context;
using ProgettoParadigmi.Models.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProgettoParadigmi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(AppuntamentiDbContext context, IConfiguration configuration)
        : ControllerBase
    {
        private IConfiguration _configuration = configuration;
        private AuthManager _manager = new(context);


        [HttpPost, Route("Login")]
        public IActionResult Login([FromBody] LoginDto body)
        {
            try
            {
                var result = _manager.LoginUser(body);
                if (result.Result != null)
                {
                    result.Result.Token = CreateToken(result.Result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost, Route("Register")]
        public IActionResult Register([FromBody] RegisterDto body)
        {
            try
            {
                var result = _manager.RegisterUser(body);
                if (result.Result != null)
                {
                    // crea il token
                    result.Result.Token = CreateToken(result.Result);
                }

                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        private string CreateToken(AuthDto user)
        {
            var issuedAt = DateTime.UtcNow;
            var expires = DateTime.Now.AddHours(4);

            var tokenHandler = new JwtSecurityTokenHandler();
            var cIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Nome),
                new Claim(ClaimTypes.Actor, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, ((int)user.TipoUtente).ToString())
            });
            // recupero da web.config issuer e secret
            var issuer = _configuration["JWT:Issuer"];
            var secret = _configuration["JWT:Secret"];

            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(secret));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = (JwtSecurityToken)tokenHandler.CreateJwtSecurityToken(issuer: issuer,
                audience: issuer,
                subject: cIdentity,
                notBefore: issuedAt,
                expires: expires,
                signingCredentials: signingCredentials
            );
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}
