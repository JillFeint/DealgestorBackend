﻿using System;

namespace Application.DTOs.Ingredientes
{
    public class IngredienteDTODriven
    {
        public Guid tblId { get; set; }
        public int tblReferencia { get; set; }
        public string tblNombreIngrediente { get; set; }
        public decimal tblPrecioUnitario { get; set; }

        public IngredienteDTODriven() { }

        public IngredienteDTODriven(Guid tblId, int tblReferencia, string tblNombreIngrediente, decimal tblPrecioUnitario)
        {
            this.tblId = tblId;
            this.tblReferencia = tblReferencia;
            this.tblNombreIngrediente = tblNombreIngrediente;
            this.tblPrecioUnitario = tblPrecioUnitario;
        }
    }
}

