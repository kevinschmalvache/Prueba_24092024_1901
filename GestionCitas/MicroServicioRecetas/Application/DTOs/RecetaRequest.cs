namespace MicroServicioRecetas.Application.DTOs
{
    public class RecetaRequest
    {
        public int CitaId { get; set; }
        public int PacienteId { get; set; }
        public int MedicoId { get; set; }
    }
}
