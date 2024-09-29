using MicroServicioCitas.Application.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Tasks;

namespace MicroServicioCitas.Application.Services
{
    /// <summary>
    /// Servicio para interactuar con RabbitMQ y enviar solicitudes de recetas.
    /// </summary>
    public class RabbitMqService : IRabbitMqService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        /// <summary>
        /// Constructor de la clase RabbitMqService.
        /// Configura la conexión y el canal de RabbitMQ, y declara el intercambio y la cola.
        /// </summary>
        public RabbitMqService()
        {
            // Configura el host de RabbitMQ
            ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declara el intercambio si es necesario
            _channel.ExchangeDeclare(exchange: "recetaExchange", type: "direct");

            // Declara la cola 'recetasQueue'
            _channel.QueueDeclare(
                queue: "recetasQueue",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            // Vincula (bind) la cola 'recetasQueue' con el exchange 'recetaExchange'
            _channel.QueueBind(queue: "recetasQueue", exchange: "recetaExchange", routingKey: "recetaRoutingKey");
        }

        /// <summary>
        /// Envía una solicitud de receta a través de RabbitMQ.
        /// </summary>
        /// <param name="recetaData">Datos de la receta a enviar.</param>
        /// <returns>Tarea que representa la operación asíncrona.</returns>
        public async Task SendRecetaRequest(object recetaData)
        {
            // Serializa los datos de la receta a formato JSON
            var message = JsonConvert.SerializeObject(recetaData);

            // Convierte el mensaje JSON a un array de bytes
            var body = Encoding.UTF8.GetBytes(message);

            // Publica el mensaje en el exchange correspondiente
            _channel.BasicPublish(
                exchange: "recetaExchange",          // Nombre del exchange
                routingKey: "recetaRoutingKey",      // Routing key de la cola de recetas
                basicProperties: null,               // No necesitas propiedades adicionales aquí
                body: body                           // El mensaje que se envía
            );

            // Puedes implementar lógica adicional después de enviar el mensaje si es necesario
            await Task.CompletedTask;
        }

        /// <summary>
        /// Libera los recursos utilizados por RabbitMqService.
        /// </summary>
        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}
