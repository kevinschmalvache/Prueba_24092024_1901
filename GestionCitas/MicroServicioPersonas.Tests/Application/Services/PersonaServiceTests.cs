using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using AutoMapper;
using MicroServicioPersonas.Application.DTOs;
using MicroServicioPersonas.Domain.Interfaces;
using MicroServicioPersonas.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MicroServicioPersonas.Aplication.Services;
using MicroServicioPersonas.Domain.Services;

namespace MicroServicioPersonas.Tests.Application.Services
{
    [TestClass]
    public class PersonaServiceTests
    {
        private Mock<IPersonaRepository> _personaRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private PersonaService _personaService;
        private Mock<PersonaDomainService> _mockPersonaDomainService;

        // Método que se ejecuta antes de cada prueba
        [TestInitialize]
        public void Configuracion()
        {
            _personaRepositoryMock = new Mock<IPersonaRepository>();
            _mapperMock = new Mock<IMapper>();
            _mockPersonaDomainService = new Mock<PersonaDomainService>();

            // Asegúrate de pasar el mock de PersonaDomainService al constructor
            _personaService = new PersonaService(_mapperMock.Object, _personaRepositoryMock.Object, _mockPersonaDomainService.Object);
        }

        // Prueba para verificar que se obtienen todas las personas correctamente
        [TestMethod]
        public async Task ObtenerTodasLasPersonas_DeberiaRetornarListaDePersonas()
        {
            // Arrange (Preparar)
            var personas = new List<Persona> { new Persona { Id = 1, Nombre = "Kevin", Apellido = "Madarriaga" } };
            _personaRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(personas);

            var personaDTOs = new List<PersonaDTO> { new PersonaDTO { Id = 1, Nombre = "Kevin", Apellido = "Madarriaga" } };
            _mapperMock.Setup(m => m.Map<List<PersonaDTO>>(personas)).Returns(personaDTOs);

            // Act (Actuar)
            var resultado = await _personaService.GetAll();

            // Assert (Afirmar)
            Assert.AreEqual(1, resultado.Count);
            Assert.AreEqual("Kevin", resultado[0].Nombre);
        }

        [TestMethod]
        // Prueba para validar la creación de una nueva persona
        public async Task CrearPersona_DeberiaAgregarPersonaExitosamente()
        {
            // Arrange
            var crearPersonaDTO = new CreatePersonaDTO { Nombre = "Kevin", Apellido = "Madarriaga", TipoDePersona = "paciente" };
            var persona = new Persona { Id = 1, Nombre = "Kevin", Apellido = "Madarriaga", TipoDePersona = "paciente" };
            var personaDTO = new PersonaDTO { Id = 1, Nombre = "Kevin", Apellido = "Madarriaga", TipoDePersona = "paciente" };

            _mapperMock.Setup(m => m.Map<Persona>(crearPersonaDTO)).Returns(persona);
            _mapperMock.Setup(m => m.Map<PersonaDTO>(persona)).Returns(personaDTO);
            _personaRepositoryMock.Setup(repo => repo.Add(persona)).ReturnsAsync(persona);

            // Act
            var resultado = await _personaService.Create(crearPersonaDTO);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual("Kevin", resultado.Nombre);
            Assert.AreEqual("Madarriaga", resultado.Apellido);
            Assert.AreEqual("paciente", resultado.TipoDePersona);
        }


        // Prueba para verificar que se obtiene una persona por ID
        [TestMethod]
        public async Task ObtenerPersonaPorId_DeberiaRetornarPersonaDTO()
        {
            // Arrange
            var persona = new Persona { Id = 1, Nombre = "Kevin", Apellido = "Madarriaga" };
            _personaRepositoryMock.Setup(repo => repo.GetById(1)).ReturnsAsync(persona);

            var personaDTO = new PersonaDTO { Id = 1, Nombre = "Kevin", Apellido = "Madarriaga" };
            _mapperMock.Setup(m => m.Map<PersonaDTO>(persona)).Returns(personaDTO);

            // Act
            var resultado = await _personaService.GetById(1);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(1, resultado.Id);
        }
    }
}
