using MicroServicioCitas.Application.DTOs;
using MicroServicioCitas.Application.Interfaces;
using MicroServicioCitas.Application.Services;
using MicroServicioCitas.Domain.Models;
using System;
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
            List<CitaDTO> citas = await _citaService.GetAll();
            return Ok(citas);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetCita(int id)
        {
            CitaDTO cita = await _citaService.GetById(id);
            return Ok(cita);
        }

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

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> UpdateCita(int id, [FromBody] UpdateCitaDTO objCita)
        {
            //if (!ModelState.IsValid || objCita.Id != id)
            //if (objCita.Id != id)
            //    return BadRequest(ModelState);

            CitaDTO updatedCita = await _citaService.Update(id, objCita);

            // Enviar mensaje a RabbitMQ al actualizar una cita
            await _rabbitMqSender.SendMessageAsync($"UpdateCita Cita actualizada: ID = {updatedCita.Id}");

            return Ok(updatedCita);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> DeleteCita(int id)
        {
            await _citaService.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPut]
        [Route("{id:int}/estado")]
        public async Task<IHttpActionResult> UpdateEstado(int id, [FromBody] string nuevoEstado)
        {
            CitaDTO citaUpdated = await _citaService.UpdateEstado(id, nuevoEstado);

            // Enviar mensaje a RabbitMQ al actualizar el estado
            await _rabbitMqSender.SendMessageAsync($"Estado de cita actualizado: ID = {id}, Nuevo Estado = {nuevoEstado}");

            //return StatusCode(HttpStatusCode.NoContent);
            return Ok(citaUpdated);
        }

    }
}
