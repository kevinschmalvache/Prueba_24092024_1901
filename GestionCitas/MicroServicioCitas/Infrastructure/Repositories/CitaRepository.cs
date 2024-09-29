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

        /// <summary>
        /// Constructor que inyecta el contexto de citas.
        /// </summary>
        /// <param name="context">Contexto de la base de datos para manejar las citas.</param>
        public CitaRepository(CitasContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todas las citas de la base de datos.
        /// </summary>
        /// <returns>Una lista de objetos Cita.</returns>
        public async Task<List<Cita>> GetAll() => await _context.Citas.ToListAsync();

        /// <summary>
        /// Obtiene una cita por su ID.
        /// </summary>
        /// <param name="id">ID de la cita a obtener.</param>
        /// <returns>La cita correspondiente al ID proporcionado.</returns>
        public async Task<Cita> GetById(int id) => await _context.Citas.FindAsync(id);

        /// <summary>
        /// Agrega una nueva cita a la base de datos.
        /// </summary>
        /// <param name="cita">Objeto Cita que contiene los datos de la cita a agregar.</param>
        /// <returns>La cita creada.</returns>
        public async Task<Cita> Add(Cita cita)
        {
            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();
            return cita;
        }

        /// <summary>
        /// Actualiza una cita existente en la base de datos.
        /// </summary>
        /// <param name="id">ID de la cita a actualizar.</param>
        /// <param name="objCitaUpdate">Objeto Cita que contiene los datos actualizados.</param>
        /// <returns>La cita actualizada.</returns>
        /// <exception cref="NotFoundException">Lanzada cuando la cita no existe o está en estado finalizada.</exception>
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

        /// <summary>
        /// Actualiza el estado de una cita existente.
        /// </summary>
        /// <param name="id">ID de la cita a actualizar.</param>
        /// <param name="nuevoEstado">El nuevo estado de la cita.</param>
        /// <returns>La cita con el nuevo estado.</returns>
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

        /// <summary>
        /// Elimina una cita de la base de datos.
        /// </summary>
        /// <param name="id">ID de la cita a eliminar.</param>
        public async Task Delete(int id)
        {
            Cita cita = await GetById(id);
            _context.Citas.Remove(cita);
            await _context.SaveChangesAsync();
        }
    }
}
