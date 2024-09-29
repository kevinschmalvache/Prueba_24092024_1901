using AutoMapper;
using MicroServicioPersonas.Aplication.Interfaces;
using MicroServicioPersonas.Application.DTOs;
using MicroServicioPersonas.Domain.Interfaces;
using MicroServicioPersonas.Domain.Models;
using MicroServicioPersonas.Domain.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServicioPersonas.Aplication.Services
{
    //Se ocupa de la orquestación, manipulación de datos y control de flujo.
    public class PersonaService : IPersonaService
    {
        private readonly IMapper _mapper;
        private readonly IPersonaRepository _personaRepository;
        private readonly PersonaDomainService _personaDomainService;

        // Constructor donde se inyecta el repositorio
        public PersonaService(IMapper mapper, IPersonaRepository personaRepository, PersonaDomainService personaDomainService)
        {
            _mapper = mapper;
            _personaRepository = personaRepository;
            _personaDomainService = personaDomainService; // Asignar
        }

        public async Task<List<PersonaDTO>> GetAll()
        {
            // Obtener la lista de personas desde el repositorio
            List<Persona> personas = await _personaRepository.GetAll();
            return _mapper.Map<List<PersonaDTO>>(personas);
        }

        public async Task<PersonaDTO> GetById(int id)
        {
            // Obtener la persona existente
            Persona objPersona = await _personaRepository.GetById(id);

            // Validar que la persona exista
            _personaDomainService.ExistPersona(objPersona);

            return _mapper.Map<PersonaDTO>(objPersona);
        }

        public async Task<PersonaDTO> Create(CreatePersonaDTO createPersonaDto)
        {
            // Mapeo del DTO a la entidad Persona
            Persona objPersona = _mapper.Map<Persona>(createPersonaDto);

            // Validaciones de negocio antes de agregar
            _personaDomainService.ValidatePersona(objPersona);

            // Llamada asincrónica al repositorio para agregar la persona
            objPersona = await _personaRepository.Add(objPersona);

            // Retornar el objeto Persona mapeado a PersonaDTO
            return _mapper.Map<PersonaDTO>(objPersona);
        }

        public async Task<PersonaDTO> Update(int id, UpdatePersonaDTO updatePersonaDto)
        {
            // Obtener la persona existente
            //Persona existPersona = await _personaRepository.GetById(id);

            // Validar que la persona exista
            //_personaDomainService.ExistPersona(existPersona);

            // Mapear solo las propiedades actualizables (Nombre y Apellido) a la entidad existente
            //_mapper.Map(updatePersonaDto, existPersona);
            Persona persona = _mapper.Map<Persona>(updatePersonaDto);

            // Actualizar la entidad en la base de datos
            // Mapear la entidad actualizada a PersonaDTO
            PersonaDTO updatedPersonaDto = _mapper.Map<PersonaDTO>(await _personaRepository.Update(id, persona));

            // Retornar el DTO de la persona actualizada
            return updatedPersonaDto;
        }


        public async Task Delete(int id)
        {
            // Obtener la existPersona existente
            Persona existPersona = await _personaRepository.GetById(id);
            _personaDomainService.ExistPersona(existPersona);

            await _personaRepository.Delete(id);
        }

        // Método para validar si una persona existe y su tipo coincide
        public async Task<bool> ValidatePersonaAsync(int id, string tipoPersona)
        {
            return await _personaRepository.ValidatePersonaAsync(id, tipoPersona);
        }
    }
}