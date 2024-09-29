using Microsoft.VisualStudio.TestTools.UnitTesting;
using MicroServicioPersonas.Application.DTOs;

namespace MicroServicioPersonas.Tests.Application.DTOs
{
    [TestClass]
    public class CreatePersonaDTOTests
    {
        // Prueba para validar que CreatePersonaDTO contiene las propiedades necesarias
        [TestMethod]
        public void CreatePersonaDTO_DeberiaTenerPropiedadesRequeridas()
        {
            // Arrange
            var dto = new CreatePersonaDTO
            {
                Nombre = "Kevin",
                Apellido = "Madarriaga"
            };

            // Act y Assert
            Assert.AreEqual("Kevin", dto.Nombre);
            Assert.AreEqual("Madarriaga", dto.Apellido);
        }
    }
}
