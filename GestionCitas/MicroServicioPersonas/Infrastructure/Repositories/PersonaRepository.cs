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

        public PersonaRepository(PersonasContext context)
        {
            _context = context;
        }
        public Task<List<Persona>> GetAll() => _context.Personas.ToListAsync();
        public Task<Persona> GetById(int id) => _context.Personas.FindAsync(id);
        public async Task<Persona> Add(Persona objPersonaCreate)
        {
            _context.Personas.Add(objPersonaCreate);
            await _context.SaveChangesAsync();
            return objPersonaCreate;
        }

        public async Task<Persona> Update(int id, Persona objPersonaUpdate)
        {
            Persona objPersonaOriginal = await _context.Personas.FindAsync(id);

            if (objPersonaOriginal is null)
                throw new NotFoundException($"La persona con el ID especificado no existe.");

            // Lista de propiedades a actualizar
            var propertiesToUpdate = new List<string> { "Nombre", "Apellido" };

            // Itera sobre las propiedades de la entidad actualizada
            //var properties = typeof(Persona).GetProperties();
            foreach (var property in propertiesToUpdate)
            {
                var propertyInfo = typeof(Persona).GetProperty(property);

                var updatedValue = propertyInfo.GetValue(objPersonaUpdate);
                if (updatedValue != null && !string.IsNullOrEmpty(updatedValue.ToString()))
                {
                    propertyInfo.SetValue(objPersonaOriginal, updatedValue);
                    _context.Entry(objPersonaOriginal).Property(property).IsModified = true;
                }
            }
            await _context.SaveChangesAsync();

            // Retornar la entidad actualizada
            return objPersonaOriginal;
        }


        //public void Update(Persona persona)
        //{
        //    _context.Entry(persona).State = EntityState.Modified;
        //    _context.SaveChanges();

        //    // Marca la entidad como modificada
        //    Persona objPersona = _context.Personas.Find(persona.Id);

        //    objPersona.Nombre = persona.Nombre;
        //    objPersona.Apellido = persona.Apellido;
        //    objPersona.TipoDePersona = persona.TipoDePersona;

        //    // Marca la entidad como modificada
        //    _context.Entry(objPersona).State = EntityState.Modified;

        //    // Guarda los cambios
        //    _context.SaveChanges();
        //}
        public async Task Delete(int id)
        {
            Persona persona = await GetById(id);
            _context.Personas.Remove(persona);
            _context.SaveChanges();
        }

        public async Task<bool> ValidatePersonaAsync(int id, string tipoPersona)
        {
            // Usamos Where para filtrar por id y tipoPersona
            return await _context.Personas
                .Where(p => p.Id == id && p.TipoDePersona.Equals(tipoPersona, StringComparison.OrdinalIgnoreCase))
                .AnyAsync(); // Verificamos si existe al menos una coincidencia
        }
    }
}