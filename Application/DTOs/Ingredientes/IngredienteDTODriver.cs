﻿using System;

namespace Application.DTOs.Ingredientes
{
    // Debe ser 'public' para ser accesible desde la capa de Infrastructure (el controlador)
    public class IngredienteDTODriver
    {
        public Guid Identidad { get; set; }
        public int Ref { get; set; }
        public string NameIngredient { get; set; }
        public int Quantity { get; set; }
        public decimal PrecioPack { get; set; }
        public decimal PrecioUnidad { get; set; }
    }
}
