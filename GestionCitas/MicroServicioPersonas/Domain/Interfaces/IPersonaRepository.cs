using MicroServicioPersonas.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServicioPersonas.Domain.Interfaces
{
    public interface IPersonaRepository
    {
        Task<List<Persona>> GetAll();
        Task<Persona> GetById(int id);
        Task<Persona> Add(Persona persona);
        Task<Persona> Update(int id, Persona persona);
        Task Delete(int id);
    }

}