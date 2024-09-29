using MicroServicioPersonas.Application.DTOs;
using MicroServicioRecetas.Application.DTOs;
using MicroServicioRecetas.Application.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System;

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

        /// <summary>
        /// Obtiene todas las recetas registradas.
        /// </summary>
        /// <returns>Retorna una lista de objetos RecetaDTO.</returns>
        [HttpGet]
        [Route(Name = "GetRecetas")]
        public async Task<IHttpActionResult> GetRecetas()
        {
            List<RecetaDTO> recetas = await _recetaService.GetRecetas();
            return Ok(recetas); // Responde con una lista de recetas.
        }

        /// <summary>
        /// Obtiene una receta específica según su ID.
        /// </summary>
        /// <param name="id">ID de la receta a buscar.</param>
        /// <returns>Retorna un objeto RecetaDTO si se encuentra la receta.</returns>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetRecetaById(int id)
        {
            RecetaDTO receta = await _recetaService.GetRecetaById(id);
            return Ok(receta); // Responde con la receta encontrada o un error si no existe.
        }

        /// <summary>
        /// Crea una nueva receta.
        /// </summary>
        /// <param name="recetaDto">DTO con la información para crear la receta.</param>
        /// <returns>Retorna la receta creada con su ID.</returns>
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> CreateReceta([FromBody] CreateRecetaDTO recetaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Valida el modelo.

            RecetaDTO createdReceta = await _recetaService.AddReceta(recetaDto);
            return CreatedAtRoute("GetRecetas", new { id = createdReceta.RecetaId }, createdReceta); // Devuelve la receta creada.
        }

        /// <summary>
        /// Actualiza una receta existente según su ID.
        /// </summary>
        /// <param name="id">ID de la receta a actualizar.</param>
        /// <param name="recetaDto">DTO con la información actualizada de la receta.</param>
        /// <returns>Retorna la receta actualizada.</returns>
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> UpdateReceta(int id, [FromBody] UpdateRecetaDTO recetaDto)
        {
            return Ok(await _recetaService.UpdateReceta(id, recetaDto)); // Actualiza la receta y responde con el resultado.
        }

        /// <summary>
        /// Elimina una receta existente según su ID.
        /// </summary>
        /// <param name="id">ID de la receta a eliminar.</param>
        /// <returns>Retorna un mensaje de éxito si la receta fue eliminada.</returns>
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> DeleteReceta(int id)
        {
            await _recetaService.DeleteReceta(id);
            return Ok(); // Confirma la eliminación de la receta.
        }

        /// <summary>
        /// Obtiene todas las recetas asociadas a un paciente específico.
        /// </summary>
        /// <param name="pacienteId">ID del paciente.</param>
        /// <returns>Retorna una lista de recetas asociadas al paciente.</returns>
        [HttpGet]
        [Route("paciente/{pacienteId:int}")]
        public async Task<IHttpActionResult> GetRecetasByPacienteId(int pacienteId)
        {
            var recetas = await _recetaService.GetRecetasByPacienteId(pacienteId);

            if (recetas == null || recetas.Count == 0)
            {
                return NotFound(); // Devuelve 404 si no se encuentran recetas para el paciente.
            }

            return Ok(recetas); // Devuelve la lista de recetas del paciente.
        }

        /// <summary>
        /// Actualiza el estado de una receta según su ID.
        /// </summary>
        /// <param name="id">ID de la receta.</param>
        /// <param name="nuevoEstado">Nuevo estado de la receta.</param>
        /// <returns>Retorna NoContent si la operación fue exitosa o un mensaje de error si no lo fue.</returns>
        [HttpPut]
        [Route("estado/{id:int}")]
        public async Task<IHttpActionResult> UpdateEstado(int id, [FromBody] string nuevoEstado)
        {
            try
            {
                bool result = await _recetaService.UpdateEstadoReceta(id, nuevoEstado);
                if (result)
                {
                    return StatusCode(HttpStatusCode.NoContent); // Devuelve 204 si la actualización fue exitosa.
                }
                else
                {
                    return BadRequest(); // Devuelve 400 si ocurrió un error.
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Devuelve un mensaje de error en caso de excepción.
            }
        }
    }
}
