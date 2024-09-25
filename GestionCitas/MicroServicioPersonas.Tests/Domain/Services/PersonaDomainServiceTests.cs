using Microsoft.VisualStudio.TestTools.UnitTesting;
using MicroServicioPersonas.Domain.Models;
using MicroServicioPersonas.Domain.Services;
using System;
using MicroServicioPersonas.Exceptions;

namespace MicroServicioPersonas.Tests.Domain.Services
{
    [TestClass] // Indica que esta clase contiene métodos de prueba
    public class PersonaDomainServiceTests
    {
        private PersonaDomainService _personaDomainService; // Variable para la instancia del servicio de dominio

        [TestInitialize] // Este método se ejecuta antes de cada prueba
        public void Setup()
        {
            _personaDomainService = new PersonaDomainService(); // Inicializa el servicio de dominio
        }

        [TestMethod] // Indica que este método es una prueba
        public void ValidarPersona_CuandoNombreEsVacio_DeberiaLanzarExcepcion()
        {
            // Arrange: Configura los datos necesarios para la prueba
            var persona = new Persona { Nombre = "", Apellido = "Doe", TipoDePersona = "Paciente" };

            // Act & Assert: Verifica que se lance una excepción al validar la persona
            Assert.ThrowsException<ArgumentException>(() => _personaDomainService.ValidatePersona(persona), "El nombre no puede estar vacío.");
        }

        [TestMethod] // Indica que este método es una prueba
        public void ValidarPersona_CuandoApellidoEsVacio_DeberiaLanzarExcepcion()
        {
            // Arrange: Configura los datos necesarios para la prueba
            var persona = new Persona { Nombre = "John", Apellido = "", TipoDePersona = "Paciente" };

            // Act & Assert: Verifica que se lance una excepción al validar la persona
            Assert.ThrowsException<ArgumentException>(() => _personaDomainService.ValidatePersona(persona), "El apellido no puede estar vacío.");
        }

        [TestMethod] // Indica que este método es una prueba
        public void ValidarPersona_CuandoTipoDePersonaEsInvalido_DeberiaLanzarExcepcion()
        {
            // Arrange: Configura los datos necesarios para la prueba
            var persona = new Persona { Nombre = "John", Apellido = "Doe", TipoDePersona = "Desconocido" };

            // Act & Assert: Verifica que se lance una excepción al validar la persona
            Assert.ThrowsException<ArgumentException>(() => _personaDomainService.ValidatePersona(persona), "El tipo de persona no es válido. Debe ser 'Medico' o 'Paciente'.");
        }

        [TestMethod] // Indica que este método es una prueba
        public void ExistPersona_CuandoPersonaEsNula_DeberiaLanzarExcepcion()
        {
            // Act & Assert: Verifica que se lance una excepción cuando la persona es nula
            Assert.ThrowsException<NotFoundException>(() => _personaDomainService.ExistPersona(null), "La persona con el ID especificado no existe.");
        }
    }
}
