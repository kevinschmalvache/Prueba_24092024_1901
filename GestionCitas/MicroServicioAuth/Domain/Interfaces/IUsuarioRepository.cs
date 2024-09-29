using MicroServicioLogin.Domain.Models;
using System;

namespace MicroServicioLogin.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Usuario GetByUsername(string username);
        void Add(Usuario usuario);
        bool Any(Func<Usuario, bool> predicate); // Opcional: para la validación de usuarios
    }
}
