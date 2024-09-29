using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using AutoMapper;
using MicroServicioPersonas.Application.DTOs;
using MicroServicioPersonas.Presentation.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using MicroServicioPersonas.Aplication.Interfaces;
using System.Net;

namespace MicroServicioPersonas.Tests.Presentation.Controllers
{
    [TestClass]
    public class PersonasControllerTests
    {
        private Mock<IPersonaService> _personaServiceMock;
        private Mock<IMapper> _mapperMock;
        private PersonasController _controller;

        // Configuración inicial que se ejecuta antes de cada prueba
        [TestInitialize]
        public void Configuracion()
        {
            _personaServiceMock = new Mock<IPersonaService>();
            _mapperMock = new Mock<IMapper>();
            _controller = new PersonasController(_personaServiceMock.Object);
        }

        // Prueba para verificar que se retorna la lista de personas correctamente
        [TestMethod]
        public async Task ObtenerTodasLasPersonas_DeberiaRetornarOkConListaDePersonas()
        {
            // Arrange (Preparar)
            var personasDTO = new List<PersonaDTO>
            {
                new PersonaDTO { Id = 1, Nombre = "Kevin", Apellido = "Madarriaga" }
            };
            _personaServiceMock.Setup(s => s.GetAll()).ReturnsAsync(personasDTO);

            // Act (Actuar)
            var resultado = await _controller.GetPersonas() as OkNegotiatedContentResult<List<PersonaDTO>>;

            // Assert (Afirmar)
            Assert.IsNotNull(resultado); // Verifica que el resultado no sea nulo
            Assert.AreEqual(1, resultado.Content.Count); // Verifica que la lista tenga un elemento
            Assert.AreEqual("Kevin", resultado.Content[0].Nombre); // Verifica que el nombre sea correcto
        }

        // Prueba para verificar que se obtiene una persona por ID
        [TestMethod]
        public async Task ObtenerPersonaPorId_DeberiaRetornarOkConPersonaDTO()
        {
            // Arrange
            var personaDTO = new PersonaDTO { Id = 1, Nombre = "Kevin", Apellido = "Madarriaga" };
            _personaServiceMock.Setup(s => s.GetById(1)).ReturnsAsync(personaDTO);

            // Act
            var resultado = await _controller.GetPersona(1) as OkNegotiatedContentResult<PersonaDTO>;

            // Assert
            Assert.IsNotNull(resultado); // Verifica que el resultado no sea nulo
            Assert.AreEqual(1, resultado.Content.Id); // Verifica que el ID sea correcto
            Assert.AreEqual("Kevin", resultado.Content.Nombre); // Verifica que el nombre sea correcto
        }

        // Prueba para validar la creación de una nueva persona
        [TestMethod]
        public async Task CrearPersona_DeberiaRetornarCreatedConPersonaDTO()
        {
            // Arrange
            var crearPersonaDTO = new CreatePersonaDTO { Nombre = "Kevin", Apellido = "Madarriaga" };
            var personaDTO = new PersonaDTO { Id = 1, Nombre = "Kevin", Apellido = "Madarriaga" };
            _personaServiceMock.Setup(s => s.Create(crearPersonaDTO)).ReturnsAsync(personaDTO);

            // Act
            var resultado = await _controller.CreatePersona(crearPersonaDTO) as CreatedAtRouteNegotiatedContentResult<PersonaDTO>;

            // Assert
            Assert.IsNotNull(resultado); // Verifica que el resultado no sea nulo
            Assert.AreEqual("GetPersona", resultado.RouteName); // Verifica que el nombre de la ruta sea correcto
            Assert.AreEqual(1, resultado.Content.Id); // Verifica que el ID sea correcto
        }


        // Prueba para validar la actualización de una persona
        [TestMethod]
        public async Task ActualizarPersona_DeberiaRetornarOk()
        {
            // Arrange
            var actualizarPersonaDTO = new UpdatePersonaDTO { Nombre = "Kevin Actualizado", Apellido = "Madarriaga" };
            var personaDTO = new PersonaDTO { Id = 1, Nombre = "Kevin Actualizado", Apellido = "Madarriaga" };

            // Configurar el mock para que retorne la persona actualizada
            _personaServiceMock.Setup(s => s.Update(1, actualizarPersonaDTO)).ReturnsAsync(personaDTO);

            // Act
            var resultado = await _controller.UpdatePersona(1, actualizarPersonaDTO) as OkNegotiatedContentResult<PersonaDTO>;

            // Assert
            Assert.IsNotNull(resultado); // Verifica que el resultado no sea nulo
            Assert.AreEqual(personaDTO.Id, resultado.Content.Id); // Verifica que el ID coincida
            Assert.AreEqual(personaDTO.Nombre, resultado.Content.Nombre); // Verifica que el nombre coincida
            Assert.AreEqual(personaDTO.Apellido, resultado.Content.Apellido); // Verifica que el apellido coincida
        }

        [TestMethod]
        public async Task EliminarPersona_DeberiaRetornarNoContent()
        {
            // Arrange
            _personaServiceMock.Setup(s => s.Delete(1)).Returns(Task.CompletedTask);

            // Act
            var resultado = await _controller.DeletePersona(1) as StatusCodeResult;

            // Assert
            Assert.IsNotNull(resultado); // Verifica que el resultado no sea nulo
            Assert.AreEqual(HttpStatusCode.NoContent, resultado.StatusCode); // Verifica que el código de estado sea NoContent (204)
        }

    }
}
