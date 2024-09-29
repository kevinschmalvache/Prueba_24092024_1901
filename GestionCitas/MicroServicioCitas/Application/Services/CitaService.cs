using AutoMapper;
using MicroServicioCitas.Application.DTOs;
using MicroServicioCitas.Application.Interfaces;
using MicroServicioCitas.Domain.Interfaces;
using MicroServicioCitas.Domain.Models;
using MicroServicioCitas.Domain.Services;
using MicroServicioPersonas.Domain.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServicioCitas.Application.Services
{
    /// <summary>
    /// Servicio encargado de gestionar las operaciones relacionadas con las citas.
    /// </summary>
    public class CitaService : ICitaService
    {
        private readonly IMapper _mapper;
        private readonly ICitaRepository _citaRepository;
        private readonly ICitaDomainService _citaDomainService;
        private readonly IRabbitMqService _rabbitMqService; // Servicio para RabbitMQ

        /// <summary>
        /// Constructor de la clase CitaService.
        /// </summary>
        /// <param name="mapper">Instancia de IMapper para la conversión de DTOs.</param>
        /// <param name="citaRepository">Repositorio para la gestión de citas.</param>
        /// <param name="citaDomainService">Servicio de dominio para la lógica de negocio de citas.</param>
        /// <param name="rabbitMqService">Servicio para la comunicación con RabbitMQ.</param>
        public CitaService(IMapper mapper,
                            ICitaRepository citaRepository,
                            ICitaDomainService citaDomainService,
                            IRabbitMqService rabbitMqService)
        {
            _mapper = mapper;
            _citaRepository = citaRepository;
            _citaDomainService = citaDomainService;
            _rabbitMqService = rabbitMqService;
        }

        /// <summary>
        /// Obtiene todas las citas.
        /// </summary>
        /// <returns>Lista de citas en formato DTO.</returns>
        public async Task<List<CitaDTO>> GetAll()
        {
            var citas = await _citaRepository.GetAll();
            return _mapper.Map<List<CitaDTO>>(citas);
        }

        /// <summary>
        /// Obtiene una cita por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la cita.</param>
        /// <returns>DTO de la cita encontrada.</returns>
        public async Task<CitaDTO> GetById(int id)
        {
            Cita cita = await _citaRepository.GetById(id);
            _citaDomainService.ExistCita(cita);
            return _mapper.Map<CitaDTO>(cita);
        }

        /// <summary>
        /// Crea una nueva cita.
        /// </summary>
        /// <param name="createCitaDto">DTO que contiene la información de la nueva cita.</param>
        /// <returns>DTO de la cita creada.</returns>
        public async Task<CitaDTO> Create(CreateCitaDTO createCitaDto)
        {
            Cita cita = _mapper.Map<Cita>(createCitaDto);
            await _citaDomainService.ValidateCita(cita);
            cita = await _citaRepository.Add(cita);
            return _mapper.Map<CitaDTO>(cita);
        }

        /// <summary>
        /// Actualiza una cita existente.
        /// </summary>
        /// <param name="id">Identificador de la cita a actualizar.</param>
        /// <param name="updateCitaDto">DTO que contiene la información actualizada de la cita.</param>
        /// <returns>DTO de la cita actualizada.</returns>
        public async Task<CitaDTO> Update(int id, UpdateCitaDTO updateCitaDto)
        {
            Cita cita = _mapper.Map<Cita>(updateCitaDto);
            Cita updatedCita = await _citaRepository.Update(id, cita);
            return _mapper.Map<CitaDTO>(updatedCita);
        }

        /// <summary>
        /// Actualiza el estado de una cita.
        /// </summary>
        /// <param name="id">Identificador de la cita a actualizar.</param>
        /// <param name="nuevoEstado">Nuevo estado a asignar a la cita.</param>
        /// <returns>DTO de la cita actualizada.</returns>
        public async Task<CitaDTO> UpdateEstado(int id, string nuevoEstado)
        {
            Cita cita = await _citaRepository.GetById(id);
            _citaDomainService.ExistCita(cita);
            _citaDomainService.ValidateEstado(cita, nuevoEstado);

            // Actualiza el estado de la cita
            cita.Estado = nuevoEstado;

            Cita objResult = await _citaRepository.UpdateEstado(id, nuevoEstado);

            // Si el estado es "Finalizada", enviar un mensaje a RabbitMQ
            if (objResult.Estado.Equals("finalizada", StringComparison.OrdinalIgnoreCase))
            {
                var recetaData = new
                {
                    CitaId = objResult.Id,
                    PacienteId = objResult.PacienteId,
                    MedicoId = objResult.MedicoId
                };

                // Enviar el mensaje a RabbitMQ para que el microservicio de Recetas procese la creación de una nueva receta
                await _rabbitMqService.SendRecetaRequest(recetaData);
            }
            return _mapper.Map<CitaDTO>(objResult); // Retorna la cita actualizada en dto
        }

        /// <summary>
        /// Elimina una cita por su identificador.
        /// </summary>
        /// <param name="id">Identificador de la cita a eliminar.</param>
        public async Task Delete(int id)
        {
            Cita existCita = await _citaRepository.GetById(id);
            _citaDomainService.ExistCita(existCita);
            await _citaRepository.Delete(id);
        }
    }
}
