using Microsoft.VisualStudio.TestTools.UnitTesting;
using MicroServicioPersonas.Domain.Models;

namespace MicroServicioPersonas.Tests.Domain.Models
{
    [TestClass]
    public class PersonaModelTests
    {
        // Prueba para verificar que la entidad Persona tiene las propiedades requeridas
        [TestMethod]
        public void Persona_DeberiaTenerPropiedadesRequeridas()
        {
            // Arrange
            var persona = new Persona
            {
                Id = 1,
                Nombre = "Kevin",
                Apellido = "Madarriaga"
            };

            // Act y Assert
            Assert.AreEqual(1, persona.Id);
            Assert.AreEqual("Kevin", persona.Nombre);
            Assert.AreEqual("Madarriaga", persona.Apellido);
        }
    }
}
