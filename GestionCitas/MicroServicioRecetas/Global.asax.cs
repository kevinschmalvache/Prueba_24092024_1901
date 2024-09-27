using AutoMapper;
using MicroServicioRecetas.Application.Mapping.AutoMapperProfiles;
using MicroServicioRecetas.Presentation.Filters;
using System.Web.Http;

namespace MicroServicioRecetas
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            UnityConfig.RegisterComponents();

            // Registrar el filtro de excepci�n
            GlobalConfiguration.Configuration.Filters.Add(new CustomExceptionFilter());

            // Configuraci�n de AutoMapper
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RecetaProfile>();
            });
            IMapper mapper = config.CreateMapper();
        }
    }
}
