using AutoMapper;
using MicroServicioPersonas.Aplication.Interfaces;
using MicroServicioPersonas.Application.DTOs;
using MicroServicioPersonas.Domain.Interfaces;
using MicroServicioPersonas.Domain.Models;
using MicroServicioPersonas.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServicioPersonas.Aplication.Services
{
    // Esta clase se encarga de la orquestación de la lógica de negocio relacionada con las personas.
    // Manipula los datos, los valida y coordina la interacción con el repositorio y el servicio de dominio.
    public class PersonaService : IPersonaService
    {
        private readonly IMapper _mapper;
        private readonly IPersonaRepository _personaRepository;
        private readonly PersonaDomainService _personaDomainService;

        /// <summary>
        /// Constructor donde se inyectan las dependencias necesarias.
        /// </summary>
        /// <param name="mapper">Instancia de IMapper para mapeo de entidades.</param>
        /// <param name="personaRepository">Repositorio para interactuar con la base de datos de personas.</param>
        /// <param name="personaDomainService">Servicio de dominio para validaciones de negocio.</param>
        public PersonaService(IMapper mapper, IPersonaRepository personaRepository, PersonaDomainService personaDomainService)
        {
            _mapper = mapper;
            _personaRepository = personaRepository;
            _personaDomainService = personaDomainService; // Servicio de dominio para validaciones
        }

        /// <summary>
        /// Obtiene una lista de todas las personas.
        /// </summary>
        /// <returns>Una lista de objetos PersonaDTO.</returns>
        public async Task<List<PersonaDTO>> GetAll()
        {
            // Llama al repositorio para obtener todas las personas
            List<Persona> personas = await _personaRepository.GetAll();

            // Mapea la lista de entidades Persona a una lista de DTOs
            return _mapper.Map<List<PersonaDTO>>(personas);
        }

        /// <summary>
        /// Obtiene una persona por su ID.
        /// </summary>
        /// <param name="id">ID de la persona.</param>
        /// <returns>El objeto PersonaDTO correspondiente.</returns>
        public async Task<PersonaDTO> GetById(int id)
        {
            // Obtiene la persona desde el repositorio
            Persona objPersona = await _personaRepository.GetById(id);

            // Verifica si la persona existe usando el servicio de dominio
            _personaDomainService.ExistPersona(objPersona);

            // Mapea la entidad Persona a PersonaDTO
            return _mapper.Map<PersonaDTO>(objPersona);
        }

        /// <summary>
        /// Crea una nueva persona.
        /// </summary>
        /// <param name="createPersonaDto">DTO con los datos de la nueva persona.</param>
        /// <returns>El objeto PersonaDTO creado.</returns>
        public async Task<PersonaDTO> Create(CreatePersonaDTO createPersonaDto)
        {
            // Mapea el DTO a la entidad Persona
            Persona objPersona = _mapper.Map<Persona>(createPersonaDto);
            objPersona.Id = 0; // Asegura que el ID sea 0 para la creación

            // Realiza validaciones de negocio usando el servicio de dominio
            _personaDomainService.ValidatePersona(objPersona);

            // Llama al repositorio para agregar la nueva persona a la base de datos
            objPersona = await _personaRepository.Add(objPersona);

            // Mapea la entidad Persona creada a PersonaDTO
            return _mapper.Map<PersonaDTO>(objPersona);
        }

        /// <summary>
        /// Actualiza una persona existente.
        /// </summary>
        /// <param name="id">ID de la persona a actualizar.</param>
        /// <param name="updatePersonaDto">DTO con los datos actualizados de la persona.</param>
        /// <returns>El objeto PersonaDTO actualizado.</returns>
        public async Task<PersonaDTO> Update(int id, UpdatePersonaDTO updatePersonaDto)
        {
            // Mapea las propiedades actualizables del DTO a la entidad Persona
            Persona persona = _mapper.Map<Persona>(updatePersonaDto);

            // Llama al repositorio para actualizar la persona en la base de datos
            PersonaDTO updatedPersonaDto = _mapper.Map<PersonaDTO>(await _personaRepository.Update(id, persona));

            // Retorna el DTO de la persona actualizada
            return updatedPersonaDto;
        }

        /// <summary>
        /// Elimina una persona por su ID.
        /// </summary>
        /// <param name="id">ID de la persona a eliminar.</param>
        public async Task Delete(int id)
        {
            // Obtiene la persona por su ID
            Persona existPersona = await _personaRepository.GetById(id);

            // Valida si la persona existe
            _personaDomainService.ExistPersona(existPersona);

            // Llama al repositorio para eliminar la persona
            await _personaRepository.Delete(id);
        }

        /// <summary>
        /// Valida si una persona existe según su ID y su tipo.
        /// </summary>
        /// <param name="id">ID de la persona.</param>
        /// <param name="tipoPersona">Tipo de persona (por ejemplo, "Paciente" o "Médico").</param>
        /// <returns>Retorna true si la persona es válida, de lo contrario false.</returns>
        public async Task<bool> ValidatePersonaAsync(int id, string tipoPersona)
        {
            // Llama al repositorio para validar la existencia y tipo de la persona
            return await _personaRepository.ValidatePersonaAsync(id, tipoPersona);
        }
    }
}
