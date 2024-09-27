using System;

namespace MicroServicioRecetas.Domain.Models
{
    public class Receta
    {
        public int RecetaId { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Estado { get; set; } // "Activa", "Vencida", "Entregada"

        // Relación con Cita (solo Id)
        public int CitaId { get; set; }

        // Relación con Médico (solo Id)
        public int MedicoId { get; set; }

        // Relación con Paciente (solo Id)
        public int PacienteId { get; set; }
    }
}