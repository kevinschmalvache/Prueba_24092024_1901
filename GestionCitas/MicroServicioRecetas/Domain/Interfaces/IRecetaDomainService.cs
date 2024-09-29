using MicroServicioRecetas.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroServicioPersonas.Domain.Interfaces
{
    public interface IRecetaDomainService
    {
        void ValidateReceta(Receta receta);
        void ValidarEstadosReceta(Receta receta);
        void ExistReceta(Receta receta);
    }
}