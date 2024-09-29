using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using MicroServicioRecetas.Application.DTOs;
using MicroServicioRecetas.Application.Interfaces;
using MicroServicioRecetas.Presentation.Controllers;
using MicroServicioPersonas.Application.DTOs;

namespace MicroServicioRecetas.Tests.Controllers
{
    [TestClass]
    public class RecetaControllerTests
    {
        private Mock<IRecetaService> _mockRecetaService;
        private RecetaController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRecetaService = new Mock<IRecetaService>();
            _controller = new RecetaController(_mockRecetaService.Object);
        }

        [TestMethod]
        public async Task ObtenerRecetas_RetornaOkResult_ConListaDeRecetas()
        {
            // Arrange
            var recetas = new List<RecetaDTO> { new RecetaDTO { RecetaId = 1, Descripcion = "Receta1" } };
            _mockRecetaService.Setup(service => service.GetRecetas()).ReturnsAsync(recetas);

            // Act
            IHttpActionResult actionResult = await _controller.GetRecetas();
            var contentResult = actionResult as OkNegotiatedContentResult<List<RecetaDTO>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(recetas.Count, contentResult.Content.Count);
        }

        [TestMethod]
        public async Task ObtenerRecetaPorId_RetornaOkResult_ConReceta()
        {
            // Arrange
            var receta = new RecetaDTO { RecetaId = 1, Descripcion = "Receta1" };
            _mockRecetaService.Setup(service => service.GetRecetaById(1)).ReturnsAsync(receta);

            // Act
            IHttpActionResult actionResult = await _controller.GetRecetaById(1);
            var contentResult = actionResult as OkNegotiatedContentResult<RecetaDTO>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(receta.RecetaId, contentResult.Content.RecetaId);
        }

        [TestMethod]
        public async Task CrearReceta_RetornaCreatedAtRouteResult()
        {
            // Arrange
            var recetaDto = new CreateRecetaDTO { Descripcion = "Nueva Receta" };
            var createdReceta = new RecetaDTO { RecetaId = 1, Descripcion = "Nueva Receta" };
            _mockRecetaService.Setup(service => service.AddReceta(recetaDto)).ReturnsAsync(createdReceta);

            // Act
            IHttpActionResult actionResult = await _controller.CreateReceta(recetaDto);
            var createdResult = actionResult as CreatedAtRouteNegotiatedContentResult<RecetaDTO>;

            // Assert
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("GetRecetas", createdResult.RouteName);
            Assert.AreEqual(createdReceta.RecetaId, createdResult.RouteValues["id"]);
        }

        [TestMethod]
        public async Task ActualizarReceta_RetornaOkResult()
        {
            // Arrange
            var recetaDto = new UpdateRecetaDTO { Descripcion = "Receta Actualizada" };
            var updatedReceta = new RecetaDTO { RecetaId = 1, Descripcion = "Receta Actualizada" };
            _mockRecetaService.Setup(service => service.UpdateReceta(1, recetaDto)).ReturnsAsync(updatedReceta);

            // Act
            IHttpActionResult actionResult = await _controller.UpdateReceta(1, recetaDto);
            var contentResult = actionResult as OkNegotiatedContentResult<RecetaDTO>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(updatedReceta.RecetaId, contentResult.Content.RecetaId);
        }

        [TestMethod]
        public async Task EliminarReceta_RetornaOkResult()
        {
            // Arrange
            _mockRecetaService.Setup(service => service.DeleteReceta(1)).Returns(Task.CompletedTask);

            // Act
            IHttpActionResult actionResult = await _controller.DeleteReceta(1);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkResult));
        }

        [TestMethod]
        public async Task ObtenerRecetasPorPacienteId_RetornaOkResult_ConListaDeRecetas()
        {
            // Arrange
            var recetas = new List<RecetaDTO> { new RecetaDTO { RecetaId = 1, Descripcion = "Receta1" } };
            _mockRecetaService.Setup(service => service.GetRecetasByPacienteId(1)).ReturnsAsync(recetas);

            // Act
            IHttpActionResult actionResult = await _controller.GetRecetasByPacienteId(1);
            var contentResult = actionResult as OkNegotiatedContentResult<List<RecetaDTO>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(recetas.Count, contentResult.Content.Count);
        }

        [TestMethod]
        public async Task ActualizarEstado_RetornaNoContentResult_CuandoExitoso()
        {
            // Arrange
            _mockRecetaService.Setup(service => service.UpdateEstadoReceta(1, "NuevoEstado")).ReturnsAsync(true);

            // Act
            IHttpActionResult actionResult = await _controller.UpdateEstado(1, "NuevoEstado");

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(StatusCodeResult));
            var statusCodeResult = actionResult as StatusCodeResult;
            Assert.AreEqual(HttpStatusCode.NoContent, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task ActualizarEstado_RetornaBadRequest_CuandoNoExitoso()
        {
            // Arrange
            _mockRecetaService.Setup(service => service.UpdateEstadoReceta(1, "NuevoEstado")).ReturnsAsync(false);

            // Act
            IHttpActionResult actionResult = await _controller.UpdateEstado(1, "NuevoEstado");

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));
        }
    }
}
