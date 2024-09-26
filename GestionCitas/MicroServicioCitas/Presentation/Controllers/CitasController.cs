using MicroServicioCitas.Application.Interfaces;
using MicroServicioCitas.Application.Services;
using MicroServicioCitas.Domain.Models;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace MicroServicioCitas.Presentation.Controllers
{
    [RoutePrefix("api/citas")]
    public class CitasController : ApiController
    {
        private readonly ICitaService _citaService;
        private readonly RabbitMqSender _rabbitMqSender;

        // Constructor que inyecta el servicio y RabbitMqSender usando Unity
        public CitasController(ICitaService citaService, RabbitMqSender rabbitMqSender)
        {
            _citaService = citaService;
            _rabbitMqSender = rabbitMqSender;
        }

        [HttpGet]
        //[Route("")]
        [Route(Name = "GetCitas")]
        public async Task<IHttpActionResult> GetCitas()
        {
            var citas = await _citaService.GetAll();
            return Ok(citas);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetCita(int id)
        {
            var cita = await _citaService.GetById(id);
            if (cita == null)
            {
                return NotFound();
            }
            return Ok(cita);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> CreateCita([FromBody] Cita objCita)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdCita = await _citaService.Create(objCita);
            // Enviar mensaje a RabbitMQ al crear una cita
            _rabbitMqSender.SendMessage($"Cita creada: ID = {createdCita.Id}, Paciente = {createdCita.PacienteId}");

            return CreatedAtRoute("GetCitas", new { id = createdCita.Id }, createdCita);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> UpdateCita(int id, [FromBody] Cita objCita)
        {
            if (!ModelState.IsValid || objCita.Id != id)
            {
                return BadRequest(ModelState);
            }

            var updatedCita = await _citaService.Update(id, objCita);
            if (updatedCita == null)
            {
                return NotFound();
            }

            // Enviar mensaje a RabbitMQ al actualizar una cita
            _rabbitMqSender.SendMessage($"Cita actualizada: ID = {updatedCita.Id}");

            return Ok(updatedCita);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> DeleteCita(int id)
        {
            var existingCita = await _citaService.GetById(id);
            if (existingCita == null)
            {
                return NotFound();
            }

            await _citaService.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPut]
        [Route("{id:int}/estado")]
        public async Task<IHttpActionResult> UpdateEstado(int id, [FromBody] string nuevoEstado)
        {
            if (string.IsNullOrWhiteSpace(nuevoEstado))
            {
                return BadRequest("El estado no puede estar vacío.");
            }

            var existeCita = await _citaService.GetById(id);
            if (existeCita == null)
            {
                return NotFound();
            }

            await _citaService.UpdateEstado(id, nuevoEstado);
            // Enviar mensaje a RabbitMQ al actualizar el estado
            _rabbitMqSender.SendMessage($"Estado de cita actualizado: ID = {id}, Nuevo Estado = {nuevoEstado}");

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
