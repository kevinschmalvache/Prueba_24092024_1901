using System;
using System.ComponentModel.DataAnnotations;

namespace MicroServicioCitas.Domain.Models
{
    public class Cita
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La Fecha es requerida.")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El lugar es requerido.")]
        public string Lugar { get; set; }

        [Required(ErrorMessage = "El PacienteId es requerido.")]
        public int PacienteId { get; set; } // Asumiendo que tienes un modelo de Paciente

        [Required(ErrorMessage = "El MedicoId es requerido.")]
        public int MedicoId { get; set; } // Asumiendo que tienes un modelo de Medico

        [Required(ErrorMessage = "El Estado es requerido.")]
        public string Estado { get; set; } // "Pendiente", "En proceso", "Finalizada"

        public Cita()
        {
            Estado = "Pendiente";
        }
    }
}