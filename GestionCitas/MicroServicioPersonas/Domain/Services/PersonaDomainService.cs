using MicroServicioPersonas.Domain.Enums;
using MicroServicioPersonas.Domain.Models;
using MicroServicioPersonas.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

            if (!persona.TipoDePersona.ToLower().Equals("medico") 
                && !persona.TipoDePersona.ToLower().Equals("paciente")
               )
            {
                throw new ArgumentException("El tipo de persona no es válido. Debe ser 'Medico' o 'Paciente'.");
            }
        }

        public void ExistPersona(Persona persona)
        {
            if (persona is null)
                throw new NotFoundException($"La persona con el ID especificado no existe.");
        }
    }
}