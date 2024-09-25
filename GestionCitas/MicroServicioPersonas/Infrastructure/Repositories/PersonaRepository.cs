using MicroServicioPersonas.Domain.Interfaces;
using MicroServicioPersonas.Domain.Models;
using MicroServicioPersonas.Infraestructure.Data;
using System.Collections.Generic;
using System.Data.Entity;
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

        public async Task<Persona> Update(Persona objPersonaUpdate)
        {
            Persona objPersonaOriginal = await _context.Personas.FindAsync(objPersonaUpdate.Id);

            // Itera sobre las propiedades de la entidad actualizada
            var properties = typeof(Persona).GetProperties();
            foreach (var property in properties)
            {
                var updatedValue = property.GetValue(objPersonaUpdate);
                if (updatedValue != null)
                {
                    property.SetValue(objPersonaOriginal, updatedValue);
                    _context.Entry(objPersonaOriginal).Property(property.Name).IsModified = true;
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
    }
}