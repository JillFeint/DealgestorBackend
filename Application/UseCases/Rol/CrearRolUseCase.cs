using Application.DTOs.Roles;
using Application.Ports.DrivenPorts.Rol;
using Application.Ports.DriverPorts.Rol;
using Domain.Entities;
using System;
using System.Threading.Tasks;
using System.Xml;

namespace Application.Services.Rol
{
    // Este es el Caso de Uso o Service Layer
    public class RolCrearPost : PortDriverRolCrear
    {
        private readonly PortDrivenRolCrear _rolPersistencePort;

        public RolCrearPost(PortDrivenRolCrear rolPersistencePort)
        {
            _rolPersistencePort = rolPersistencePort;
        }

        public async Task<RolDTODriver> CrearNuevoRol(RolDTODriver nuevoRolDTO)
        {
            bool rolExiste = await _rolPersistencePort.ExisteRolPorTipo(nuevoRolDTO.Nombre);
            if (rolExiste)
            {
                throw new ArgumentException($"El Rol con tipo '{nuevoRolDTO.Tipo}' ya existe en el sistema.");
            }

            var nuevoRol = new Domain.Entities.Rol(
                Identificacion: nuevoRolDTO.Identificacion == Guid.Empty ? Guid.NewGuid() : nuevoRolDTO.Identificacion,
                Tipo: nuevoRolDTO.Tipo,
                Nombre: nuevoRolDTO.Nombre
            );

            var rolDTODriven = new RolDTODriven
            {
                tblIdentificacion = nuevoRol.Identificacion,
                tblTipo = nuevoRol.Tipo,
                tblNombre = nuevoRol.Nombre
            };

            RolDTODriven rolPersistido = await _rolPersistencePort.GuardarRol(rolDTODriven);

            var rolCreadoDTO = new RolDTODriver
            {
                Identificacion = rolPersistido.tblIdentificacion,
                Tipo = rolPersistido.tblTipo,
                Nombre = rolPersistido.tblNombre
            };

            return rolCreadoDTO;
        }
    }
}