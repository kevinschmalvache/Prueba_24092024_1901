using MicroServicioLogin.Domain.Models;
using System.Collections.Generic;
using System.Data.Entity;

namespace MicroServicioLogin.Infrastructure.Data
{
    public class LoginDbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }

        public LoginDbContext() : base("name=LoginDbConnection")
        {
        }
    }
}
