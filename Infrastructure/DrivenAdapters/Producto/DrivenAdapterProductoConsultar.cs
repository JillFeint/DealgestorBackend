using Application.DTOs.Productos;
using Application.Ports.DrivenPorts.Producto;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DrivenAdapters.Producto
{
    /// <summary>
    /// Adaptador driven para consultar productos en la base de datos
    /// </summary>
    public class DrivenAdapterProductoConsultar : PortDrivenProductoConsultar
    {
        private readonly ApplicationDbContext _context;

        public DrivenAdapterProductoConsultar(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Consulta un producto por su nombre en la base de datos
        /// </summary>
        /// <param name="nombre">Nombre del producto a consultar</param>
        /// <returns>DTO del producto consultado</returns>
        public async Task<Domain.Entities.Producto> ConsultarProductoNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre del producto no puede estar vacío", nameof(nombre));
            }

            ProductoDTODriven producto = await _context.tblProductos
                .FirstOrDefaultAsync(p => p.tblNombre.ToLower() == nombre.ToLower());

            return new Domain.Entities.Producto
            {
                Referencia = producto.tblReferencia,
                Nombre = producto.tblNombre,
                Imagen = producto.tblImagen,
                Categoria = producto.tblCategoria,
                PrecioSugerido = producto.tblPrecioSugerido,
                Costo = producto.tblCosto,
                Ingredientes = producto.tblIngredientesIds
            };
        }
    }
}
