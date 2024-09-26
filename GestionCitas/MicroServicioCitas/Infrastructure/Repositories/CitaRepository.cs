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
        public async Task<List<Cita>> GetAll() => await _context.Citas.ToListAsync();
        public async Task<Cita> GetById(int id) => await _context.Citas.FindAsync(id);

        public async Task<Cita> Add(Cita cita)
        {
            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();
            return cita;
        }

        public async Task<Cita> Update(Cita objCitaUpdate)
        {
            // Encuentra la cita existente en la base de datos
            Cita objCitaOriginal = await _context.Citas.FindAsync(objCitaUpdate.Id);

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
            Cita existingCita = await GetById(id);

            // Actualiza el estado de la cita
            existingCita.Estado = nuevoEstado;

            // Marca la entidad como modificada
            _context.Entry(existingCita).State = System.Data.Entity.EntityState.Modified;

            await _context.SaveChangesAsync();
            // Retorna la cita actualizada
            return existingCita;
        }

        public async Task Delete(int id)
        {
            Cita cita = await GetById(id);
            _context.Citas.Remove(cita);
            await _context.SaveChangesAsync();
        }
    }
}
