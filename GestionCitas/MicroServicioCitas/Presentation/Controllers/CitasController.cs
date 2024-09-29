using MicroServicioCitas.Application.DTOs;
using MicroServicioCitas.Application.Interfaces;
using MicroServicioCitas.Application.Sender;
using System.Collections.Generic;
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

        /// <summary>
        /// Constructor que inyecta el servicio de citas y el RabbitMqSender.
        /// </summary>
        /// <param name="citaService">Servicio para manejar las citas.</param>
        /// <param name="rabbitMqSender">Sender de RabbitMQ para la mensajería.</param>
        public CitasController(ICitaService citaService, RabbitMqSender rabbitMqSender)
        {
            _citaService = citaService;
            _rabbitMqSender = rabbitMqSender;
        }

        /// <summary>
        /// Obtiene todas las citas.
        /// </summary>
        /// <returns>Una lista de citas.</returns>
        [HttpGet]
        [Route(Name = "GetCitas")]
        public async Task<IHttpActionResult> GetCitas()
        {
            List<CitaDTO> citas = await _citaService.GetAll();
            return Ok(citas);
        }

        /// <summary>
        /// Obtiene una cita por su ID.
        /// </summary>
        /// <param name="id">ID de la cita a obtener.</param>
        /// <returns>La cita correspondiente al ID proporcionado.</returns>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetCita(int id)
        {
            CitaDTO cita = await _citaService.GetById(id);
            return Ok(cita);
        }

        /// <summary>
        /// Crea una nueva cita.
        /// </summary>
        /// <param name="objCita">Objeto DTO que contiene los datos de la cita a crear.</param>
        /// <returns>La cita creada.</returns>
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> CreateCita([FromBody] CreateCitaDTO objCita)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            CitaDTO createdCita = await _citaService.Create(objCita);
            // Enviar mensaje a RabbitMQ al crear una cita
            await _rabbitMqSender.SendMessageAsync($"Cita creada: ID = {createdCita.Id}, Paciente = {createdCita.PacienteId}");

            return CreatedAtRoute("GetCitas", new { id = createdCita.Id }, createdCita);
        }

        /// <summary>
        /// Actualiza una cita existente.
        /// </summary>
        /// <param name="id">ID de la cita a actualizar.</param>
        /// <param name="objCita">Objeto DTO que contiene los datos actualizados de la cita.</param>
        /// <returns>La cita actualizada.</returns>
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> UpdateCita(int id, [FromBody] UpdateCitaDTO objCita)
        {
            CitaDTO updatedCita = await _citaService.Update(id, objCita);

            // Enviar mensaje a RabbitMQ al actualizar una cita
            await _rabbitMqSender.SendMessageAsync($"UpdateCita Cita actualizada: ID = {updatedCita.Id}");

            return Ok(updatedCita);
        }

        /// <summary>
        /// Elimina una cita existente.
        /// </summary>
        /// <param name="id">ID de la cita a eliminar.</param>
        /// <returns>Estado de la respuesta HTTP.</returns>
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> DeleteCita(int id)
        {
            await _citaService.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Actualiza el estado de una cita existente.
        /// </summary>
        /// <param name="id">ID de la cita a actualizar.</param>
        /// <param name="nuevoEstado">Nuevo estado de la cita.</param>
        /// <returns>La cita con el nuevo estado.</returns>
        [HttpPut]
        [Route("{id:int}/estado")]
        public async Task<IHttpActionResult> UpdateEstado(int id, [FromBody] string nuevoEstado)
        {
            CitaDTO citaUpdated = await _citaService.UpdateEstado(id, nuevoEstado);

            // Enviar mensaje a RabbitMQ al actualizar el estado
            await _rabbitMqSender.SendMessageAsync($"Estado de cita actualizado: ID = {id}, Nuevo Estado = {nuevoEstado}");

            return Ok(citaUpdated);
        }
    }
}
