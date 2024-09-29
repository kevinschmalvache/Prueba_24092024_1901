using MicroServicioCitas.Application.Interfaces;
using RestSharp;
using System.Threading.Tasks;

namespace MicroServicioCitas.Application.Services
{
    /// <summary>
    /// Servicio para interactuar con la API de Personas.
    /// </summary>
    public class PersonaApiService : IPersonaApi
    {
        private readonly RestClient _client;

        /// <summary>
        /// Constructor de la clase PersonaApiService.
        /// </summary>
        /// <param name="baseUrl">URL base de la API de Personas.</param>
        public PersonaApiService(string baseUrl)
        {
            _client = new RestClient(baseUrl);
        }

        /// <summary>
        /// Valida si un paciente existe en la API de Personas.
        /// </summary>
        /// <param name="id">Identificador del paciente.</param>
        /// <returns>True si el paciente es válido, de lo contrario false.</returns>
        public async Task<bool> ValidatePaciente(int id)
        {
            var request = new RestRequest($"api/personas/validate/{id}/paciente", Method.Get);
            var response = await _client.ExecuteAsync<bool>(request);
            return response.Data;
        }

        /// <summary>
        /// Valida si un médico existe en la API de Personas.
        /// </summary>
        /// <param name="id">Identificador del médico.</param>
        /// <returns>True si el médico es válido, de lo contrario false.</returns>
        public async Task<bool> ValidateMedico(int id)
        {
            var request = new RestRequest($"api/personas/validate/{id}/medico", Method.Get);
            var response = await _client.ExecuteAsync<bool>(request);
            return response.Data;
        }
    }
}
