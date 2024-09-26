using MicroServicioCitas.Infrastructure.Configurations;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MicroServicioCitas.Application.Services
{
    public class RabbitMqSender
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

        // Método para enviar un mensaje a la cola
        public void SendMessage(string message)
        {
            // Convertir el mensaje a un arreglo de bytes
            byte[] body = Encoding.UTF8.GetBytes(message);

            // Publicar el mensaje en la cola "recetasQueue"
            _channel.BasicPublish(exchange: "", routingKey: "recetasQueue", basicProperties: null, body: body);
        }

        // Método para cerrar la conexión y el canal
        public void Close()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
