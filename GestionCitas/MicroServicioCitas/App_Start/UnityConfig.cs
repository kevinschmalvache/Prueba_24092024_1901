using AutoMapper;
using MicroServicioCitas.Application.Interfaces;
using MicroServicioCitas.Application.Mapping.AutoMapperProfiles;
using MicroServicioCitas.Application.Sender;
using MicroServicioCitas.Application.Services;
using MicroServicioCitas.Domain.Interfaces;
using MicroServicioCitas.Domain.Services;
using MicroServicioCitas.Infraestructure.Data;
using MicroServicioCitas.Infraestructure.Repositories;
using MicroServicioCitas.Infrastructure.Configurations;
using MicroServicioPersonas.Domain.Interfaces;
using RestSharp;
using System.Web.Http;
using Unity;
using Unity.Injection;
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
            container.RegisterType<ICitaDomainService, CitaDomainService>(new SingletonLifetimeManager());

            // Registrar IPersonaApi y su implementación
            container.RegisterType<IPersonaApi>(new InjectionFactory(c =>new PersonaApiService("https://localhost:44399/"))); // Cambia esto por tu URL base api personas


            // Registrar RestClient
            container.RegisterType<IRestClient, RestClient>(new SingletonLifetimeManager());
            container.RegisterType<RestClient>(new ContainerControlledLifetimeManager());

            // Configuración de AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CitaProfile>();
            });
            var mapper = config.CreateMapper();

            // Registrar IMapper en Unity
            container.RegisterInstance<IMapper>(mapper);


            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}