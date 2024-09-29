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
    /// <summary>
    /// Consumidor encargado de procesar mensajes relacionados con recetas y logs de RabbitMQ.
    /// </summary>
    public class RecetaConsumer
    {
        private readonly IModel _channel;
        private readonly IRecetaRepository _recetaRepository; // Repositorio para manejar recetas

        /// <summary>
        /// Constructor que inicializa el consumidor de recetas con el canal de RabbitMQ y el repositorio de recetas.
        /// Configura las colas y los intercambios (exchanges) para manejar recetas y logs.
        /// </summary>
        /// <param name="channel">Canal de comunicación de RabbitMQ.</param>
        /// <param name="recetaRepository">Repositorio para manejar las operaciones con recetas.</param>
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

        /// <summary>
        /// Inicia la recepción de mensajes desde las colas de RabbitMQ para recetas y logs.
        /// Los mensajes se procesan según el tipo de mensaje (recetas o logs).
        /// </summary>
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

        /// <summary>
        /// Procesa la solicitud de receta recibida desde la cola de RabbitMQ.
        /// Crea una nueva receta basada en los datos recibidos y la almacena en la base de datos.
        /// </summary>
        /// <param name="recetaData">Datos de la solicitud de receta.</param>
        /// <returns>Una tarea asíncrona.</returns>
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

        /// <summary>
        /// Maneja los mensajes de log recibidos desde la cola de RabbitMQ.
        /// Imprime el mensaje en la consola y lo registra en el depurador.
        /// </summary>
        /// <param name="message">Mensaje de log recibido.</param>
        /// <returns>Una tarea asíncrona.</returns>
        private async Task HandleLogMessage(string message)
        {
            Console.WriteLine(message);
            Debug.WriteLine("WriteLine: " + message);
            await Task.CompletedTask;
        }
    }
}
