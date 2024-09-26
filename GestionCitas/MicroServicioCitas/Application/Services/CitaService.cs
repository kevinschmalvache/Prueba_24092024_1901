using MicroServicioCitas.Application.Interfaces;
using MicroServicioCitas.Domain.Interfaces;
using MicroServicioCitas.Domain.Models;
using MicroServicioCitas.Domain.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServicioCitas.Application.Services
{
    public class CitaService : ICitaService
    {
        private readonly ICitaRepository _citaRepository;
        private readonly CitaDomainService _citaDomainService;
        private readonly IRabbitMqService _rabbitMqService; // Servicio para RabbitMQ

        public CitaService(ICitaRepository citaRepository, CitaDomainService citaDomainService, IRabbitMqService rabbitMqService)
        {
            _citaRepository = citaRepository;
            _citaDomainService = citaDomainService;
            _rabbitMqService = rabbitMqService;
        }

        public async Task<List<Cita>> GetAll()
        {
            return await _citaRepository.GetAll();
        }

        public async Task<Cita> GetById(int id)
        {
            var cita = await _citaRepository.GetById(id);
            _citaDomainService.ExistCita(cita); // Valida si existe la cita
            return cita;
        }

        public async Task<Cita> Create(Cita cita)
        {
            _citaDomainService.ValidateCita(cita);
            cita.Estado = "Pendiente"; // Estado inicial
            return await _citaRepository.Add(cita);
        }

        public async Task<Cita> Update(int id, Cita cita)
        {
            var existCita = await _citaRepository.GetById(id);
            _citaDomainService.ExistCita(existCita);
            _citaDomainService.ValidateCita(cita); // Validar la cita actualizada

            cita.Id = id; // Asigna el ID a la cita actualizada
            return await _citaRepository.Update(cita);
        }

        public async Task<Cita> UpdateEstado(int id, string nuevoEstado)
        {
            Cita cita = await _citaRepository.GetById(id);
            _citaDomainService.ExistCita(cita);

            // Validaciones del nuevo estado
            if (nuevoEstado != "Pendiente" && nuevoEstado != "En proceso" && nuevoEstado != "Finalizada")
            {
                throw new ArgumentException("Estado no válido.");
            }

            // Actualiza el estado de la cita
            cita.Estado = nuevoEstado;
            Cita objResult = await _citaRepository.UpdateEstado(id, nuevoEstado); // Llama al método Update del repositorio

            // Si el estado es "Finalizada", enviar un mensaje a RabbitMQ
            if (nuevoEstado == "Finalizada")
            {
                var recetaData = new
                {
                    CitaId = id,
                    PacienteId = objResult.PacienteId,
                    MedicoId = objResult.MedicoId
                };

                // Enviar el mensaje a RabbitMQ para que el microservicio de Recetas procese la creación de una nueva receta
                await _rabbitMqService.SendRecetaRequest(recetaData);
            }

            return cita; // Retorna la cita actualizada
        }

        public async Task Delete(int id)
        {
            Cita existCita = await _citaRepository.GetById(id);
            _citaDomainService.ExistCita(existCita);
            await _citaRepository.Delete(id);
        }

        public async Task FinalizarCita(int id)
        {
            Cita cita = await GetById(id);
            if (cita.Estado != "Pendiente")
                throw new Exception("Solo se pueden finalizar citas que estén en estado 'Pendiente'.");

            cita.Estado = "Finalizada"; // Cambia el estado a "Finalizada"
            await _citaRepository.Update(cita);
            await _rabbitMqService.SendRecetaRequest(cita.Id); // Envía la receta al finalizar la cita
        }
    }
}
