using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MicroServicioPersonas.Application.DTOs;
using MicroServicioPersonas.Domain.Models;

namespace MicroServicioPersonas.Tests.Application.Mapping
{
    [TestClass]
    public class AutoMapperProfileTests
    {
        private IMapper _mapper;

        // Configuración inicial del mapeo
        [TestInitialize]
        public void Configuracion()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Persona, PersonaDTO>();
                cfg.CreateMap<CreatePersonaDTO, Persona>();
            });

            _mapper = config.CreateMapper();
        }

        // Prueba para validar el mapeo de Persona a PersonaDTO
        [TestMethod]
        public void AutoMapper_DeberiaMapearPersonaAPersonaDTO()
        {
            // Arrange
            var persona = new Persona { Id = 1, Nombre = "Kevin", Apellido = "Madarriaga" };

            // Act
            var personaDTO = _mapper.Map<PersonaDTO>(persona);

            // Assert
            Assert.AreEqual(persona.Id, personaDTO.Id);
            Assert.AreEqual(persona.Nombre, personaDTO.Nombre);
        }

        // Prueba para validar el mapeo de CreatePersonaDTO a Persona
        [TestMethod]
        public void AutoMapper_DeberiaMapearCreatePersonaDTOAPersona()
        {
            // Arrange
            var createPersonaDTO = new CreatePersonaDTO { Nombre = "Kevin", Apellido = "Madarriaga" };

            // Act
            var persona = _mapper.Map<Persona>(createPersonaDTO);

            // Assert
            Assert.AreEqual(createPersonaDTO.Nombre, persona.Nombre);
            Assert.AreEqual(createPersonaDTO.Apellido, persona.Apellido);
        }
    }
}
