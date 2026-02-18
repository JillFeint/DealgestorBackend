using Application.DTOs.Productos;
using Application.Ports.DrivenPorts.Producto;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.DrivenAdapters.Producto
{
    /// <summary>
    /// Adaptador conducido para crear productos en la base de datos.
    /// </summary>
    public class DrivenAdapterProductoCrear : PortDrivenProductoCrear
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DrivenAdapterProductoCrear> _logger;

        public DrivenAdapterProductoCrear(ApplicationDbContext dbContext, ILogger<DrivenAdapterProductoCrear> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Verifica si existe un producto con la referencia o nombre especificados.
        /// </summary>
        public async Task<bool> ExisteProductoNombre(int referencia, string nombre)
        {
            try
            {
                _logger.LogInformation("Verificando existencia de producto: Ref {Ref}, Nombre {Nombre}", referencia, nombre);

                bool existe = await _dbContext.tblProductos.AnyAsync(p =>
                    p.tblReferencia == referencia & p.tblNombre.ToLower() == nombre.ToLower());

                if (existe)
                {
                    _logger.LogWarning("Producto ya existe: Ref {Ref}, Nombre {Nombre}", referencia, nombre);
                }
                else
                {
                    _logger.LogInformation("Producto no existe: Ref {Ref}, Nombre {Nombre}", referencia, nombre);
                }

                return existe;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar existencia de producto: Ref {Ref}, Nombre {Nombre}", referencia, nombre);
                throw;
            }
        }

        /// <summary>
        /// Crea un nuevo producto en la base de datos.
        /// </summary>
        public async Task<Domain.Entities.Producto> CrearProducto(ProductoDTODriver producto)
        {
            try
            {
                _logger.LogInformation("Creando nuevo producto: {@Producto}", new { producto.Nombre, producto.Referencia });

                Guid id = Guid.NewGuid();
                List<Domain.Entities.Ingrediente> ingredientes = producto.IngredientesIds ?? new List<Domain.Entities.Ingrediente>();

                var nuevoProducto = new ProductoDTODriven
                {
                    tblId = id,
                    tblReferencia = producto.Referencia,
                    tblNombre = producto.Nombre,
                    tblImagen = producto.Imagen,
                    tblCategoria = producto.Categoria,
                    tblPrecioSugerido = producto.PrecioSugerido,
                    tblCosto = producto.Costo,
                    tblIngredientesIds = ingredientes
                };

                _dbContext.tblProductos.Add(nuevoProducto);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Producto creado exitosamente: {@ProductoCreado}", 
                    new { nuevoProducto.tblReferencia, nuevoProducto.tblNombre });

                return new Domain.Entities.Producto
                {
                    Id = nuevoProducto.tblId,
                    Referencia = nuevoProducto.tblReferencia,
                    Nombre = nuevoProducto.tblNombre,
                    Imagen = nuevoProducto.tblImagen,
                    Categoria = nuevoProducto.tblCategoria,
                    PrecioSugerido = nuevoProducto.tblPrecioSugerido,
                    Costo = nuevoProducto.tblCosto,
                    Ingredientes = nuevoProducto.tblIngredientesIds
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear producto: {@Producto}", new { producto.Nombre, producto.Referencia });
                throw;
            }
        }
    }
}