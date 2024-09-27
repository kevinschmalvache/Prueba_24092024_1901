using MicroServicioRecetas.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServicioRecetas.Domain.Interfaces
{
    public interface IRecetaRepository
    {
        Task<List<Receta>> GetRecetasAsync();
        Task<Receta> GetRecetaByIdAsync(int id);
        Task<Receta> AddRecetaAsync(Receta receta);
        Task<Receta> UpdateRecetaAsync(int id, Receta receta);
        Task DeleteRecetaAsync(int id);
    }
}
