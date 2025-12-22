using Application.DTOs.Roles;
using Application.Ports.DrivenPorts.Rol;
using Application.Ports.DriverPorts.Rol;
using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Application.UseCases.Rol
{
    public class CrearRolUseCase : PortDriverRolCrear
    {
        private readonly PortDrivenRolCrear _rolPersistencePort;

        public CrearRolUseCase(PortDrivenRolCrear rolPersistencePort)
        {
            _rolPersistencePort = rolPersistencePort;
        }

        public async Task<RolDTODriver> CrearNuevoRol(RolDTODriver nuevoRolDTO)
        {
            ValidarEntrada(nuevoRolDTO);

            string nombreOriginal = nuevoRolDTO.Name.Trim();
            string tipoOriginal = nuevoRolDTO.Tipe.Trim();
            string nombreCmp = nombreOriginal.ToUpperInvariant();
            string tipoCmp = tipoOriginal.ToUpperInvariant();

            bool rolExiste = await _rolPersistencePort.ExisteRolPorNombre(nombreCmp, tipoCmp);
            if (rolExiste)
            {
                throw new ArgumentException($"El Rol con el nombre '{nombreOriginal}' ya existe en el sistema.");
            }

            Domain.Entities.Rol rolPersistido = await _rolPersistencePort.CrearRol(new RolDTODriver
            {
                Identidad = nuevoRolDTO.Identidad,
                Name = nombreOriginal,
                Tipe = tipoOriginal
            });

            var rolCreadoDTO = new RolDTODriver
            {
                Identidad = rolPersistido.Identificacion,
                Tipe = rolPersistido.Tipo,
                Name = rolPersistido.Nombre
            };

            return rolCreadoDTO;
        }

        private void ValidarEntrada(RolDTODriver dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.Name)) throw new ArgumentException("El nombre es obligatorio.", nameof(dto.Name));
            if (string.IsNullOrWhiteSpace(dto.Tipe)) throw new ArgumentException("El tipo es obligatorio.", nameof(dto.Tipe));
        }
    }
}