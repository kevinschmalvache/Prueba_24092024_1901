using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroServicioCitas.Application.DTOs
{
    public class CreateCitaDTO
    {
        public DateTime Fecha { get; set; }
        public string Lugar { get; set; }
        public int PacienteId { get; set; }
        public int MedicoId { get; set; }
    }
}