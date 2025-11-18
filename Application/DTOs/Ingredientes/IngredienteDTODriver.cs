﻿using System;

namespace Application.DTOs.Ingredientes
{
    // Debe ser 'public' para ser accesible desde la capa de Infrastructure (el controlador)
    public class IngredienteDTODriver
    {
        // Propiedades que se expondrán a través de la API.
        // Ahora están alineadas con tu entidad Domain.Entities.Ingrediente
        public Guid Id { get; set; }
        public int Referencia { get; set; }
        public string NombreIngrediente { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
