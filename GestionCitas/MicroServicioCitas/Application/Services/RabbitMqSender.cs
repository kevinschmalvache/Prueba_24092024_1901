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
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqSender(RabbitMqConfig config)
        {
            var factory = new ConnectionFactory()
            {
                HostName = config.HostName,
                Port = config.Port,
                UserName = config.UserName,
                Password = config.Password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "recetasQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "", routingKey: "recetasQueue", basicProperties: null, body: body);
        }

        public void Close()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}