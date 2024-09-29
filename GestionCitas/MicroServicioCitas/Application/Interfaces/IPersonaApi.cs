using MicroServicioCitas.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServicioCitas.Application.Interfaces
{
    public interface IPersonaApi
    {
        Task<bool> ValidatePaciente(int id);
        Task<bool> ValidateMedico(int id);
    }
}