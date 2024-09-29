using AutoMapper;
using MicroServicioPersonas.Aplication.Interfaces;
using MicroServicioPersonas.Aplication.Services;
using MicroServicioPersonas.Application.Mapping.AutoMapperProfiles;
using MicroServicioPersonas.Domain.Interfaces;
using MicroServicioPersonas.Domain.Services;
using MicroServicioPersonas.Infraestructure.Data;
using MicroServicioPersonas.Infraestructure.Repositories;
using System.Web.Http;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;

namespace MicroServicioPersonas
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<IPersonaRepository, PersonaRepository>(new SingletonLifetimeManager());
            container.RegisterType<IPersonaService, PersonaService>(new SingletonLifetimeManager());
            container.RegisterType<PersonasContext>();
            container.RegisterType<PersonaDomainService>(new SingletonLifetimeManager());
            
            // Configuración de AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PersonaProfile>();
            });
            var mapper = config.CreateMapper();

            // Registrar IMapper en Unity
            container.RegisterInstance<IMapper>(mapper);

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}