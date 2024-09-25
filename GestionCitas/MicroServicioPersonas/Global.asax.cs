using MicroServicioPersonas.Presentation.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

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

            // Configuraci�n Unity - Llamar a la extensi�n para registrar los servicios
            UnityConfig.RegisterComponents();

            // Registrar el filtro de excepci�n
            GlobalConfiguration.Configuration.Filters.Add(new CustomExceptionFilter());
        }
    }
}
