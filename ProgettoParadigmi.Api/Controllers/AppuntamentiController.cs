using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProgettoParadigmi.Api.Business;
using ProgettoParadigmi.EmailSender;
using ProgettoParadigmi.Models.Context;
using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class AppuntamentiController(AppuntamentiDbContext context, EmailService mailService) : ControllerBase
    {
        private AppuntamentiManager _man = new(context, mailService);


        [HttpPost, Route("Insert")]
        public async Task<IActionResult> Insert([FromBody] AppuntamentoDto dto)
        {
            try
            {
                var result = await _man.InsertAppuntamento(dto);
                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e);
            }
        }

        [HttpGet, Route("{userId:Guid}/{mese:int?}/{anno:int?}")]
        public IActionResult GetAppuntamentiByUserId(Guid userId, int mese = 0, int anno = 0)
        {
            try
            {
                var result = _man.GetAppuntamentiByUserId(userId, mese, anno);
                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost, Route("Update/{appuntamentoId:Guid}")]
        public IActionResult Update([FromBody] AppuntamentoDto dto, Guid appuntamentoId)
        {
            try
            {
                var result = _man.UpdateAppuntamento(dto, appuntamentoId);
                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet, Route("AppuntamentiDaAccettare/{userId:Guid}")]
        public IActionResult GetAppuntamentiDaAccettare(Guid userId)
        {
            try
            {
                var result = _man.GetAppuntamentiDaAccettare(userId);
                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost, Route("AggiornaStatoInvito")]
        public IActionResult AggiornaStatoInvito([FromBody] AggiornaStatoInvitoDto dto)
        {
            try
            {
                var result = _man.AggiornaStatoInvito(dto.PartecipazioneId, dto.Stato);
                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
    }
}
