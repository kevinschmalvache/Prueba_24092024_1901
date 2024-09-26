namespace MicroServicioCitas.Infraestructure.Data
{
    using global::MicroServicioCitas.Domain.Models;
    using System.Data.Entity;

    public class CitasContext : DbContext
    {
        public DbSet<Cita> Citas { get; set; }

        public CitasContext() : base("name=CitasConnectionString")
        {
        }
    }
}