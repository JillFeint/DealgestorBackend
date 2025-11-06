using Application.Ports.DrivenPorts.Negocio;
using Domain.Entities;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System;
using System.Collections.Generic;

namespace Infrastructure.DrivenAdapters.Negocio
{
    public class DrivenAdapterNegocio : IDrivenNegocioRepository
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://api-externa-de-negocios.com/v1";

        public DrivenAdapterNegocio(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Domain.Entities.Negocio>> ObtenerNegociosDisponiblesAsync(Guid perfilId)
        {
            string endpoint = $"{BaseUrl}/negocios/disponibles-para/{perfilId}";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var negocios = JsonSerializer.Deserialize<List<Domain.Entities.Negocio>>(jsonResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return negocios ?? new List<Domain.Entities.Negocio>();
                }

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return new List<Domain.Entities.Negocio>();
                }

                throw new ApplicationException($"Error en API externa. Código: {response.StatusCode}");
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Fallo de comunicación con el servicio externo de negocios.", ex);
            }
        }
    }
}
