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
    /// <summary>
    /// Clase que implementa las operaciones de acceso a datos para la entidad Receta.
    /// Interactúa con la base de datos utilizando Entity Framework y expone métodos para
    /// obtener, agregar, actualizar y eliminar recetas.
    /// </summary>
    public class RecetaRepository : IRecetaRepository
    {
        private readonly RecetasContext _context;

        /// <summary>
        /// Constructor de la clase RecetaRepository. Inyecta el contexto de la base de datos.
        /// </summary>
        /// <param name="context">Contexto de la base de datos para las recetas.</param>
        public RecetaRepository(RecetasContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todas las recetas de la base de datos de manera asíncrona.
        /// </summary>
        /// <returns>Una lista de recetas.</returns>
        public async Task<List<Receta>> GetRecetasAsync()
        {
            return await _context.Recetas.ToListAsync();
        }

        /// <summary>
        /// Obtiene una receta por su ID de manera asíncrona.
        /// </summary>
        /// <param name="id">El ID de la receta.</param>
        /// <returns>La receta correspondiente al ID.</returns>
        public async Task<Receta> GetRecetaByIdAsync(int id)
        {
            return await _context.Recetas.FirstOrDefaultAsync(r => r.RecetaId == id);
        }

        /// <summary>
        /// Agrega una nueva receta a la base de datos de manera asíncrona.
        /// </summary>
        /// <param name="receta">La receta que se va a agregar.</param>
        /// <returns>La receta recién agregada.</returns>
        public async Task<Receta> AddRecetaAsync(Receta receta)
        {
            _context.Recetas.Add(receta);
            await _context.SaveChangesAsync();
            return receta;
        }

        /// <summary>
        /// Actualiza una receta existente en la base de datos de manera asíncrona.
        /// Solo se actualizan las propiedades permitidas como Descripción y Estado.
        /// </summary>
        /// <param name="id">ID de la receta a actualizar.</param>
        /// <param name="updatedReceta">Receta con los nuevos valores.</param>
        /// <returns>La receta actualizada.</returns>
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

                // Actualiza si el nuevo valor no es null o vacío
                if (newValue != null && !string.IsNullOrEmpty(newValue.ToString()))
                {
                    propertyInfo.SetValue(objOriginalReceta, newValue);
                    _context.Entry(objOriginalReceta).Property(propertyName).IsModified = true;
                }
            }

            await _context.SaveChangesAsync();
            return objOriginalReceta;
        }

        /// <summary>
        /// Elimina una receta de la base de datos de manera asíncrona.
        /// </summary>
        /// <param name="id">ID de la receta a eliminar.</param>
        public async Task DeleteRecetaAsync(int id)
        {
            Receta receta = await _context.Recetas.FindAsync(id);

            if (receta == null)
                throw new NotFoundException("La receta con el ID especificado no existe.");

            _context.Recetas.Remove(receta);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Obtiene todas las recetas relacionadas a un paciente específico por su ID de manera asíncrona.
        /// </summary>
        /// <param name="pacienteId">ID del paciente.</param>
        /// <returns>Lista de recetas asociadas al paciente.</returns>
        public async Task<List<Receta>> GetRecetasByPacienteId(int pacienteId)
        {
            return await _context.Recetas.Where(r => r.PacienteId == pacienteId).ToListAsync();
        }

        /// <summary>
        /// Actualiza el estado de una receta en la base de datos de manera asíncrona.
        /// Verifica si el estado actual es 'vencida' o 'entregada', en cuyo caso se lanza una excepción.
        /// </summary>
        /// <param name="id">ID de la receta a actualizar.</param>
        /// <param name="nuevoEstado">Nuevo estado para la receta.</param>
        /// <returns>True si la operación fue exitosa.</returns>
        public async Task<bool> UpdateEstadoRecetaAsync(int id, string nuevoEstado)
        {
            Receta receta = await _context.Recetas.FindAsync(id);

            if (receta == null)
                throw new NotFoundException("La receta con el ID especificado no existe.");

            // Verificar si el estado actual no permite modificaciones
            if (receta.Estado.Equals("vencida", StringComparison.OrdinalIgnoreCase) ||
                receta.Estado.Equals("entregada", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("No se puede editar una receta en estado 'Vencida' o 'Entregada'.");

            receta.Estado = nuevoEstado;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
