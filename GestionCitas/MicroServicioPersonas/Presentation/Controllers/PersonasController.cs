using MicroServicioPersonas.Aplication.Interfaces;
using MicroServicioPersonas.Application.DTOs;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace MicroServicioPersonas.Presentation.Controllers
{
    [RoutePrefix("api/personas")]
    public class PersonasController : ApiController
    {
        private readonly IPersonaService _personaService;

        // Constructor que inyecta el servicio usando Unity
        public PersonasController(IPersonaService personaService)
        {
            _personaService = personaService;
        }

        /// <summary>
        /// Obtiene una lista de todas las personas registradas.
        /// </summary>
        /// <returns>Retorna una lista de objetos PersonaDTO.</returns>
        [HttpGet]
        [Route(Name = "GetPersona")]
        public async Task<IHttpActionResult> GetPersonas()
        {
            return Ok(await _personaService.GetAll()); // Retorna todas las personas.
        }

        /// <summary>
        /// Obtiene una persona específica por su ID.
        /// </summary>
        /// <param name="id">ID de la persona a buscar.</param>
        /// <returns>Retorna un objeto PersonaDTO si la persona es encontrada.</returns>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetPersona(int id)
        {
            return Ok(await _personaService.GetById(id)); // Retorna la persona o un error si no se encuentra.
        }

        /// <summary>
        /// Crea una nueva persona.
        /// </summary>
        /// <param name="objPersona">DTO con la información para crear una nueva persona.</param>
        /// <returns>Retorna la persona creada con su ID.</returns>
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> CreatePersona([FromBody] CreatePersonaDTO objPersona)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Valida el modelo.

            PersonaDTO createdPersona = await _personaService.Create(objPersona);
            return CreatedAtRoute("GetPersona", new { id = createdPersona.Id }, createdPersona); // Devuelve la persona creada.
        }

        /// <summary>
        /// Actualiza una persona existente.
        /// </summary>
        /// <param name="id">ID de la persona a actualizar.</param>
        /// <param name="objPersona">DTO con la información actualizada de la persona.</param>
        /// <returns>Retorna la persona actualizada.</returns>
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> UpdatePersona(int id, [FromBody] UpdatePersonaDTO objPersona)
        {
            return Ok(await _personaService.Update(id, objPersona)); // Actualiza la persona y responde con el resultado.
        }

        /// <summary>
        /// Elimina una persona existente por su ID.
        /// </summary>
        /// <param name="id">ID de la persona a eliminar.</param>
        /// <returns>Retorna un código 204 si la operación fue exitosa.</returns>
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> DeletePersona(int id)
        {
            await _personaService.Delete(id);
            return StatusCode(HttpStatusCode.NoContent); // Responde con un código de éxito sin contenido.
        }

        /// <summary>
        /// Valida si una persona es válida según su ID y tipo de persona.
        /// </summary>
        /// <param name="id">ID de la persona a validar.</param>
        /// <param name="tipoPersona">Tipo de persona a validar (ej. "Paciente", "Médico").</param>
        /// <returns>Retorna verdadero si la persona es válida, de lo contrario retorna un 404.</returns>
        [HttpGet]
        [Route("validate/{id:int}/{tipoPersona}")]
        public async Task<IHttpActionResult> ValidatePersona(int id, string tipoPersona)
        {
            bool isValid = await _personaService.ValidatePersonaAsync(id, tipoPersona);
            if (isValid)
                return Ok(true); // Devuelve true si la persona es válida.

            return NotFound(); // Devuelve 404 si no se encuentra o no es válida.
        }
    }
}
