using MicroServicioLogin.Domain.Interfaces;
using MicroServicioLogin.Domain.Models;
using MicroServicioLogin.Infrastructure.Data;
using System;
using System.Linq;

namespace MicroServicioLogin.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly LoginDbContext _context;

        public UsuarioRepository(LoginDbContext context)
        {
            _context = context;
        }

        public Usuario GetByUsername(string username)
        {
            return _context.Usuarios.SingleOrDefault(u => u.Username == username);
        }

        public void Add(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
        }

        public bool Any(Func<Usuario, bool> predicate)
        {
            return _context.Usuarios.Any(predicate);
        }
    }
}
