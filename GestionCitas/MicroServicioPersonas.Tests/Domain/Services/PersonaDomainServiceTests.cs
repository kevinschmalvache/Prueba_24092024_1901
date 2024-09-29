using Microsoft.VisualStudio.TestTools.UnitTesting;
using MicroServicioPersonas.Domain.Models;
using MicroServicioPersonas.Domain.Services;
using System;

namespace MicroServicioPersonas.Tests.Domain.Services
{
    [TestClass]
    public class PersonaDomainServiceTests
    {
        private PersonaDomainService _personaDomainService;

        // Configuración inicial
        [TestInitialize]
        public void Configuracion()
        {
            _personaDomainService = new PersonaDomainService();
        }

        // Prueba para validar que se lanza una excepción si la persona es inválida
        [TestMethod]
        public void ValidarPersona_DeberiaLanzarExcepcionSiEsInvalida()
        {
            // Arrange
            var personaInvalida = new Persona { Nombre = "", Apellido = "" };

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => _personaDomainService.ValidatePersona(personaInvalida));
        }

    }
}
