using System;

namespace Application.ViewModels
{
    public class NegocioViewModel
    {
        public Guid Id { get; set; }
        public int Referencia { get; set; }
        public string Nombre { get; set; }
        public string TipoNegocio { get; set; }
    }
}
