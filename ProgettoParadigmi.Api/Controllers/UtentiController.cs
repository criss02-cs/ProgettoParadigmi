using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProgettoParadigmi.Api.Business;
using ProgettoParadigmi.EmailSender;
using ProgettoParadigmi.Models.Context;
using ProgettoParadigmi.Models.Dto;
using ProgettoParadigmi.Models.Entities;

namespace ProgettoParadigmi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class UtentiController(AppuntamentiDbContext ctx) : ControllerBase
    {
        private UtentiManager _man = new(ctx);
        private AuthManager _authManager = new(ctx);

        [HttpGet, Route("GetAll/{take?}/{skip?}")]
        public IActionResult GetAll(int take = 10, int skip = 0)
        {
            try
            {
                var result = _man.GetAll(take, skip);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost, Route("Insert")]
        public IActionResult Insert([FromBody] RegisterDto dto)
        {
            try
            {
                var result = _authManager.RegisterUser(dto);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost, Route("ChangePassword")]
        public IActionResult ChangePassword([FromBody] ChangePasswordDto dto)
        {
            try
            {
                var isAbilitato = IsUserAbilitated(Request.Headers, dto.Id);
                if (!isAbilitato.Result)
                    return Ok(isAbilitato);
                var result = _man.ChangePassword(dto);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        private Response<bool> IsUserAbilitated(IHeaderDictionary requestHeaders, Guid userId)
        {
            if (requestHeaders.TryGetValue("Authorization", out var headerAuth))
            {
                var jwtToken = headerAuth.First().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(jwtToken);
                var tipoUtenteString = jwt.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value ?? "";
                var idRequest = jwt.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Actor)?.Value ?? "";
                if (!string.IsNullOrEmpty(tipoUtenteString) && !string.IsNullOrEmpty(idRequest))
                {
                    var tipoUtente = (TipoUtente)Convert.ToInt32(tipoUtenteString);
                    var id = Guid.Parse(idRequest);
                    if (id != userId && tipoUtente != TipoUtente.Admin)
                    {
                        var res = ResponseFactory.CreateResponseFromResult(false, false,
                            "Non puoi cambiare la password di un altro utente");
                        return res;
                    }
                }
            }
            return ResponseFactory.CreateResponseFromResult(true);
        }

        [HttpGet, Route("{userId:Guid}")]
        public IActionResult GetUserById(Guid userId)
        {
            try
            {
                var result = _man.GetUtenteById(userId);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
