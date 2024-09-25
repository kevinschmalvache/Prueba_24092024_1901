using MicroServicioPersonas.Aplication.Interfaces;
using MicroServicioPersonas.Aplication.Services;
using MicroServicioPersonas.Domain.Interfaces;
using MicroServicioPersonas.Domain.Models;
using MicroServicioPersonas.Domain.Services;
using MicroServicioPersonas.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroServicioPersonas.Tests.Application.Services
{
    [TestClass]
    public class PersonaServiceTests
    {
        private IPersonaService _personaService;
        private Mock<IPersonaRepository> _mockRepository;
        private PersonaDomainService _personaDomainService;

        [TestInitialize]
        public void Setup()
        {
            // Inicializa el mock del repositorio y el servicio de dominio
            _mockRepository = new Mock<IPersonaRepository>();
            _personaDomainService = new PersonaDomainService();
            _personaService = new PersonaService(_mockRepository.Object, _personaDomainService);
        }

        [TestMethod]
        public async Task ObtenerTodasLasPersonas_DeberiaRetornarListaDePersonas()
        {
            // Arrange
            var personas = new List<Persona>
            {
                new Persona { Id = 1, Nombre = "Juan", Apellido = "Pérez", TipoDePersona = "Paciente" },
                new Persona { Id = 2, Nombre = "Ana", Apellido = "Gómez", TipoDePersona = "Médico" }
            };

            _mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(personas);

            // Act
            var result = await _personaService.GetAll();

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))] // Esperamos que se lance esta excepción
        public async Task ObtenerPersonaPorId_DeberiaLanzarExcepcion_CuandoNoExiste()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetById(1)).ReturnsAsync((Persona)null);

            // Act
            await _personaService.GetById(1); // Se espera que se lance NotFoundException
        }

        [TestMethod]
        public async Task CrearPersona_DeberiaRetornarPersonaCreada()
        {
            // Arrange
            var nuevaPersona = new Persona { Nombre = "Carlos", Apellido = "López", TipoDePersona = "Paciente" };
            _mockRepository.Setup(repo => repo.Add(nuevaPersona)).ReturnsAsync(nuevaPersona);

            // Act
            var result = await _personaService.Create(nuevaPersona);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(nuevaPersona.Nombre, result.Nombre);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))] // Esperamos que se lance esta excepción
        public async Task CrearPersona_DeberiaLanzarExcepcion_CuandoNombreEsVacio()
        {
            // Arrange
            var nuevaPersona = new Persona { Nombre = "", Apellido = "López", TipoDePersona = "Paciente" };

            // Act
            await _personaService.Create(nuevaPersona); // Se espera que se lance ArgumentException
        }

        [TestMethod]
        public async Task ActualizarPersona_DeberiaRetornarPersonaActualizada()
        {
            // Arrange
            var personaExistente = new Persona { Id = 1, Nombre = "Juan", Apellido = "Pérez", TipoDePersona = "Paciente" };
            var personaActualizar = new Persona { Nombre = "Juan Carlos", Apellido = "Pérez", TipoDePersona = "Paciente" };
            _mockRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(personaExistente);
            _mockRepository.Setup(repo => repo.Update(personaActualizar)).ReturnsAsync(personaActualizar);

            // Act
            var result = await _personaService.Update(1, personaActualizar);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Juan Carlos", result.Nombre);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))] // Esperamos que se lance esta excepción
        public async Task ActualizarPersona_DeberiaLanzarExcepcion_CuandoNoExiste()
        {
            // Arrange
            var personaActualizar = new Persona { Nombre = "Juan Carlos", Apellido = "Pérez", TipoDePersona = "Paciente" };

            // Configurar el mock para que GetById devuelva null para el ID proporcionado
            _mockRepository.Setup(repo => repo.GetById(999)).ReturnsAsync((Persona)null); // ID que simula que no existe

            // Act
            await _personaService.Update(999, personaActualizar); // Se espera que se lance NotFoundException
        }

        [TestMethod]
        public async Task EliminarPersona_DeberiaNoLanzarExcepcion_CuandoPersonaExiste()
        {
            // Arrange
            var personaExistente = new Persona { Id = 1, Nombre = "Juan", Apellido = "Pérez", TipoDePersona = "Paciente" };
            _mockRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(personaExistente);

            // Act
            await _personaService.Delete(1); // No se espera ninguna excepción
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))] // Esperamos que se lance esta excepción
        public async Task EliminarPersona_DeberiaLanzarExcepcion_CuandoNoExiste()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetById(1)).ReturnsAsync((Persona)null);

            // Act
            await _personaService.Delete(1); // Se espera que se lance NotFoundException
        }
    }
}
