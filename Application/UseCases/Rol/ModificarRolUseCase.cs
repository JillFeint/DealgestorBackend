using Application.DTOs.Roles;
using Application.Ports.DrivenPorts.Rol;
using Application.Ports.DriverPorts.Rol;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.Rol
{
    public class ModificarRolUseCase : PortDriverRolModificar
    {
        private readonly IPortDrivenRolModificar _drivenRolModificar;

        public ModificarRolUseCase(IPortDrivenRolModificar drivenRolModificar)
        {
            _drivenRolModificar = drivenRolModificar;
        }

        public async Task<RolDTODriver> ModificarRol(RolDTODriver rolModificarDTO)
        {
            // Mapear RolDTODriver a la entidad de dominio Rol
            var rolToModify = new Domain.Entities.Rol
            {
                Identificacion = rolModificarDTO.Identificacion,
                Tipo = rolModificarDTO.Tipo,
                Nombre = rolModificarDTO.Nombre
            };

            // Llamar al puerto driven para modificar el rol
            var modifiedRol = await _drivenRolModificar.ModificarRol(rolToModify);

            // Mapear la entidad de dominio Rol modificada de nuevo a RolDTODriver
            var resultDTO = new RolDTODriver
            {
                Identificacion = modifiedRol.Identificacion,
                Tipo = modifiedRol.Tipo,
                Nombre = modifiedRol.Nombre
            };

            return resultDTO;
        }
    }
}
