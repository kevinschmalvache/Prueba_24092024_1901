using AutoMapper;
using MicroServicioPersonas.Domain.Interfaces;
using MicroServicioRecetas.Application.Interfaces;
using MicroServicioRecetas.Application.Mapping.AutoMapperProfiles;
using MicroServicioRecetas.Application.Services;
using MicroServicioRecetas.Domain.Interfaces;
using MicroServicioRecetas.Domain.Services;
using MicroServicioRecetas.Infrastructure.Repositories;
using MicroServicioRecetas.Presentation.Controllers;
using System.Web.Http;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;

namespace MicroServicioRecetas
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            UnityContainer container = new UnityContainer();

            // Registra las dependencias
            container.RegisterType<IRecetaRepository, RecetaRepository>(new SingletonLifetimeManager());
            container.RegisterType<IRecetaService, RecetaService>(new SingletonLifetimeManager());
            container.RegisterType<IRecetaDomainService, RecetaDomainService>(new SingletonLifetimeManager());
            // También registrar el controlador
            container.RegisterType<RecetaController>(new TransientLifetimeManager());


            // Configuración de AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RecetaProfile>();
            });
            var mapper = config.CreateMapper();

            // Registrar IMapper en Unity
            container.RegisterInstance<IMapper>(mapper);


            // Configurar Unity como el resolvedor predeterminado de dependencias
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);

        }
    }
}