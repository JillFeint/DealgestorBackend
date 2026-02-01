using Application.DTOs.Perfiles;
using Application.Ports.DrivenPorts.Perfil;
using Application.Ports.DriverPorts.Perfil;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.Perfil
{
    public class ConsultarPerfilUseCase : PortDriverPerfilConsultar
    {
        private readonly PortDrivenPerfilConsultar _perfilDrivenConsultar;

        public ConsultarPerfilUseCase(PortDrivenPerfilConsultar perfilDrivenConsultar)
        {
            _perfilDrivenConsultar = perfilDrivenConsultar ?? throw new ArgumentNullException(nameof(perfilDrivenConsultar));
        }

        public async Task<PerfilRespuestaDTODriver> ConsultarPerfilPorEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El email no puede estar vacío.", nameof(email));

            Domain.Entities.Perfil perfilDominio = await _perfilDrivenConsultar.ConsultarPerfilPorEmailAsync(email);

            if (perfilDominio == null)
            {
                throw new ArgumentException("No se encontró ningún perfil con el email especificado.", nameof(email));
            }

            return new PerfilRespuestaDTODriver
            {           
                Email = perfilDominio.Email,
                Negocios = perfilDominio.Negocios,
                Roles = perfilDominio.PermisosRol,
                FechaCreacion = perfilDominio.FechaCreacion
            };
        }
    }
}