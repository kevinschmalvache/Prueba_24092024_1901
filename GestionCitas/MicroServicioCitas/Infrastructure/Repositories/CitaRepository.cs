using MicroServicioCitas.Domain.Interfaces;
using MicroServicioCitas.Domain.Models;
using MicroServicioCitas.Infraestructure.Data;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace MicroServicioCitas.Infraestructure.Repositories
{
    public class CitaRepository : ICitaRepository
    {
        private readonly CitasContext _context;

        public CitaRepository(CitasContext context)
        {
            _context = context;
        }

        // Obtener todas las citas
        public async Task<List<Cita>> GetAll()
        {
            return await _context.Citas.ToListAsync(); // Asegúrate de que estás utilizando el DbSet correcto
        }

        // Obtener una cita por ID
        public async Task<Cita> GetById(int id)
        {
            return await _context.Citas.FindAsync(id); // Asegúrate de que estás utilizando el DbSet correcto
        }

        // Agregar una nueva cita
        public async Task<Cita> Add(Cita cita)
        {
            _context.Citas.Add(cita);
            await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos
            return cita; // Retorna la cita creada
        }

        // Actualizar una cita existente
        public async Task<Cita> Update(Cita objCitaUpdate)
        {
            // Encuentra la cita existente en la base de datos
            Cita objCitaOriginal = await _context.Citas.FindAsync(objCitaUpdate.Id);
            if (objCitaOriginal == null)
            {
                throw new KeyNotFoundException("Cita no encontrada."); // Manejo de errores
            }

            // Itera sobre las propiedades de la entidad actualizada
            var properties = typeof(Cita).GetProperties();
            foreach (var property in properties)
            {
                // Obtiene el valor actualizado
                var updatedValue = property.GetValue(objCitaUpdate);
                if (updatedValue != null)
                {
                    // Actualiza el valor en la cita original
                    property.SetValue(objCitaOriginal, updatedValue);
                    // Marca la propiedad como modificada
                    _context.Entry(objCitaOriginal).Property(property.Name).IsModified = true;
                }
            }

            // Guarda los cambios en la base de datos
            await _context.SaveChangesAsync();

            // Retorna la entidad actualizada
            return objCitaOriginal;
        }

        public async Task<Cita> UpdateEstado(int id, string nuevoEstado)
        {
            var existingCita = await GetById(id);
            if (existingCita == null)
            {
                throw new KeyNotFoundException("Cita no encontrada.");
            }

            // Actualiza el estado de la cita
            existingCita.Estado = nuevoEstado;

            // Marca la entidad como modificada
            _context.Entry(existingCita).State = System.Data.Entity.EntityState.Modified;

            await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos

            return existingCita; // Retorna la cita actualizada
        }
        // Eliminar una cita
        public async Task Delete(int id)
        {
            var cita = await GetById(id);
            if (cita == null)
            {
                throw new KeyNotFoundException("Cita no encontrada."); // Manejo de errores
            }

            _context.Citas.Remove(cita);
            await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos
        }
    }
}
