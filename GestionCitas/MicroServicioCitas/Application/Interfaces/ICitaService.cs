using MicroServicioCitas.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServicioCitas.Application.Interfaces
{
    public interface ICitaService
    {
        Task<List<CitaDTO>> GetAll();
        Task<CitaDTO> GetById(int id);
        Task<CitaDTO> Create(CreateCitaDTO persona);
        Task<CitaDTO> Update(int id, UpdateCitaDTO persona);
        Task<CitaDTO> UpdateEstado(int id, string nuevoEstado);
        Task Delete(int id);
    }
}