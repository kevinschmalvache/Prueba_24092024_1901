namespace MicroServicioPersonas.Application.DTOs
{
    public class CreateRecetaDTO
    {

        public string Descripcion { get; set; }
        public string Estado { get; set; }

        // Relación con Cita (solo Id)
        public int CitaId { get; set; }

        // Relación con Médico (solo Id)
        public int MedicoId { get; set; }

        // Relación con Paciente (solo Id)
        public int PacienteId { get; set; }
    }
}