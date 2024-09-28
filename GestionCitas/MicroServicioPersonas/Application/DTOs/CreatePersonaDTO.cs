using System.ComponentModel.DataAnnotations;

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