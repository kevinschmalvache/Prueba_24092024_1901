using MicroServicioPersonas.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MicroServicioPersonas.Domain.Interfaces
{
    public interface IPersonaRepository
    {
        Task<List<Persona>> GetAll();
        Task<Persona> GetById(int id);
        Task<Persona> Add(Persona persona);
        Task<Persona> Update(Persona persona);
        Task Delete(int id);
    }

}