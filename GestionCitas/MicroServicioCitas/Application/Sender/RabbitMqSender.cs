﻿using MicroServicioCitas.Infrastructure.Configurations;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace MicroServicioCitas.Application.Sender
{
    public class RabbitMqSender : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

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

            // Declarar la cola de recetas
            //_channel.QueueDeclare(queue: "recetasQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            // Declarar la cola de logs
            _channel.QueueDeclare(queue: "logQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            // Si usas exchange, también debes declararlo (si aún no está declarado)
            //_channel.ExchangeDeclare(exchange: "recetaExchange", type: "direct", durable: false);
            //_channel.ExchangeDeclare(exchange: "logExchange", type: "direct", durable: false);

            _channel.QueueBind(queue: "logQueue", exchange: "logExchange", routingKey: "logRoutingKey");

        }


        // Método para enviar mensajes a la cola de logs
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

        public async Task CloseAsync()
        {
            await Task.Run(() =>
            {
                _channel.Close();
                _connection.Close();
            });
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
