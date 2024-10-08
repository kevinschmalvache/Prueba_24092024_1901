using AutoMapper;
using MicroServicioRecetas.Application.Consumers; // Aseg�rate de importar el namespace correcto
using MicroServicioRecetas.Application.Mapping.AutoMapperProfiles;
using MicroServicioRecetas.Infraestructure.Data;
using MicroServicioRecetas.Infrastructure.Configurations;
using MicroServicioRecetas.Infrastructure.Repositories;
using MicroServicioRecetas.Presentation.Filters;
using RabbitMQ.Client;
using System.Web.Http;

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

            // Registrar el filtro de excepci�n
            GlobalConfiguration.Configuration.Filters.Add(new CustomExceptionFilter());

            // Configuraci�n de AutoMapper
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RecetaProfile>();
            });
            IMapper mapper = config.CreateMapper();

            // Iniciar el consumidor de RabbitMQ
            StartRabbitMqConsumer(new RabbitMqConfig());
        }

        private void StartRabbitMqConsumer(RabbitMqConfig config)
        {
            // Crear la conexi�n a RabbitMQ
            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = config.HostName,
                Port = config.Port,
                UserName = config.UserName,
                Password = config.Password
            };

            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();

            // Crear una instancia del contexto de recetas
            var recetasContext = new RecetasContext(); // Aseg�rate de que esto sea correcto seg�n tu implementaci�n

            // Inicializar el repositorio de recetas
            var recetaRepository = new RecetaRepository(recetasContext); // Pasar el contexto aqu�

            // Crear e iniciar el consumidor
            _recetaConsumer = new RecetaConsumer(_channel, recetaRepository);
            _recetaConsumer.StartConsuming();
        }

        protected void Application_End()
        {
            // Cerrar el canal y la conexi�n cuando la aplicaci�n termine
            if (_channel != null) _channel.Close();
            if (_connection != null) _connection.Close();
        }
    }
}
