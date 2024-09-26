using MicroServicioCitas.Domain.Models;
using MicroServicioCitas.Exceptions;
using System;

namespace MicroServicioCitas.Domain.Services
{
    // Se centra en la lógica de negocio y validaciones, manteniendo la integridad de las entidades del dominio.
    public class CitaDomainService
    {
        public void ValidateCita(Cita cita)
        {
            if (cita == null)
                throw new ArgumentNullException(nameof(cita), "La cita no puede ser nula.");

            if (cita.Fecha == default)
                throw new ArgumentException("La fecha de la cita es obligatoria.");

            if (string.IsNullOrWhiteSpace(cita.Lugar))
                throw new ArgumentException("El lugar de la cita es obligatorio.");

            // Asegúrate de que el Paciente y el Médico existan (podrías consultar en sus respectivos repositorios)
            // if (!PacienteExists(cita.PacienteId)) 
            //     throw new ArgumentException("El paciente especificado no existe.");
            // if (!MedicoExists(cita.MedicoId)) 
            //     throw new ArgumentException("El médico especificado no existe.");

            // Asegúrate de que el estado inicial sea correcto
            if (cita.Estado != "Pendiente")
                throw new ArgumentException("El estado de la cita debe ser 'Pendiente' al crear.");
        }

        public void ExistCita(Cita cita)
        {
            if (cita == null)
                throw new NotFoundException("La cita no existe.");
        }
    }
}