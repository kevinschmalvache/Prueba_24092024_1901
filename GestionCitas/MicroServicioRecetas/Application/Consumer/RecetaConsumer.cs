using MicroServicioPersonas.Domain.Enums;
using MicroServicioRecetas.Application.DTOs;
using MicroServicioRecetas.Domain.Interfaces;
using MicroServicioRecetas.Domain.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace MicroServicioRecetas.Application.Consumers
{
    public class RecetaConsumer
    {
        private readonly IModel _channel;
        private readonly IRecetaRepository _recetaRepository; // Repositorio para manejar recetas

        public RecetaConsumer(IModel channel, IRecetaRepository recetaRepository)
        {
            _channel = channel;
            _recetaRepository = recetaRepository;

            // Declarar la cola y el exchange para recetas
            _channel.QueueDeclare(queue: "recetasQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.ExchangeDeclare(exchange: "recetaExchange", type: "direct", durable: false);
            _channel.QueueBind(queue: "recetasQueue", exchange: "recetaExchange", routingKey: MessageType.recetaRoutingKey.ToString());

            // Declarar la cola de logs
            _channel.QueueDeclare(queue: "logQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.ExchangeDeclare(exchange: "logExchange", type: "direct", durable: false);
            _channel.QueueBind(queue: "logQueue", exchange: "logExchange", routingKey: MessageType.logRoutingKey.ToString());
        }

        public void StartConsuming()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                try
                {
                    // Determinar el tipo de mensaje por la routingKey
                    var routingKey = ea.RoutingKey;
                    var messageType = Enum.TryParse(routingKey, out MessageType type) ? type : MessageType.Unknown;

                    switch (messageType)
                    {
                        case MessageType.recetaRoutingKey:
                            var recetaData = JsonConvert.DeserializeObject<RecetaRequest>(message);
                            await ProcessRecetaRequest(recetaData);
                            break;

                        case MessageType.logRoutingKey:
                            await HandleLogMessage(message);
                            break;

                        default:
                            await HandleLogMessage("Tipo de mensaje no reconocido.");
                            break;
                    }
                }
                catch (JsonException ex)
                {
                    await HandleLogMessage($"Error al deserializar el mensaje: {ex.Message}");
                }
                catch (Exception ex)
                {
                    await HandleLogMessage($"Error al procesar el mensaje: {ex.Message}");
                }
            };

            _channel.BasicConsume(queue: "recetasQueue", autoAck: true, consumer: consumer);
            _channel.BasicConsume(queue: "logQueue", autoAck: true, consumer: consumer);
        }

        private async Task ProcessRecetaRequest(RecetaRequest recetaData)
        {
            if (recetaData == null)
            {
                await HandleLogMessage("No se recibió datos de receta.");
                return;
            }

            var receta = new Receta
            {
                CitaId = recetaData.CitaId,
                PacienteId = recetaData.PacienteId,
                MedicoId = recetaData.MedicoId,
                Descripcion = "Descripción de la receta", // Puedes solicitar esto al médico
                FechaCreacion = DateTime.Now,
                Estado = "Activa"
            };

            // Agregar la receta a la base de datos
            await _recetaRepository.AddRecetaAsync(receta);
        }

        private async Task HandleLogMessage(string message)
        {
            Console.WriteLine(message);
            Debug.WriteLine("WriteLine: " + message);
            await Task.CompletedTask;
        }
    }

}
