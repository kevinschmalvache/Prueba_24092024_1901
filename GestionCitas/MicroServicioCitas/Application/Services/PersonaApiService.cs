using MicroServicioCitas.Application.Interfaces;
using RestSharp;
using System.Threading.Tasks;

namespace MicroServicioCitas.Application.Services
{
    public class PersonaApiService : IPersonaApi
    {
        private readonly RestClient _client;

        public PersonaApiService(string baseUrl)
        {
            _client = new RestClient(baseUrl);
        }

        public async Task<bool> ValidatePaciente(int id)
        {
            var request = new RestRequest($"api/personas/validate/{id}/paciente", Method.Get);
            var response = await _client.ExecuteAsync<bool>(request);
            return response.Data;
        }

        public async Task<bool> ValidateMedico(int id)
        {
            var request = new RestRequest($"api/personas/validate/{id}/medico", Method.Get);
            var response = await _client.ExecuteAsync<bool>(request);
            return response.Data;
        }
    }
}
