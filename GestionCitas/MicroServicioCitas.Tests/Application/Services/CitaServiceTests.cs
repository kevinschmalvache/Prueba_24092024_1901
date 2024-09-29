using AutoMapper;
using MicroServicioCitas.Application.DTOs;
using MicroServicioCitas.Application.Interfaces;
using MicroServicioCitas.Domain.Interfaces;
using MicroServicioCitas.Domain.Models;
using MicroServicioCitas.Application.Services;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MicroServicioCitas.Domain.Services;
using System;
using MicroServicioPersonas.Domain.Interfaces;

namespace MicroServicioCitas.Tests
{
    [TestClass]
    public class CitaServiceTests
    {
        //private Mock<IMapper> _mockMapper;
        //private Mock<ICitaRepository> _mockCitaRepository;
        //private Mock<CitaDomainService> _mockCitaDomainService;
        //private Mock<IRabbitMqService> _mockRabbitMqService;
        //private CitaService _citaService;

        //[TestInitialize]
        //public void SetUp()
        //{
        //    _mockMapper = new Mock<IMapper>();
        //    _mockCitaRepository = new Mock<ICitaRepository>();
        //    _mockCitaDomainService = new Mock<CitaDomainService>();
        //    _mockRabbitMqService = new Mock<IRabbitMqService>();

        //    _citaService = new CitaService(
        //        _mockMapper.Object,
        //        _mockCitaRepository.Object,
        //        _mockCitaDomainService.Object,
        //        _mockRabbitMqService.Object
        //    );
        //}


        private Mock<IMapper> _mockMapper;
        private Mock<ICitaRepository> _mockCitaRepository;
        private Mock<ICitaDomainService> _mockCitaDomainService; // Cambiar a la interfaz
        private Mock<IRabbitMqService> _mockRabbitMqService;
        private CitaService _citaService;

        [TestInitialize]
        public void SetUp()
        {
            _mockMapper = new Mock<IMapper>();
            _mockCitaRepository = new Mock<ICitaRepository>();
            _mockCitaDomainService = new Mock<ICitaDomainService>(); // Cambiar a la interfaz
            _mockRabbitMqService = new Mock<IRabbitMqService>();

            _citaService = new CitaService(
                _mockMapper.Object,
                _mockCitaRepository.Object,
                _mockCitaDomainService.Object,
                _mockRabbitMqService.Object
            );
        }


        [TestMethod]
        public async Task GetAll_ReturnsListOfCitas()
        {
            // Arrange
            var citas = new List<Cita>
            {
                new Cita { Id = 1, Estado = "Pendiente" },
                new Cita { Id = 2, Estado = "Finalizada" }
            };

            _mockCitaRepository.Setup(repo => repo.GetAll()).ReturnsAsync(citas);
            _mockMapper.Setup(m => m.Map<List<CitaDTO>>(It.IsAny<List<Cita>>()))
                .Returns(new List<CitaDTO> { new CitaDTO { Id = 1 }, new CitaDTO { Id = 2 } });

            // Act
            var result = await _citaService.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public async Task GetById_ShouldReturnCitaDTO()
        {
            // Arrange
            var cita = new Cita { Id = 1, PacienteId = 1, MedicoId = 1, Estado = "pendiente" };
            _mockCitaRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(cita);
            _mockMapper.Setup(m => m.Map<CitaDTO>(It.IsAny<Cita>())).Returns(new CitaDTO { Id = 1 });

            // Act
            var result = await _citaService.GetById(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        public async Task Create_CreaCita_RetornaCitaDTO()
        {
            // Arrange
            var createCitaDto = new CreateCitaDTO
            {
                Fecha = DateTime.Now,
                Lugar = "Consulta General",
                PacienteId = 1,
                MedicoId = 1
            };

            var cita = new Cita
            {
                Id = 1,
                Estado = "Pendiente",
                Fecha = createCitaDto.Fecha,
                Lugar = createCitaDto.Lugar,
                PacienteId = createCitaDto.PacienteId,
                MedicoId = createCitaDto.MedicoId
            };

            var citaDto = new CitaDTO
            {
                Id = 1,
                Estado = "Pendiente",
                Fecha = cita.Fecha,
                Lugar = cita.Lugar,
                PacienteId = cita.PacienteId,
                MedicoId = cita.MedicoId
            };

            _mockMapper.Setup(m => m.Map<Cita>(createCitaDto)).Returns(cita);
            _mockCitaDomainService.Setup(service => service.ValidateCita(cita)).Returns(Task.CompletedTask);
            _mockCitaRepository.Setup(repo => repo.Add(cita)).ReturnsAsync(cita);
            _mockMapper.Setup(m => m.Map<CitaDTO>(cita)).Returns(citaDto);

            // Act
            var result = await _citaService.Create(createCitaDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(citaDto.Id, result.Id);
            Assert.AreEqual(citaDto.Estado, result.Estado);
        }

        [TestMethod]
        public async Task Update_ActualizaCita_RetornaCitaDTO()
        {
            // Arrange
            var updateCitaDto = new UpdateCitaDTO
            {
                Fecha = DateTime.Now,
                Lugar = "Consulta General"
            };

            var cita = new Cita
            {
                Id = 1,
                Estado = "Pendiente",
                Fecha = updateCitaDto.Fecha.Value,
                Lugar = updateCitaDto.Lugar,
            };

            var updatedCita = new Cita
            {
                Id = 1,
                Estado = "Pendiente",
                Fecha = updateCitaDto.Fecha.Value,
                Lugar = updateCitaDto.Lugar,
            };

            var citaDto = new CitaDTO
            {
                Id = 1,
                Estado = "Pendiente",
                Fecha = updatedCita.Fecha,
                Lugar = updatedCita.Lugar,
            };

            _mockMapper.Setup(m => m.Map<Cita>(updateCitaDto)).Returns(cita);
            _mockCitaRepository.Setup(repo => repo.Update(It.IsAny<int>(), cita)).ReturnsAsync(updatedCita);
            _mockMapper.Setup(m => m.Map<CitaDTO>(updatedCita)).Returns(citaDto);

            // Act
            var result = await _citaService.Update(1, updateCitaDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(citaDto.Id, result.Id);
            Assert.AreEqual(citaDto.Estado, result.Estado);
        }

        // Agrega más pruebas unitarias para los otros métodos del servicio
    }
}
