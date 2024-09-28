using MicroServicioRecetas.Application.DTOs;
using MicroServicioRecetas.Domain.Interfaces;
using MicroServicioRecetas.Domain.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
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

            // Declarar la cola y el exchange
            _channel.QueueDeclare(queue: "recetasQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.ExchangeDeclare(exchange: "recetaExchange", type: "direct", durable: false);
            _channel.QueueBind(queue: "recetasQueue", exchange: "recetaExchange", routingKey: "recetaRoutingKey");
        }

        public void StartConsuming()
        {
            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                try
                {
                    var recetaData = JsonConvert.DeserializeObject<RecetaRequest>(message);
                    await ProcessRecetaRequest(recetaData);
                }
                catch (JsonException ex)
                {
                    // Maneja el error de deserialización aquí
                    Console.WriteLine($"Error al deserializar el mensaje: {ex.Message}");
                }
                catch (Exception ex)
                {
                    // Maneja cualquier otro error aquí
                    Console.WriteLine($"Error al procesar el mensaje: {ex.Message}");
                }
            };

            _channel.BasicConsume(queue: "recetasQueue", autoAck: true, consumer: consumer);
        }

        private async Task ProcessRecetaRequest(RecetaRequest recetaData)
        {
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
    }
}
