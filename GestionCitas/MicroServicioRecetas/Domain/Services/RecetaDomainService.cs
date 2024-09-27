using MicroServicioRecetas.Domain.Enums;
using MicroServicioRecetas.Domain.Models;
using MicroServicioRecetas.Exceptions;
using System;
using System.Linq;

namespace MicroServicioRecetas.Domain.Services
{
    // Esta clase gestiona la lógica de negocio y validaciones específicas para las recetas
    public class RecetaDomainService
    {
        public void ValidateReceta(Receta receta)
        {
            if (string.IsNullOrWhiteSpace(receta.Descripcion))
            {
                throw new ArgumentException("El código único de la receta no puede estar vacío.");
            }

            if (receta.FechaCreacion == DateTime.MinValue)
            {
                throw new ArgumentException("La fecha de creación de la receta es inválida.");
            }

            // Validar si el estado de la receta es un valor válido en el Enum EstadoReceta
            if (!Enum.GetNames(typeof(EstadoReceta)).Contains(receta.Estado))
            {
                throw new ArgumentException("El estado de la receta no es válido.");
            }
        }

        public void ExistReceta(Receta receta)
        {
            if (receta == null)
            {
                throw new NotFoundException("La receta con el ID especificado no existe.");
            }
        }
    }
}
