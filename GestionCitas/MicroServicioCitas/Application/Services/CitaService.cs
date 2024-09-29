using AutoMapper;
using MicroServicioCitas.Application.DTOs;
using MicroServicioCitas.Application.Interfaces;
using MicroServicioCitas.Domain.Interfaces;
using MicroServicioCitas.Domain.Models;
using MicroServicioCitas.Domain.Services;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServicioCitas.Application.Services
{
    public class CitaService : ICitaService
    {
        private readonly IMapper _mapper;
        private readonly ICitaRepository _citaRepository;
        private readonly CitaDomainService _citaDomainService;
        private readonly IRabbitMqService _rabbitMqService; // Servicio para RabbitMQ

        public CitaService(IMapper mapper,
                            ICitaRepository citaRepository,
                            CitaDomainService citaDomainService,
                            IRabbitMqService rabbitMqService)
        {
            _mapper = mapper;
            _citaRepository = citaRepository;
            _citaDomainService = citaDomainService;
            _rabbitMqService = rabbitMqService;
        }

        public async Task<List<CitaDTO>> GetAll()
        {
            var citas = await _citaRepository.GetAll();
            return _mapper.Map<List<CitaDTO>>(citas);
        }

        public async Task<CitaDTO> GetById(int id)
        {
            Cita cita = await _citaRepository.GetById(id);
            _citaDomainService.ExistCita(cita);
            return _mapper.Map<CitaDTO>(cita);
        }

        public async Task<CitaDTO> Create(CreateCitaDTO createCitaDto)
        {
            Cita cita = _mapper.Map<Cita>(createCitaDto);
            await _citaDomainService.ValidateCita(cita);
            cita = await _citaRepository.Add(cita);
            return _mapper.Map<CitaDTO>(cita);
        }

        public async Task<CitaDTO> Update(int id, UpdateCitaDTO updateCitaDto)
        {
            Cita cita = _mapper.Map<Cita>(updateCitaDto);
            Cita updatedCita = await _citaRepository.Update(id, cita);
            return _mapper.Map<CitaDTO>(updatedCita);
        }

        public async Task<CitaDTO> UpdateEstado(int id, string nuevoEstado)
        {
            Cita cita = await _citaRepository.GetById(id);
            _citaDomainService.ExistCita(cita);
            _citaDomainService.ValidateEstado(cita, nuevoEstado);

            // Actualiza el estado de la cita
            cita.Estado = nuevoEstado;

            Cita objResult = await _citaRepository.UpdateEstado(id, nuevoEstado);

            // Si el estado es "Finalizada", enviar un mensaje a RabbitMQ
            if (objResult.Estado.ToLower().Equals("finalizada"))
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

        public async Task Delete(int id)
        {
            Cita existCita = await _citaRepository.GetById(id);
            _citaDomainService.ExistCita(existCita);
            await _citaRepository.Delete(id);
        }
    }
}
