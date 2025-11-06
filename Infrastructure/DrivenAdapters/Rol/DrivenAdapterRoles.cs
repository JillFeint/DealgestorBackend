using Application.Ports.DrivenPorts.Rol;
using Domain.Entities;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System;

namespace Infrastructure.DrivenAdapters.Rol
{
    // 💡 Implementa: El puerto de salida.
    public class DrivenAdapterRoles : IDrivenRolRepository
    {
        private readonly HttpClient _httpClient;
        // 💡 Seguridad: Se recomienda cargar la URL base desde la configuración (IConfiguration).
        private const string BaseUrl = "http://api-externa-de-negocios.com/v1";

        public DrivenAdapterRoles(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Domain.Entities.Rol>> ConsultarRolAsync(string nombre)
        {
            // Dummy implementation
            await Task.CompletedTask;
            return new List<Domain.Entities.Rol>();
        }
    }
}