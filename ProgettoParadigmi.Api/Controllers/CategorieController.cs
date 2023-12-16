using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProgettoParadigmi.Api.Business;
using ProgettoParadigmi.Models.Context;
using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class CategorieController(AppuntamentiDbContext context) : ControllerBase
    {
        private CategorieManager _man = new(context);
        [HttpPost, Route("Insert")]
        public IActionResult Insert([FromBody] CategoriaDto dto)
        {
            try
            {
                var result = _man.AddCategoria(dto);
                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                var result = ResponseFactory.CreateResponseFromResult(false, false, e.Message);
                return StatusCode(500, result);
            }
        }

        [HttpGet, Route("{userId:Guid}")]
        public IActionResult GetCategorieByUserId(Guid userId)
        {
            try
            {
                var result = _man.GetCategorieByUserId(userId);
                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                var result = 
                    ResponseFactory.CreateResponseFromResult<List<CategoriaDto>>([], false, e.Message);
                return StatusCode(500, result);
            }
        }
    }
}
