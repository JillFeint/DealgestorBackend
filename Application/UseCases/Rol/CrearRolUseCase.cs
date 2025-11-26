using Application.DTOs.Roles;
﻿using Application.Ports.DrivenPorts.Rol;
﻿using Application.Ports.DriverPorts.Rol;
﻿using Domain.Entities;
﻿using System;
﻿using System.Threading.Tasks;
﻿
﻿namespace Application.UseCases.Rol
﻿{
﻿    public class CrearRolUseCase : PortDriverRolCrear
﻿    {
﻿        private readonly PortDrivenRolCrear _rolPersistencePort;
﻿
﻿        public CrearRolUseCase(PortDrivenRolCrear rolPersistencePort)
﻿        {
﻿            _rolPersistencePort = rolPersistencePort;
﻿        }
﻿
﻿        public async Task<RolDTODriver> CrearNuevoRol(RolDTODriver nuevoRolDTO)
﻿        {
﻿            bool rolExiste = await _rolPersistencePort.ExisteRolPorNombre(nuevoRolDTO.Name, nuevoRolDTO.Tipe);
﻿            if (rolExiste)
﻿            {
﻿                throw new ArgumentException($"El Rol con el nombre '{nuevoRolDTO.Name}' ya existe en el sistema.");
﻿            }

            Domain.Entities.Rol rolPersistido = await _rolPersistencePort.CrearRol(nuevoRolDTO);
﻿
﻿            var rolCreadoDTO = new RolDTODriver
﻿            {
﻿                Identidad = rolPersistido.Identificacion,
﻿                Tipe = rolPersistido.Tipo,
﻿                Name = rolPersistido.Nombre
﻿            };
﻿
﻿            return rolCreadoDTO;
﻿        }
﻿    }
﻿}
﻿