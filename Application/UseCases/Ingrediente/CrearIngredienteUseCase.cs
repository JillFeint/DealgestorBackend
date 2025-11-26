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
﻿            bool ingredienteExiste = await _ingredientePersistencePort.ExisteIngredienteNombre(nuevoIngredienteDTO.Ref, nuevoIngredienteDTO.NameIngredient);
﻿            if (ingredienteExiste)
﻿            {
﻿                throw new ArgumentException($"El Rol con el nombre '{nuevoIngredienteDTO.NameIngredient}' ya existe en el sistema.");
﻿            }

            Domain.Entities.Ingrediente ingredientePersistido = await _ingredientePersistencePort.CrearIngrediente(nuevoIngredienteDTO);
﻿
﻿            var ingredienteCreadoDTO = new IngredienteDTODriver
﻿            {
﻿                Identidad = ingredientePersistido.Id,
                Ref = ingredientePersistido.Referencia,
                NameIngredient = ingredientePersistido.NombreIngrediente,
                Quantity = ingredientePersistido.Cantidad,
                PrecioPack = ingredientePersistido.PrecioPaquete,
                PrecioUnidad = ingredientePersistido.PrecioUnitario
﻿            };
﻿
﻿            return ingredienteCreadoDTO;
﻿        }
﻿    }
﻿}
﻿