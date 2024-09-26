using MicroServicioCitas.Application.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace MicroServicioCitas.Application.Services
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqService()
        {
            //Configuro el host de rabbitMq
            ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declara el intercambio si es necesario
            _channel.ExchangeDeclare(exchange: "recetaExchange", type: "direct");
        }

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

        // Cierre de conexión
        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}
