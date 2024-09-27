using MicroServicioPersonas.Application.DTOs;
using MicroServicioRecetas.Application.DTOs;
using MicroServicioRecetas.Application.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace MicroServicioRecetas.Presentation.Controllers
{
    [RoutePrefix("api/recetas")]
    public class RecetaController : ApiController
    {
        private readonly IRecetaService _recetaService;

        public RecetaController(IRecetaService recetaService)
        {
            _recetaService = recetaService;
        }

        [HttpGet]
        //[Route("")]
        [Route(Name = "GetRecetas")]
        public async Task<IHttpActionResult> GetRecetas()
        {
            List<RecetaDTO> recetas = await _recetaService.GetRecetas();
            return Ok(recetas);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetRecetaById(int id)
        {
            RecetaDTO receta = await _recetaService.GetRecetaById(id);
            return Ok(receta);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> CreateReceta([FromBody] CreateRecetaDTO recetaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            RecetaDTO createdReceta = await _recetaService.AddReceta(recetaDto);
            return CreatedAtRoute("GetRecetas", new { id = createdReceta.RecetaId }, createdReceta);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> UpdateReceta(int id, [FromBody] UpdateRecetaDTO recetaDto)
        {
            return Ok(await _recetaService.UpdateReceta(id, recetaDto));
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> DeleteReceta(int id)
        {
            await _recetaService.DeleteReceta(id);
            return Ok();
        }
    }
}
