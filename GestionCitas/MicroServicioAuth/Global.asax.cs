using MicroServicioAuth.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace MicroServicioAuth
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Configuraci�n Unity - Llamar a la extensi�n para registrar los servicios
            UnityConfig.RegisterComponents();

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
