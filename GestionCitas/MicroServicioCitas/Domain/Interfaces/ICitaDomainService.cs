using MicroServicioCitas.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MicroServicioPersonas.Domain.Interfaces
{
    public interface ICitaDomainService
    {
        Task ValidateCita(Cita cita);
        void ExistCita(Cita cita);
        void ValidateEstado(Cita cita, string nuevoEstado);
    }

}