using MicroServicioAuth.App_Start;
using Swashbuckle.Application;
using System.Web.Http;

namespace MicroServicioAuth
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //var config = GlobalConfiguration.Configuration;

            //config.EnableSwagger(c =>
            //{
            //    //c.SingleApiVersion("v1", "MicroServicioAuth");
            //})
            //.EnableSwaggerUi(c =>
            //{
            //    c.EnableApiKeySupport("Authorization", "header");
            //});

            // Configuración Unity - Llamar a la extensión para registrar los servicios
            UnityConfig.RegisterComponents();

            // Rutas de Web API
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
