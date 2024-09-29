using MicroServicioCitas.Domain.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace MicroServicioCitas.Tests
{
    [TestClass]
    public class CitaTests
    {
        [TestMethod]
        public void Cita_Valida_NoDeberiaGenerarErroresDeValidacion()
        {
            // Arrange
            var cita = new Cita
            {
                Id = 1,
                Fecha = DateTime.Now,
                Lugar = "Consulta Médica",
                PacienteId = 1,
                MedicoId = 1,
                Estado = "Pendiente"
            };

            // Act
            var validationResults = ValidateCita(cita);

            // Assert
            Assert.IsTrue(validationResults.Count == 0, "La cita no debería tener errores de validación.");
        }

        [TestMethod]
        public void Cita_SinLugar_DeberiaGenerarErrorDeValidacion()
        {
            // Arrange
            var cita = new Cita
            {
                Id = 1,
                Fecha = DateTime.Now,
                PacienteId = 1,
                MedicoId = 1,
                Estado = "Pendiente"
            };

            // Act
            var validationResults = ValidateCita(cita);

            // Assert
            Assert.IsTrue(validationResults.Count == 1 && validationResults[0].ErrorMessage == "El lugar es requerido.", "Se esperaba un error de validación por la falta de lugar.");
        }



        private IList<ValidationResult> ValidateCita(Cita cita)
        {
            var context = new ValidationContext(cita, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(cita, context, results, validateAllProperties: true);
            return results;
        }
    }
}
