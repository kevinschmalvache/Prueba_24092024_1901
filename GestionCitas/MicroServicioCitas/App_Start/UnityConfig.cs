using MicroServicioCitas.Application.Interfaces;
using MicroServicioCitas.Application.Services;
using MicroServicioCitas.Domain.Interfaces;
using MicroServicioCitas.Infraestructure.Data;
using MicroServicioCitas.Infraestructure.Repositories;
using MicroServicioCitas.Infrastructure.Configurations;
using System.Web.Http;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;

namespace MicroServicioCitas
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            // Registrar dependencias
            container.RegisterType<ICitaRepository, CitaRepository>(new SingletonLifetimeManager());
            container.RegisterType<ICitaService, CitaService>(new SingletonLifetimeManager());
            container.RegisterType<CitasContext>();

            container.RegisterType<RabbitMqConfig>(new SingletonLifetimeManager());
            container.RegisterType<RabbitMqSender>(new SingletonLifetimeManager());
            container.RegisterType<IRabbitMqService, RabbitMqService>(new SingletonLifetimeManager());


            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}