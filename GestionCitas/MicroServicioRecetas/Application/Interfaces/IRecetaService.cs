using MicroServicioPersonas.Application.DTOs;
using MicroServicioRecetas.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServicioRecetas.Application.Interfaces
{
    public interface IRecetaService
    {
        Task<List<RecetaDTO>> GetRecetas();
        Task<RecetaDTO> GetRecetaById(int id);
        Task<RecetaDTO> AddReceta(CreateRecetaDTO recetaDto);
        Task<RecetaDTO> UpdateReceta(int id, UpdateRecetaDTO recetaDto);
        Task DeleteReceta(int id);
    }
}
