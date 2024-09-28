using System;

namespace MicroServicioCitas.Application.DTOs
{
    public class UpdateCitaDTO
    {
        public DateTime? Fecha { get; set; } // Nullable para que pueda omitirse
        public string Lugar { get; set; }
    }
}