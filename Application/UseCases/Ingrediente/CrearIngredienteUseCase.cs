using Application.DTOs.Ingredientes;
using Application.DTOs.Roles;
﻿using Application.Ports.DrivenPorts.Rol;
﻿using Application.Ports.DriverPorts.Rol;
﻿using Domain.Entities;
﻿using System;
﻿using System.Threading.Tasks;
﻿
﻿namespace Application.UseCases.Rol
﻿{
﻿    public class CrearIngredienteUseCase : PortDriverIngredienteCrear
﻿    {
﻿        private readonly PortDrivenIngredienteCrear _ingredientePersistencePort;
﻿
﻿        public CrearIngredienteUseCase(PortDrivenIngredienteCrear ingredientePersistencePort)
﻿        {
            _ingredientePersistencePort = ingredientePersistencePort;
﻿        }
﻿
﻿        public async Task<IngredienteDTODriver> CrearNuevoRol(IngredienteDTODriver nuevoIngredienteDTO)
﻿        {
﻿            bool rolExiste = await _rolPersistencePort.ExisteIngredienteNombre(nuevoRolDTO.Name, nuevoRolDTO.Tipe);
﻿            if (rolExiste)
﻿            {
﻿                throw new ArgumentException($"El Rol con el nombre '{nuevoRolDTO.Name}' ya existe en el sistema.");
﻿            }

            Domain.Entities.Ingrediente ingredientePersistido = await _ingredientePersistencePort.CrearIngrediente(nuevoIngredienteDTO);
﻿
﻿            var ingredienteCreadoDTO = new IngredienteDTODriver
﻿            {
﻿                Identidad = ingredientePersistido.IngredienteDTODriver,
﻿                Tipe = ingredientePersistido.Tipo,
﻿                Name = ingredientePersistido.Nombre
﻿            };
﻿
﻿            return ingredienteCreadoDTO;
﻿        }
﻿    }
﻿}
﻿