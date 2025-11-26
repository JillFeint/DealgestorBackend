﻿using System;

namespace Application.DTOs.Ingredientes
{
    public class IngredienteDTODriven
    {
        public Guid tblId { get; set; }
        public int tblReferencia { get; set; }
        public string tblNombreIngrediente { get; set; }
        public int tblCantidad { get; set; }
        public decimal tblPrecioPaquete { get; set; }
        public decimal tblPrecioUnitario { get; set; }

        public IngredienteDTODriven() { }

        public IngredienteDTODriven(Guid tblId, int tblReferencia, string tblNombreIngrediente, int tblCantidad , decimal tblPrecioUnitario, decimal tblPrecioPaquete)
        {
            this.tblId = tblId;
            this.tblReferencia = tblReferencia;
            this.tblNombreIngrediente = tblNombreIngrediente;
            this.tblCantidad = tblCantidad;
            this.tblPrecioPaquete = tblPrecioPaquete;
            this.tblPrecioUnitario = tblPrecioUnitario;
        }
    }
}

