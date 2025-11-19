﻿using System;

namespace Application.DTOs.Ingredientes
{
    // Debe ser 'public' para ser accesible desde la capa de Infrastructure (el controlador)
    public class IngredienteDTODriver
    {
        public Guid Id { get; set; }
        public int Referencia { get; set; }
        public string NombreIngrediente { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
