using System;

namespace MicroServicioLogin.Domain.Models
{
    public class Usuario
    {
        public int UsuarioId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
