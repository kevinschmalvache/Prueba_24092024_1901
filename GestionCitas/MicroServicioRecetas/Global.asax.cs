using AutoMapper;
using MicroServicioRecetas.Application.Mapping.AutoMapperProfiles;
using MicroServicioRecetas.Application.Consumers; // Asegúrate de importar el namespace correcto
using MicroServicioRecetas.Presentation.Filters;
using RabbitMQ.Client;
using System.Web.Http;
using MicroServicioRecetas.Infrastructure.Repositories;
using MicroServicioRecetas.Infraestructure.Data;
using System.Data.Common;

namespace MicroServicioRecetas
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private IConnection _connection;
        private IModel _channel;
        private static RecetaConsumer _recetaConsumer;

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            UnityConfig.RegisterComponents();

            // Registrar el filtro de excepción
            GlobalConfiguration.Configuration.Filters.Add(new CustomExceptionFilter());

            // Configuración de AutoMapper
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RecetaProfile>();
            });
            IMapper mapper = config.CreateMapper();

            // Iniciar el consumidor de RabbitMQ
            StartRabbitMqConsumer();
        }

        private void StartRabbitMqConsumer()
        {
            // Crear la conexión a RabbitMQ
            var factory = new ConnectionFactory() { HostName = "localhost" }; // Asegúrate de que esto concuerde con tu configuración
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();

            // Crear una instancia del contexto de recetas
            var recetasContext = new RecetasContext(); // Asegúrate de que esto sea correcto según tu implementación

            // Inicializar el repositorio de recetas
            var recetaRepository = new RecetaRepository(recetasContext); // Pasar el contexto aquí

            // Crear e iniciar el consumidor
            _recetaConsumer = new RecetaConsumer(_channel, recetaRepository);
            _recetaConsumer.StartConsuming();
        }

        protected void Application_End()
        {
            // Cerrar el canal y la conexión cuando la aplicación termine
            if (_channel != null) _channel.Close();
            if (_connection != null) _connection.Close();
        }
    }
}
