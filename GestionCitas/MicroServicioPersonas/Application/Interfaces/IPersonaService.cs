using MicroServicioPersonas.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServicioPersonas.Aplication.Interfaces
{
    public interface IPersonaService
    {
        Task<List<PersonaDTO>> GetAll();
        Task<PersonaDTO> GetById(int id);
        Task<PersonaDTO> Create(CreatePersonaDTO persona);
        Task<PersonaDTO> Update(int id, UpdatePersonaDTO persona);
        Task Delete(int id);
    }
}