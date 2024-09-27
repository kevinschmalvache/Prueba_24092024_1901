using MicroServicioPersonas.Domain.Enums;
using MicroServicioPersonas.Domain.Models;
using MicroServicioPersonas.Exceptions;
using System;
using System.Linq;

namespace MicroServicioPersonas.Domain.Services
{
    // Se centra en la lógica de negocio y validaciones, manteniendo la integridad de las entidades del dominio.
    public class PersonaDomainService
    {
        public void ValidatePersona(Persona persona)
        {
            if (string.IsNullOrWhiteSpace(persona.Nombre))
            {
                throw new ArgumentException("El nombre no puede estar vacío.");
            }

            if (string.IsNullOrWhiteSpace(persona.Apellido))
            {
                throw new ArgumentException("El apellido no puede estar vacío.");
            }

            // Validar si el tipo persona esta en el enum
            if (!Enum.GetNames(typeof(TipoDePersona)).Contains(persona.TipoDePersona))
            {
                throw new ArgumentException("El estado de la receta no es válido.");
            }

        }

        public void ExistPersona(Persona persona)
        {
            if (persona is null)
                throw new NotFoundException($"La persona con el ID especificado no existe.");
        }
    }
}