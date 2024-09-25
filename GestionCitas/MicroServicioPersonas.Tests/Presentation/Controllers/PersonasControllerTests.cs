using MicroServicioPersonas.Aplication.Interfaces;
using MicroServicioPersonas.Domain.Models;
using MicroServicioPersonas.Presentation.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace MicroServicioPersonas.Tests.Presentation.Controllers
{
    [TestClass]
    public class PersonasControllerTests
    {
        private PersonasController _controller;
        private Mock<IPersonaService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            // Inicializa el Mock del servicio IPersonaService
            _mockService = new Mock<IPersonaService>();
            // Crea una instancia del controlador PersonasController inyectando el servicio mockeado
            _controller = new PersonasController(_mockService.Object);
        }

        [TestMethod]
        public async Task GetPersonas_ReturnsOkResult_WithListOfPersonas()
        {
            // Arrange: Prepara una lista de personas para devolver
            var personas = new List<Persona>
            {
                new Persona { Id = 1, Nombre = "Juan", Apellido = "Pérez" },
                new Persona { Id = 2, Nombre = "Ana", Apellido = "García" }
            };
            _mockService.Setup(service => service.GetAll()).ReturnsAsync(personas);

            // Act: Llama al método GetPersonas
            var result = await _controller.GetPersonas();

            // Assert: Verifica que el resultado sea OK y que contenga la lista de personas
            var okResult = result as OkNegotiatedContentResult<List<Persona>>;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(personas.Count, okResult.Content.Count);
        }

        [TestMethod]
        public async Task GetPersona_ValidId_ReturnsOkResult_WithPersona()
        {
            // Arrange: Prepara una persona específica para devolver
            var persona = new Persona { Id = 1, Nombre = "Juan", Apellido = "Pérez" };
            _mockService.Setup(service => service.GetById(1)).ReturnsAsync(persona);

            // Act: Llama al método GetPersona
            var result = await _controller.GetPersona(1);

            // Assert: Verifica que el resultado sea OK y que contenga la persona
            var okResult = result as OkNegotiatedContentResult<Persona>;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(persona.Id, okResult.Content.Id);
        }

        [TestMethod]
        public async Task CreatePersona_ValidPersona_ReturnsCreatedAtRouteResult()
        {
            // Arrange: Prepara una nueva persona para crear
            var nuevaPersona = new Persona { Nombre = "Ana", Apellido = "García" };
            var creadaPersona = new Persona { Id = 3, Nombre = "Ana", Apellido = "García" };
            _mockService.Setup(service => service.Create(nuevaPersona)).ReturnsAsync(creadaPersona);

            // Act: Llama al método CreatePersona
            var result = await _controller.CreatePersona(nuevaPersona);

            // Assert: Verifica que el resultado sea CreatedAtRoute
            var createdResult = result as CreatedAtRouteNegotiatedContentResult<Persona>;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(creadaPersona.Id, createdResult.Content.Id);
        }

        [TestMethod]
        public async Task UpdatePersona_ValidId_ReturnsOkResult_WithUpdatedPersona()
        {
            // Arrange: Prepara una persona para actualizar
            var personaActualizada = new Persona { Id = 1, Nombre = "Juan", Apellido = "Pérez" };
            _mockService.Setup(service => service.Update(1, personaActualizada)).ReturnsAsync(personaActualizada);

            // Act: Llama al método UpdatePersona
            var result = await _controller.UpdatePersona(1, personaActualizada);

            // Assert: Verifica que el resultado sea OK y que contenga la persona actualizada
            var okResult = result as OkNegotiatedContentResult<Persona>;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(personaActualizada.Id, okResult.Content.Id);
        }

        [TestMethod]
        public async Task DeletePersona_ValidId_ReturnsNoContentResult()
        {
            // Arrange: Prepara el Id de la persona a eliminar
            int personaId = 1;
            _mockService.Setup(service => service.Delete(personaId)).Returns(Task.CompletedTask);

            // Act: Llama al método DeletePersona
            var result = await _controller.DeletePersona(personaId);

            // Assert: Verifica que el resultado sea NoContent
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            var statusCodeResult = result as StatusCodeResult;
            Assert.AreEqual(HttpStatusCode.NoContent, statusCodeResult.StatusCode);
        }
    }
}
