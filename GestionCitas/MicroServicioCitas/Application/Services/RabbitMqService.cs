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
            var factory = new ConnectionFactory() { HostName = "localhost" }; // Configura tu host
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declara el intercambio si es necesario
            _channel.ExchangeDeclare(exchange: "recetaExchange", type: "direct");
        }

        public async Task SendRecetaRequest(int citaId)
        {
            var recetaData = new { CitaId = citaId };
            var message = JsonConvert.SerializeObject(recetaData);
            var body = Encoding.UTF8.GetBytes(message);

            // Publica el mensaje
            _channel.BasicPublish(exchange: "recetaExchange",
                                  routingKey: "recetaRoutingKey",
                                  basicProperties: null,
                                  body: body);
        }

        // Cierre de conexión
        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}
