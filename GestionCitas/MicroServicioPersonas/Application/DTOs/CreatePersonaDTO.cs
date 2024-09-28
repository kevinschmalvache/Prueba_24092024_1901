using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MicroServicioPersonas.Application.DTOs
{
    public class CreatePersonaDTO
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El tipo de persona es requerido.")]
        public string TipoDePersona { get; set; }  // Validado contra el enum
    }
}