using System.Threading.Tasks;

namespace MicroServicioCitas.Application.Interfaces
{
    public interface IRabbitMqService
    {
        Task SendRecetaRequest(object citaId);
    }
}