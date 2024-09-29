using System;
using System.Web.Http;
using Unity.WebApi;
using MicroServicioLogin.Application.Interfaces; // Ajusta según tu estructura de carpetas
using MicroServicioLogin.Application.Services;
using Unity;
using MicroServicioLogin.Domain.Interfaces;
using MicroServicioLogin.Infrastructure.Repositories;   // Ajusta según tu estructura de carpetas

namespace MicroServicioAuth.App_Start
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // Registra tus interfaces y servicios
            container.RegisterType<IAuthService, AuthService>();
            container.RegisterType<IUsuarioRepository, UsuarioRepository>();


            // Configura Web API para usar el contenedor de Unity
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }

}