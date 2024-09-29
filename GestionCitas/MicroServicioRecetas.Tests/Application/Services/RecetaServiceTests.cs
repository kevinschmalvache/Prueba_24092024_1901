using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MicroServicioRecetas.Application.DTOs;
using MicroServicioRecetas.Application.Interfaces;
using MicroServicioRecetas.Application.Services;
using MicroServicioRecetas.Domain.Interfaces;
using MicroServicioRecetas.Domain.Models;
using MicroServicioRecetas.Domain.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MicroServicioPersonas.Application.DTOs;
using MicroServicioPersonas.Domain.Interfaces;

namespace MicroServicioRecetas.Tests
{
    [TestClass]
    public class RecetaServiceTests
    {
        private Mock<IRecetaRepository> _recetaRepositoryMock;
        private Mock<IRecetaDomainService> _recetaDomainServiceMock;
        private Mock<IMapper> _mapperMock;
        private RecetaService _recetaService;

        [TestInitialize]
        public void Configurar()
        {
            _recetaRepositoryMock = new Mock<IRecetaRepository>();
            _recetaDomainServiceMock = new Mock<IRecetaDomainService>();
            _mapperMock = new Mock<IMapper>();
            _recetaService = new RecetaService(_recetaRepositoryMock.Object, _recetaDomainServiceMock.Object, _mapperMock.Object);
        }

        [TestMethod]
        public async Task ObtenerRecetas_DeberiaRetornarRecetasMapeadas()
        {
            // Arrange
            var recetas = new List<Receta> { new Receta { RecetaId =  1, Descripcion = "Receta1" } };
            var recetaDTOs = new List<RecetaDTO> { new RecetaDTO { RecetaId =  1, Descripcion = "Receta1" } };

            _recetaRepositoryMock.Setup(repo => repo.GetRecetasAsync()).ReturnsAsync(recetas);
            _mapperMock.Setup(mapper => mapper.Map<List<RecetaDTO>>(recetas)).Returns(recetaDTOs);

            // Act
            var resultado = await _recetaService.GetRecetas();

            // Assert
            Assert.AreEqual(recetaDTOs, resultado);
        }

        [TestMethod]
        public async Task ObtenerRecetaPorId_DeberiaRetornarRecetaMapeada()
        {
            // Arrange
            var receta = new Receta { RecetaId =  1, Descripcion = "Receta1" };
            var recetaDTO = new RecetaDTO { RecetaId =  1, Descripcion = "Receta1" };

            _recetaRepositoryMock.Setup(repo => repo.GetRecetaByIdAsync(1)).ReturnsAsync(receta);
            _recetaDomainServiceMock.Setup(service => service.ExistReceta(receta));
            _mapperMock.Setup(mapper => mapper.Map<RecetaDTO>(receta)).Returns(recetaDTO);

            // Act
            var resultado = await _recetaService.GetRecetaById(1);

            // Assert
            Assert.AreEqual(recetaDTO, resultado);
        }

        [TestMethod]
        public async Task AgregarReceta_DeberiaRetornarRecetaAgregada()
        {
            // Arrange
            var createRecetaDTO = new CreateRecetaDTO { Descripcion = "Nueva Receta" };
            var receta = new Receta { RecetaId =  1, Descripcion = "Nueva Receta", FechaCreacion = DateTime.Now };
            var recetaDTO = new RecetaDTO { RecetaId =  1, Descripcion = "Nueva Receta" };

            _mapperMock.Setup(mapper => mapper.Map<Receta>(createRecetaDTO)).Returns(receta);
            _recetaDomainServiceMock.Setup(service => service.ValidateReceta(receta));
            _recetaRepositoryMock.Setup(repo => repo.AddRecetaAsync(receta)).ReturnsAsync(receta);
            _mapperMock.Setup(mapper => mapper.Map<RecetaDTO>(receta)).Returns(recetaDTO);

            // Act
            var resultado = await _recetaService.AddReceta(createRecetaDTO);

            // Assert
            Assert.AreEqual(recetaDTO, resultado);
        }

    }
}
