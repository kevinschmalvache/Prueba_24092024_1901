using MicroServicioRecetas.Domain.Interfaces;
using MicroServicioRecetas.Domain.Models;
using MicroServicioRecetas.Exceptions;
using MicroServicioRecetas.Infraestructure.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServicioRecetas.Infrastructure.Repositories
{
    public class RecetaRepository : IRecetaRepository
    {
        private readonly RecetasContext _context;

        public RecetaRepository(RecetasContext context)
        {
            _context = context;
        }

        public async Task<List<Receta>> GetRecetasAsync()
        {
            return await _context.Recetas.ToListAsync();
        }

        public async Task<Receta> GetRecetaByIdAsync(int id)
        {
            return await _context.Recetas.FirstOrDefaultAsync(r => r.RecetaId == id);
        }

        public async Task<Receta> AddRecetaAsync(Receta receta)
        {
            _context.Recetas.Add(receta);
            await _context.SaveChangesAsync();
            return receta;
        }

        public async Task<Receta> UpdateRecetaAsync(int id, Receta updatedReceta)
        {
            Receta objOriginalReceta = await _context.Recetas.FindAsync(id);

            if (objOriginalReceta == null)
            {
                throw new NotFoundException("La receta con el ID especificado no existe.");
            }

            // Lista de propiedades a actualizar
            var propertiesToUpdate = new List<string> { "Descripcion", "Estado" };

            foreach (var propertyName in propertiesToUpdate)
            {
                var propertyInfo = typeof(Receta).GetProperty(propertyName);
                var newValue = propertyInfo.GetValue(updatedReceta);

                // Solo actualiza si el nuevo valor no es null o vacío
                if (newValue != null && !string.IsNullOrEmpty(newValue.ToString()))
                {
                    propertyInfo.SetValue(objOriginalReceta, newValue);
                    _context.Entry(objOriginalReceta).Property(propertyName).IsModified = true;
                }
                // Si el nuevo valor es null o vacío, no se hace nada y se conserva el valor original.
            }

            await _context.SaveChangesAsync();
            return objOriginalReceta;
        }

        public async Task DeleteRecetaAsync(int id)
        {
            Receta receta = await _context.Recetas.FindAsync(id);

            if (receta == null)
                throw new NotFoundException("La receta con el ID especificado no existe.");

            _context.Recetas.Remove(receta);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Receta>> GetRecetasByPacienteId(int pacienteId)
        {
            return await _context.Recetas.Where(r => r.PacienteId == pacienteId)
                                         .ToListAsync();
        }

        public async Task<bool> UpdateEstadoRecetaAsync(int id, string nuevoEstado)
        {
            Receta receta = await _context.Recetas.FindAsync(id);

            if (receta == null)
                throw new NotFoundException("La receta con el ID especificado no existe.");

            if (receta.Estado.ToLower().Equals("vencida") || receta.Estado.ToLower().Equals("entregada"))
                throw new InvalidOperationException("No se puede editar una receta en estado 'Vencida' o 'Entregada'.");

            receta.Estado = nuevoEstado;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
