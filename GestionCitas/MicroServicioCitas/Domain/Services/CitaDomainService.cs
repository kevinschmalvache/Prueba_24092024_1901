using MicroServicioCitas.Application.Interfaces;
using MicroServicioCitas.Domain.Enums;
using MicroServicioCitas.Domain.Models;
using MicroServicioCitas.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServicioCitas.Domain.Services
{
    // Lógica de negocio y validaciones, manteniendo la integridad de las entidades del dominio.
    public class CitaDomainService
    {

        private readonly IPersonaApi _personaApi; // Inyección de la interfaz de persona API

        public CitaDomainService(IPersonaApi personaApi)
        {
            _personaApi = personaApi;
        }

        public void ValidateEstado(Cita cita, string nuevoEstado)
        {
            if (string.IsNullOrWhiteSpace(nuevoEstado))
                throw new ArgumentException("El estado de la cita no es válido.");

            // Validar si el tipo persona esta en el enum
            if (!Enum.GetNames(typeof(EstadoCita)).Contains(cita.Estado, StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException("El estado de la cita no es válido.");
            }

            // Validar que el estado de la cita sea "Pendiente" o "En proceso"
            if (nuevoEstado.Equals("finalizada", StringComparison.OrdinalIgnoreCase) &&
                !cita.Estado.Equals("pendiente", StringComparison.OrdinalIgnoreCase) &&
                !cita.Estado.Equals("enproceso", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Solo se pueden finalizar citas que estén en estado 'Pendiente' o 'En proceso'.");
            }

        }

        public async Task ValidateCita(Cita cita)
        {
            if (cita == null)
                throw new ArgumentNullException(nameof(cita), "La cita no puede ser nula.");

            if (cita.Fecha == default)
                throw new ArgumentException("La fecha de la cita es obligatoria.");

            if (string.IsNullOrWhiteSpace(cita.Lugar))
                throw new ArgumentException("El lugar de la cita es obligatorio.");

            // Validar que el paciente existe
            if (!await _personaApi.ValidatePaciente(cita.PacienteId))
                throw new ArgumentException("El paciente especificado no existe.");

            // Validar que el médico existe
            if (!await _personaApi.ValidateMedico(cita.MedicoId))
                throw new ArgumentException("El médico especificado no existe.");

            //if (string.IsNullOrWhiteSpace(cita.Estado))
            //    throw new ArgumentException("El estado de la cita es obligatorio.");

            //ValidateEstado(cita);

            // if (!PacienteExists(cita.PacienteId)) 
            //     throw new ArgumentException("El paciente especificado no existe.");
            // if (!MedicoExists(cita.MedicoId)) 
            //     throw new ArgumentException("El médico especificado no existe.");

            // Asegúrate de que el estado inicial sea correcto
            //if (cita.Estado != "Pendiente")
            //    throw new ArgumentException("El estado de la cita debe ser 'Pendiente' al crear.");
        }

        public void ExistCita(Cita cita)
        {
            if (cita == null)
                throw new NotFoundException("La cita no existe.");
        }
    }
}