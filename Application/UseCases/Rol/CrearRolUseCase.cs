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
﻿            bool rolExiste = await _rolPersistencePort.ExisteRolPorNombre(nuevoRolDTO.Nombre, nuevoRolDTO.Tipo);
﻿            if (rolExiste)
﻿            {
﻿                throw new ArgumentException($"El Rol con el nombre '{nuevoRolDTO.Nombre}' ya existe en el sistema.");
﻿            }

﻿            RolDTODriver rolPersistido = await _rolPersistencePort.CrearRol(rolDTODriver);
﻿
﻿            var rolCreadoDTO = new RolDTODriven
﻿            {
﻿                Identificacion = rolPersistido.tblIdentificacion,
﻿                Tipo = rolPersistido.tblTipo,
﻿                Nombre = rolPersistido.tblNombre
﻿            };
﻿
﻿            return rolCreadoDTO;
﻿        }
﻿    }
﻿}
﻿