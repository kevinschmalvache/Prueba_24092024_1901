﻿using System.ComponentModel.DataAnnotations;

namespace MicroServicioPersonas.Domain.Models
{
    public class Persona
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es requerido.")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El tipo de persona es requerido.")]
        public string TipoDePersona { get; set; }
    }
}