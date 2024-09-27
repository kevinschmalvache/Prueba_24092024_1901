namespace MicroServicioRecetas.Infraestructure.Data
{
    using global::MicroServicioRecetas.Domain.Models;
    using System.Data.Entity;

    public class RecetasContext : DbContext
    {
        public DbSet<Receta> Recetas { get; set; }

        public RecetasContext() : base("name=RecetasConnectionString")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
