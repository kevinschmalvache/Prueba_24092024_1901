using MicroServicioCitas.Presentation.Filters;
using MicroServicioCitas;
using System.Web.Http;

namespace MicroServicioCitas
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

            //var recipeService = new RecipeService(); // Inicializa tu servicio de recetas
            //var recipeCreationConsumer = new RecipeCreationConsumer(recipeService);
        }
    }
}
