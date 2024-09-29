using MicroServicioCitas.Domain.Interfaces;
using MicroServicioCitas.Domain.Models;
using MicroServicioCitas.Exceptions;
using MicroServicioCitas.Infraestructure.Data;
using System;
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

        public async Task<Cita> Update(int id, Cita objCitaUpdate)
        {
            // Encuentra la cita existente en la base de datos
            Cita objCitaOriginal = await _context.Citas.FindAsync(id);

            if (objCitaOriginal == null)
                throw new NotFoundException("La cita no existe.");

            if (objCitaOriginal.Estado.Equals("finalizada", StringComparison.OrdinalIgnoreCase))
                throw new NotFoundException("No se puede editar una cita en estado finalizada.");

            // Itera sobre las propiedades de la entidad actualizada
            var propertiesToUpdate = new List<string> { "Lugar", "Fecha" };
            foreach (var property in propertiesToUpdate)
            {
                var propertyInfo = typeof(Cita).GetProperty(property);
                var updatedValue = propertyInfo.GetValue(objCitaUpdate);
                if (updatedValue != null && !string.IsNullOrEmpty(updatedValue.ToString()))
                {
                    propertyInfo.SetValue(objCitaOriginal, updatedValue);
                    _context.Entry(objCitaOriginal).Property(property).IsModified = true;
                }
            }

            // Guarda los cambios en la base de datos
            await _context.SaveChangesAsync();

            // Retorna la entidad actualizada
            return objCitaOriginal;
        }

        public async Task<Cita> UpdateEstado(int id, string nuevoEstado)
        {
            Cita objCita = await _context.Citas.FindAsync(id);

            // Actualiza el estado de la cita
            objCita.Estado = nuevoEstado;

            // Marca la entidad como modificada
            _context.Entry(objCita).State = System.Data.Entity.EntityState.Modified;

            await _context.SaveChangesAsync();
            // Retorna la cita actualizada
            return objCita;
        }

        public async Task Delete(int id)
        {
            Cita cita = await GetById(id);
            _context.Citas.Remove(cita);
            await _context.SaveChangesAsync();
        }
    }
}
