using MicroServicioCitas.Infrastructure.Configurations;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace MicroServicioCitas.Application.Sender
{
    /// <summary>
    /// Clase responsable de enviar mensajes a RabbitMQ.
    /// </summary>
    public class RabbitMqSender : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        /// <summary>
        /// Constructor que inicializa la conexión y el canal de RabbitMQ.
        /// </summary>
        /// <param name="config">Configuración de RabbitMQ.</param>
        public RabbitMqSender(RabbitMqConfig config)
        {
            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = config.HostName,
                Port = config.Port,
                UserName = config.UserName,
                Password = config.Password
            };

            // Crear la conexión y el canal
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: "logExchange", type: "direct");

            // Declarar la cola de logs
            _channel.QueueDeclare(queue: "logQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            // Vincular la cola a un exchange
            _channel.QueueBind(queue: "logQueue", exchange: "logExchange", routingKey: "logRoutingKey");
        }

        /// <summary>
        /// Envía un mensaje a la cola de logs de RabbitMQ.
        /// </summary>
        /// <param name="logMessage">Mensaje de log a enviar.</param>
        /// <returns>Retorna verdadero si el mensaje fue enviado exitosamente; de lo contrario, falso.</returns>
        public async Task<bool> SendMessageAsync(string logMessage)
        {
            try
            {
                byte[] body = Encoding.UTF8.GetBytes(logMessage);

                await Task.Run(() =>
                    _channel.BasicPublish(
                        exchange: "logExchange", // Publica al exchange de logs
                        routingKey: "logRoutingKey", // Routing key para logs
                        basicProperties: null,
                        body: body
                    )
                );

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Cierra el canal y la conexión de RabbitMQ de forma asíncrona.
        /// </summary>
        public async Task CloseAsync()
        {
            await Task.Run(() =>
            {
                _channel.Close();
                _connection.Close();
            });
        }

        /// <summary>
        /// Libera los recursos utilizados por el objeto RabbitMqSender.
        /// </summary>
        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
