using MicroServicioPersonas.Domain.Interfaces;
using MicroServicioPersonas.Domain.Models;
using MicroServicioPersonas.Exceptions;
using MicroServicioPersonas.Infraestructure.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServicioPersonas.Infraestructure.Repositories
{
    public class PersonaRepository : IPersonaRepository
    {
        private readonly PersonasContext _context;

        /// <summary>
        /// Constructor que inicializa el contexto de la base de datos.
        /// </summary>
        /// <param name="context">Contexto de la base de datos de personas.</param>
        public PersonaRepository(PersonasContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene una lista de todas las personas.
        /// </summary>
        /// <returns>Una lista de objetos Persona.</returns>
        public Task<List<Persona>> GetAll() => _context.Personas.ToListAsync();

        /// <summary>
        /// Obtiene una persona específica por su ID.
        /// </summary>
        /// <param name="id">ID de la persona.</param>
        /// <returns>Una instancia de Persona si se encuentra, de lo contrario null.</returns>
        public Task<Persona> GetById(int id) => _context.Personas.FindAsync(id);

        /// <summary>
        /// Agrega una nueva persona a la base de datos.
        /// </summary>
        /// <param name="objPersonaCreate">Objeto Persona que se va a agregar.</param>
        /// <returns>La persona recién creada.</returns>
        public async Task<Persona> Add(Persona objPersonaCreate)
        {
            _context.Personas.Add(objPersonaCreate);
            await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos.
            return objPersonaCreate; // Retorna la persona creada.
        }

        /// <summary>
        /// Actualiza una persona existente en la base de datos.
        /// </summary>
        /// <param name="id">ID de la persona a actualizar.</param>
        /// <param name="objPersonaUpdate">Objeto Persona con los valores actualizados.</param>
        /// <returns>La persona actualizada.</returns>
        /// <exception cref="NotFoundException">Se lanza si la persona no es encontrada.</exception>
        public async Task<Persona> Update(int id, Persona objPersonaUpdate)
        {
            Persona objPersonaOriginal = await _context.Personas.FindAsync(id);

            if (objPersonaOriginal is null)
                throw new NotFoundException($"La persona con el ID especificado no existe."); // Lanza excepción si no se encuentra la persona.

            // Lista de propiedades que se actualizarán
            var propertiesToUpdate = new List<string> { "Nombre", "Apellido" };

            // Itera sobre las propiedades de la entidad actualizada
            foreach (var property in propertiesToUpdate)
            {
                var propertyInfo = typeof(Persona).GetProperty(property); // Obtiene la propiedad a actualizar.
                var updatedValue = propertyInfo.GetValue(objPersonaUpdate); // Obtiene el valor actualizado.

                // Verifica que el valor no sea nulo o vacío antes de actualizar
                if (updatedValue != null && !string.IsNullOrEmpty(updatedValue.ToString()))
                {
                    propertyInfo.SetValue(objPersonaOriginal, updatedValue); // Actualiza el valor en la entidad original.
                    _context.Entry(objPersonaOriginal).Property(property).IsModified = true; // Marca la propiedad como modificada.
                }
            }

            await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos.
            return objPersonaOriginal; // Retorna la persona actualizada.
        }

        /// <summary>
        /// Elimina una persona de la base de datos por su ID.
        /// </summary>
        /// <param name="id">ID de la persona a eliminar.</param>
        public async Task Delete(int id)
        {
            Persona persona = await GetById(id); // Obtiene la persona a eliminar.
            _context.Personas.Remove(persona); // Elimina la persona del contexto.
            _context.SaveChanges(); // Guarda los cambios en la base de datos.
        }

        /// <summary>
        /// Valida si una persona existe según su ID y su tipo de persona.
        /// </summary>
        /// <param name="id">ID de la persona.</param>
        /// <param name="tipoPersona">Tipo de persona (por ejemplo, "Paciente", "Médico").</param>
        /// <returns>Retorna true si la persona es válida, de lo contrario false.</returns>
        public async Task<bool> ValidatePersonaAsync(int id, string tipoPersona)
        {
            // Filtra por el ID y el tipo de persona, y verifica si existe alguna coincidencia.
            return await _context.Personas
                .Where(p => p.Id == id && p.TipoDePersona.Equals(tipoPersona, StringComparison.OrdinalIgnoreCase))
                .AnyAsync(); // Verifica si existe al menos una coincidencia.
        }
    }
}
