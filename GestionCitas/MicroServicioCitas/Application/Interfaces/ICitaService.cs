using MicroServicioCitas.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServicioCitas.Application.Interfaces
{
    public interface ICitaService
    {
        Task<List<Cita>> GetAll();
        Task<Cita> GetById(int id);
        Task<Cita> Create(Cita persona);
        Task<Cita> Update(int id, Cita persona);
        Task<Cita> UpdateEstado(int id, string nuevoEstado);
        Task Delete(int id);
    }
}