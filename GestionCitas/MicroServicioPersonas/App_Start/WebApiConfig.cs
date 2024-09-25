using MicroServicioPersonas.Presentation.Filters;
using System.Web.Http;

namespace MicroServicioPersonas
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuración y servicios de Web API

            //Habilitamos swagger
            //config.EnableSwagger(c =>
            //{
            //    c.SingleApiVersion("v1", "Tu API");
            //})
            //.EnableSwaggerUi();


            // Rutas de Web API
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Registrar el filtro de excepción
            config.Filters.Add(new CustomExceptionFilter());
        }
    }
}
