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

            // 2. Mapeo a la Entidad de Dominio (Input DTO -> Domain Entity)
            // Aquí es donde el DTO se convierte en un objeto que la capa de Dominio entiende.


            var nuevoRol = new Domain.Entities.Rol(
                Identificacion: nuevoRolDTO.Identificacion == Guid.Empty ? Guid.NewGuid() : nuevoRolDTO.Identificacion,
                Tipo: nuevoRolDTO.Tipo,
                Nombre: nuevoRolDTO.Nombre
            );

            // 3. Invocación al Puerto de Salida (Driven Port)
            // El Core (Caso de Uso) le dice a un puerto lo que quiere hacer (persistir).
            // No le importa CÓMO se hace (SQL, NoSQL, etc.).
            RolDTODriven rolPersistido = await _rolPersistencePort.GuardarRol(nuevoRol);

            // 4. Mapeo del Resultado (Domain Entity -> Output DTO)
            // La entidad de dominio se mapea de nuevo al DTO que el Driver Adapter espera.
            var rolCreadoDTO = new RolDTODriven
            {
                Identificacion = rolPersistido.Identificacion,
                Tipo = rolPersistido.Tipo,
                Nombre = rolPersistido.Nombre
            };

            return rolCreadoDTO;
        }

        // --- Implementación del método existente ---
        public async Task<List<RolDTODriven>> ConsultarIdentificadoresRol(string nombre)
        {
            // Lógica para consultar...
            throw new NotImplementedException();
        }
    }
}