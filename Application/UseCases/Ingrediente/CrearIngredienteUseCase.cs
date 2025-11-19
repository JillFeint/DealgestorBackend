using Application.DTOs.Ingredientes;
﻿using Application.Ports.DrivenPorts.Ingrediente;
﻿using Application.Ports.DriverPorts.Ingrediente;
﻿using Domain.Entities;
﻿using System;
﻿using System.Threading.Tasks;
﻿
﻿namespace Application.UseCases.Ingrediente
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
﻿        public async Task<IngredienteDTODriver> CrearNuevoIngrediente(IngredienteDTODriver nuevoIngredienteDTO)
﻿        {
﻿            bool rolExiste = await _ingredientePersistencePort.ExisteIngredienteNombre(nuevoIngredienteDTO.Referencia, nuevoIngredienteDTO.NombreIngrediente);
﻿            if (rolExiste)
﻿            {
﻿                throw new ArgumentException($"El Rol con el nombre '{nuevoIngredienteDTO.NombreIngrediente}' ya existe en el sistema.");
﻿            }

            Domain.Entities.Ingrediente ingredientePersistido = await _ingredientePersistencePort.CrearIngrediente(nuevoIngredienteDTO);
﻿
﻿            var ingredienteCreadoDTO = new IngredienteDTODriver
﻿            {
﻿                Id = ingredientePersistido.Id,
                Referencia = ingredientePersistido.Referencia,
                NombreIngrediente = ingredientePersistido.NombreIngrediente,
﻿                PrecioUnitario = ingredientePersistido.PrecioUnitario
﻿            };
﻿
﻿            return ingredienteCreadoDTO;
﻿        }
﻿    }
﻿}
﻿