using System;

namespace MicroServicioCitas.Application.DTOs
{
    public class CitaDTO
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Lugar { get; set; }
        public int PacienteId { get; set; }
        public int MedicoId { get; set; }
        public string Estado { get; set; }
    }
}