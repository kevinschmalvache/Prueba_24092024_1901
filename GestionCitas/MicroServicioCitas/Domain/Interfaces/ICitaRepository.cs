using MicroServicioCitas.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServicioCitas.Domain.Interfaces
{
    public interface ICitaRepository
    {
        Task<List<Cita>> GetAll();
        Task<Cita> GetById(int id);
        Task<Cita> Add(Cita persona);
        Task<Cita> Update(Cita persona);
        Task<Cita> UpdateEstado(int id, string nuevoEstado);
        Task Delete(int id);
    }

}