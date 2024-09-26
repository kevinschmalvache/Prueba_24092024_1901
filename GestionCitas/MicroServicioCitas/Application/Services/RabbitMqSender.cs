using MicroServicioCitas.Infrastructure.Configurations;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace MicroServicioCitas.Application.Services
{
    public class RabbitMqSender : IDisposable
    {
        // Campos privados para la conexión y el canal de RabbitMQ
        private readonly IConnection _connection;
        private readonly IModel _channel;

        // Constructor que recibe la configuración de RabbitMQ
        public RabbitMqSender(RabbitMqConfig config)
        {
            // Crear una fábrica de conexiones con la configuración proporcionada
            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = config.HostName,
                Port = config.Port,
                UserName = config.UserName,
                Password = config.Password
            };

            // Crear una conexión y un canal
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declarar una cola llamada "recetasQueue"
            _channel.QueueDeclare(queue: "recetasQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        // Método asíncrono para enviar un mensaje a la cola
        public async Task<bool> SendMessageAsync(string message)
        {
            try
            {
                // Convertir el mensaje a un arreglo de bytes
                byte[] body = Encoding.UTF8.GetBytes(message);

                // Publicar el mensaje en la cola "recetasQueue"
                await Task.Run(() =>
                    _channel.BasicPublish(exchange: "", routingKey: "recetasQueue", basicProperties: null, body: body)
                );

                return true; // Indicar que el mensaje se envió correctamente
            }
            catch (Exception exc)
            {
                return false; // Indicar que hubo un error al enviar el mensaje
            }
        }

        // Método asíncrono para cerrar la conexión y el canal
        public async Task CloseAsync()
        {
            await Task.Run(() =>
            {
                _channel.Close();
                _connection.Close();
            });
        }

        // Implementación de IDisposable para liberar recursos
        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
