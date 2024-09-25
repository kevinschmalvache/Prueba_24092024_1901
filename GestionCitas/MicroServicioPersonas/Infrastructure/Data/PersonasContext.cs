using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroServicioPersonas.Infraestructure.Data
{
    using global::MicroServicioPersonas.Domain.Models;
    using System.Data.Entity;

    public class PersonasContext : DbContext
    {
        public DbSet<Persona> Personas { get; set; }

        public PersonasContext() : base("name=PersonasConnectionString")
        {
        }
    }
}