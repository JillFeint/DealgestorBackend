using Application.DTOs.Productos;
using Application.Ports.DrivenPorts.Producto;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Infrastructure.DrivenAdapters.Producto
{
    /// <summary>
    /// Adaptador conducido para modificar productos en la base de datos
    /// </summary>
    public class DrivenAdapterProductoModificar : PortDrivenProductoModificar
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DrivenAdapterProductoModificar> _logger;

        public DrivenAdapterProductoModificar(ApplicationDbContext dbContext, ILogger<DrivenAdapterProductoModificar> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Domain.Entities.Producto?> ObtenerPorReferencia(int referencia)
        {
            try
            {
                _logger.LogInformation("Obteniendo producto por referencia: {Ref}", referencia);

                var producto = await _dbContext.tblProductos.FirstOrDefaultAsync(p => p.tblReferencia == referencia);

                if (producto == null)
                {
                    _logger.LogWarning("Producto no encontrado: Ref {Ref}", referencia);
                    return null;
                }

                return new Domain.Entities.Producto
                {
                    Id = producto.tblId,
                    Referencia = producto.tblReferencia,
                    Nombre = producto.tblNombre,
                    Imagen = producto.tblImagen,
                    Categoria = producto.tblCategoria,
                    PrecioSugerido = producto.tblPrecioSugerido,
                    Costo = producto.tblCosto,
                    Ingredientes = producto.tblIngredientesIds
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener producto: Ref {Ref}", referencia);
                throw;
            }
        }

        public async Task<bool> ExisteDuplicado(int referencia, string nombre, Guid excluirId)
        {
            try
            {
                _logger.LogInformation("Verificando duplicado de producto: Ref {Ref}, Nombre {Nombre}", referencia, nombre);

                bool existe = await _dbContext.tblProductos.AnyAsync(p =>
                    p.tblId != excluirId &&
                    (p.tblReferencia == referencia || p.tblNombre.ToLower() == nombre.ToLower()));

                if (existe)
                {
                    _logger.LogWarning("Producto duplicado detectado: Ref {Ref}, Nombre {Nombre}", referencia, nombre);
                }

                return existe;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar duplicado de producto: Ref {Ref}, Nombre {Nombre}", referencia, nombre);
                throw;
            }
        }

        /// <summary>
        /// Modifica un producto existente en la base de datos
        /// </summary>
        public async Task<Domain.Entities.Producto> ModificarProducto(ProductoDTODriver productoModificador)
        {
            try
            {
                _logger.LogInformation("Modificando producto: {@Producto}", new { productoModificador.Referencia, productoModificador.Nombre });

                ArgumentNullException.ThrowIfNull(productoModificador);

                var productoEnDb = await _dbContext.tblProductos.FirstOrDefaultAsync(p => p.tblReferencia == productoModificador.Referencia);

                if (productoEnDb == null)
                {
                    _logger.LogWarning("Producto no encontrado para modificar: Ref {Ref}", productoModificador.Referencia);
                    return null;
                }

                productoEnDb.tblNombre = productoModificador.Nombre;
                productoEnDb.tblImagen = productoModificador.Imagen;
                productoEnDb.tblCategoria = productoModificador.Categoria;
                productoEnDb.tblPrecioSugerido = productoModificador.PrecioSugerido;
                productoEnDb.tblCosto = productoModificador.Costo;
                productoEnDb.tblIngredientesIds = productoModificador.IngredientesIds;

                _dbContext.tblProductos.Update(productoEnDb);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Producto modificado exitosamente: {@ProductoModificado}",
                    new { productoEnDb.tblReferencia, productoEnDb.tblNombre });

                return new Domain.Entities.Producto
                {
                    Id = productoEnDb.tblId,
                    Referencia = productoEnDb.tblReferencia,
                    Nombre = productoEnDb.tblNombre,
                    Imagen = productoEnDb.tblImagen,
                    Categoria = productoEnDb.tblCategoria,
                    PrecioSugerido = productoEnDb.tblPrecioSugerido,
                    Costo = productoEnDb.tblCosto,
                    Ingredientes = productoEnDb.tblIngredientesIds
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al modificar producto: {@Producto}", new { productoModificador.Referencia, productoModificador.Nombre });
                throw;
            }
        }
    }
}