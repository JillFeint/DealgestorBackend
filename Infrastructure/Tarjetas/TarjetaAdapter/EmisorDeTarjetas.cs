using Domain.Entities;
using Infrastructure.Tarjetas.TarjetaPort;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Tarjetas.TarjetaAdapter
{
    public class EmisorDeTarjetas : PortTarjetaGenerador
    {
        private readonly ConfiguracionDeTarjeta _config;

        public EmisorDeTarjetas(IOptions<ConfiguracionDeTarjeta> opciones)
        {
            _config = opciones.Value;
        }

        public string CrearTarjetaAcceso(Perfil perfil)
        {
            // Claims: La información que se "imprime" en la Tarjeta.
            var datosDeLaTarjeta = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, perfil.Identidad.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, perfil.Email),
                new Claim(ClaimTypes.Role, perfil.PermisosRol.ToString())
            };

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Secret));
            var credenciales = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var descriptorDeTarjeta = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(datosDeLaTarjeta),
                Expires = DateTime.UtcNow.AddMinutes(_config.DuracionEnMinutos),
                Issuer = _config.Issuer,
                Audience = _config.Audience,
                SigningCredentials = credenciales
            };

            var manejadorDeToken = new JwtSecurityTokenHandler();
            var tarjeta = manejadorDeToken.CreateToken(descriptorDeTarjeta);

            return manejadorDeToken.WriteToken(tarjeta);
        }
    }
}
