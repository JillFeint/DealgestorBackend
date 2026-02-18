using Application.Ports.DrivenPorts.Tarjeta;
using Domain.Entities;
using Infrastructure.Tarjetas;
using Infrastructure.Tarjetas.TarjetaPort;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Tarjetas.TarjetaAdapter
{
    public class EmisorDeTarjetas : PortTarjetaGenerador, IGeneradorDeTokens
    {
        private readonly ConfiguracionDeTarjeta _config;

        public EmisorDeTarjetas(IOptions<ConfiguracionDeTarjeta> opciones)
        {
            _config = opciones.Value;
        }

        public (string Token, DateTime FechaExpiracion) CrearTarjetaAcceso(Perfil perfil)
        {
            // Validación: Verificar que el perfil tenga roles asignados
            if (perfil.PermisosRol == null || perfil.PermisosRol.Count == 0)
            {
                throw new InvalidOperationException(
                    $"El perfil '{perfil.Email}' no tiene roles asignados. " +
                    "No se puede generar un token sin roles.");
            }

            // Claims: La información que se "imprime" en la Tarjeta.
            var datosDeLaTarjeta = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, perfil.Identidad.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, perfil.Email)
            };

            // Agregar cada rol como claim individual
            // Esto permite que [Authorize(Roles = "Admin")] funcione correctamente
            foreach (var rol in perfil.PermisosRol)
            {
                // Validación: Verificar que el rol tenga nombre válido
                if (string.IsNullOrWhiteSpace(rol.Nombre))
                {
                    throw new InvalidOperationException(
                        $"El perfil '{perfil.Email}' tiene un rol sin nombre. " +
                        "Datos inconsistentes en la base de datos.");
                }
                
                datosDeLaTarjeta.Add(new Claim(ClaimTypes.Role, rol.Nombre));
            }

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Secret));
            var credenciales = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            DateTime fechaExpiracion = DateTime.UtcNow.AddMinutes(_config.DuracionEnMinutos);

            var descriptorDeTarjeta = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(datosDeLaTarjeta),
                Expires = fechaExpiracion,
                Issuer = _config.Issuer,
                Audience = _config.Audience,
                SigningCredentials = credenciales
            };

            var manejadorDeToken = new JwtSecurityTokenHandler();
            var tarjeta = manejadorDeToken.CreateToken(descriptorDeTarjeta);

            return (manejadorDeToken.WriteToken(tarjeta), fechaExpiracion);
        }

        // Mantener compatibilidad con PortTarjetaGenerador
        string PortTarjetaGenerador.CrearTarjetaAcceso(Perfil perfil)
        {
            return CrearTarjetaAcceso(perfil).Token;
        }
    }
}
