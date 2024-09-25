using MicroServicioPersonas.Presentation.Filters;
using System.Web.Http;

namespace MicroServicioPersonas
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            //UnityContainer container = new UnityContainer();

            // Registrar el repositorio y el servicio
            //container.RegisterType<IPersonaRepository, PersonaRepository>(new SingletonLifetimeManager());
            //container.RegisterType<IPersonaService, PersonaService>(new SingletonLifetimeManager());

            // Configuración Unity - Llamar a la extensión para registrar los servicios
            UnityConfig.RegisterComponents();

            // Registrar el filtro de excepción
            GlobalConfiguration.Configuration.Filters.Add(new CustomExceptionFilter());
        }
    }
}
