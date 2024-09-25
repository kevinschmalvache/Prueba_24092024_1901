using MicroServicioPersonas.Domain.Models;
using MicroServicioPersonas.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

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