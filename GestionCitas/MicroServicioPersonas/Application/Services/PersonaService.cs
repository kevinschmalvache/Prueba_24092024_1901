using MicroServicioPersonas.Aplication.Interfaces;
using MicroServicioPersonas.Domain.Interfaces;
using MicroServicioPersonas.Domain.Models;
using MicroServicioPersonas.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServicioPersonas.Aplication.Services
{
    //Se ocupa de la orquestación, manipulación de datos y control de flujo.
    public class PersonaService : IPersonaService
    {
        private readonly IPersonaRepository _personaRepository;
        private readonly PersonaDomainService _personaDomainService;

        // Constructor donde se inyecta el repositorio
        public PersonaService(IPersonaRepository personaRepository, PersonaDomainService personaDomainService)
        {
            _personaRepository = personaRepository;
            _personaDomainService = personaDomainService; // Asignar
        }

        public Task<List<Persona>> GetAll()
        {
            return _personaRepository.GetAll();
        }

        public async Task<Persona> GetById(int id)
        {
            Persona objPersona = await _personaRepository.GetById(id);
            _personaDomainService.ExistPersona(objPersona);

            return objPersona;
        }

        public async Task<Persona> Create(Persona objPersona)
        {
            // Validaciones de negocio antes de agregar
            _personaDomainService.ValidatePersona(objPersona);

            // Llamada asincrónica al repositorio
            objPersona = await _personaRepository.Add(objPersona);

            return objPersona;
        }

        public async Task<Persona> Update(int id, Persona objPersona)
        {

            // Obtener la existPersona existente
            Persona existPersona = await _personaRepository.GetById(id);
            _personaDomainService.ExistPersona(existPersona);

            // Validaciones de negocio antes de actualizar
            objPersona.Id = id;

            // Retornar la entidad actualizada
            return await _personaRepository.Update(objPersona);
        }


        public async Task Delete(int id)
        {
            // Obtener la existPersona existente
            Persona existPersona = await _personaRepository.GetById(id);
            _personaDomainService.ExistPersona(existPersona);

            _personaRepository.Delete(id);
        }
    }
}