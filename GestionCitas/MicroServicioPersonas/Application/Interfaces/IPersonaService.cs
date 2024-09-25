using MicroServicioPersonas.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServicioPersonas.Aplication.Interfaces
{
    public interface IPersonaService
    {
        Task<List<Persona>> GetAll();
        Task<Persona> GetById(int id);
        Task<Persona> Create(Persona persona);
        Task<Persona> Update(int id, Persona persona);
        Task Delete(int id);
    }
}