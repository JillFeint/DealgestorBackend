using Application.DTOs.Roles;
using Application.Ports.DrivenPorts.Rol;
using Application.Ports.DriverPorts.Rol;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.Rol
{
    public class ConsultarRolUseCase : PortDriverRolConsultar
    {
        private readonly PortDrivenRolConsultar _rolDrivenConsultar;

        public ConsultarRolUseCase(PortDrivenRolConsultar rolDrivenConsultar)
        {
            _rolDrivenConsultar = rolDrivenConsultar;
        }

        public async Task<RolDTODriver> ConsultarIdentificadoresRol(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre no puede estar vacío.", nameof(nombre));

            Domain.Entities.Rol rolDominio = await _rolDrivenConsultar.ConsultarRolAsync(nombre);

            if (rolDominio == null)
            {
                throw new ArgumentException("No se encontró ningún rol con el nombre especificado.", nameof(nombre));
            }

            return new RolDTODriver {
                 Identidad = rolDominio.Identificacion,
                 Tipe = rolDominio.Tipo,
                 Name = rolDominio.Nombre 
            };
        }
    }
}