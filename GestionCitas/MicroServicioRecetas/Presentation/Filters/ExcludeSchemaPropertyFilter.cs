using MicroServicioPersonas.Application.DTOs;
using Newtonsoft.Json;
using Swashbuckle.Swagger;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Http.Description;

namespace MicroServicioRecetas.Presentation.Filters
{
    public class ExcludeSchemaPropertyFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters != null)
            {
                foreach (var parameter in operation.parameters)
                {
                    if (parameter.schema != null && parameter.schema.properties != null)
                    {
                        // Filtra las propiedades que tienen el atributo JsonIgnore
                        var propertiesToRemove = parameter.schema.properties
                            .Where(prop => ShouldExcludeProperty(parameter.schema.title, prop.Key))
                            .Select(prop => prop.Key)
                            .ToList();

                        foreach (var propertyToRemove in propertiesToRemove)
                        {
                            parameter.schema.properties.Remove(propertyToRemove);
                        }
                    }
                }
            }
        }

        private bool ShouldExcludeProperty(string schemaTitle, string propertyName)
        {
            // Aquí se debe mapear el nombre del esquema al tipo del modelo
            Type modelType = GetModelType(schemaTitle);

            if (modelType != null)
            {
                // Obtiene información de la propiedad del modelo
                var propertyInfo = modelType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

                if (propertyInfo != null)
                {
                    // Verifica si tiene el atributo JsonIgnore
                    return propertyInfo.GetCustomAttributes(typeof(JsonIgnoreAttribute), true).Any();
                }
            }
            return false;
        }

        private Type GetModelType(string schemaTitle)
        {
            // Mapea el título del esquema al tipo del modelo correspondiente
            switch (schemaTitle)
            {
                case "CreateRecetaDTO":
                    return typeof(CreateRecetaDTO);
                // Agregar otros modelos según sea necesario
                default:
                    return null;
            }
        }
    }
}
