using MicroServicioCitas.Application.DTOs;
using MicroServicioCitas.Application.Interfaces;
using MicroServicioCitas.Application.Sender;
using MicroServicioCitas.Infrastructure.Configurations;
using MicroServicioCitas.Presentation.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace MicroServicioCitas.Tests
{
    [TestClass]
    public class CitasControllerTests
    {
        private Mock<ICitaService> _mockCitaService;
        private RabbitMqSender _rabbitMqSender;
        private CitasController _controller;

        [TestInitialize]
        public void Setup()
        {
            // Crear el mock de ICitaService
            _mockCitaService = new Mock<ICitaService>();

            // Configurar RabbitMqSender con un RabbitMqConfig simulado
            var rabbitMqConfig = new RabbitMqConfig
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
            _rabbitMqSender = new RabbitMqSender(rabbitMqConfig);

            // Crear el controlador con los mocks
            _controller = new CitasController(_mockCitaService.Object, _rabbitMqSender);
        }

        [TestMethod]
        public async Task UpdateCita_ConIdValido_RegresaCitaActualizada()
        {
            // Arrange
            var actualizarCitaDto = new UpdateCitaDTO
            {
                Fecha = DateTime.Now, // O null si quieres omitirlo
                Lugar = "Consulta Médica"
            };

            var citaActualizada = new CitaDTO { Id = 1, PacienteId = 1, Fecha = actualizarCitaDto.Fecha.Value, Lugar = actualizarCitaDto.Lugar };
            _mockCitaService.Setup(service => service.Update(1, actualizarCitaDto)).ReturnsAsync(citaActualizada);

            // Act
            var resultado = await _controller.UpdateCita(1, actualizarCitaDto) as OkNegotiatedContentResult<CitaDTO>;

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(citaActualizada.Id, resultado.Content.Id);
            Assert.AreEqual(citaActualizada.Lugar, resultado.Content.Lugar);
            Assert.AreEqual(citaActualizada.Fecha, resultado.Content.Fecha);
        }
    }
}
