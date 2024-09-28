using AutoMapper;
using MicroServicioPersonas.Aplication.Interfaces;
using MicroServicioPersonas.Application.DTOs;
using MicroServicioPersonas.Domain.Models;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace MicroServicioPersonas.Presentation.Controllers
{
    [RoutePrefix("api/personas")]
    public class PersonasController : ApiController
    {

        private readonly IPersonaService _personaService;
        private readonly IMapper _mapper;

        // Constructor que inyecta el servicio usando unity
        public PersonasController(IPersonaService personaService, IMapper mapper)
        {
            _personaService = personaService;
            _mapper = mapper;
        }

        [HttpGet]
        //[Route("")]
        [Route(Name = "GetPersona")]
        public async Task<IHttpActionResult> GetPersonas()
        {
            return Ok(await _personaService.GetAll());
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetPersona(int id)
        {
            return Ok(await _personaService.GetById(id));
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> CreatePersona([FromBody] CreatePersonaDTO objPersona)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            PersonaDTO createdPersona = await _personaService.Create(objPersona);
            return CreatedAtRoute("GetPersona", new { id = createdPersona.Id }, createdPersona);
        }


        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> UpdatePersona(int id, [FromBody] UpdatePersonaDTO objPersona)
        {
            return Ok(await _personaService.Update(id, objPersona));
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> DeletePersona(int id)
        {
            await _personaService.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}