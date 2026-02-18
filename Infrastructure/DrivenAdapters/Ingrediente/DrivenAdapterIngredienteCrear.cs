using Application.DTOs.Ingredientes;
using Application.Ports.DrivenPorts.Ingrediente;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Infrastructure.DrivenAdapters.Ingrediente
{
    /// <summary>
    /// Adaptador conducido para crear ingredientes en la base de datos
    /// </summary>
    public class DrivenAdapterIngredienteCrear : PortDrivenIngredienteCrear
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DrivenAdapterIngredienteCrear> _logger;

        public DrivenAdapterIngredienteCrear(ApplicationDbContext dbContext, ILogger<DrivenAdapterIngredienteCrear> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Verifica si existe un ingrediente con la referencia y nombre especificados
        /// </summary>
        /// <param name="referencia">Referencia del ingrediente</param>
        /// <param name="nombre">Nombre del ingrediente</param>
        /// <returns>True si existe, False en caso contrario</returns>
        public async Task<bool> ExisteIngredienteNombre(int referencia, string nombre)
        {
            try
            {
                _logger.LogInformation("Verificando existencia de ingrediente: Ref {Ref}, Nombre {Nombre}", referencia, nombre);

                var existe = await _dbContext.tblIngredientes.AnyAsync(i => 
                    i.tblReferencia == referencia && 
                    i.tblNombreIngrediente.ToLower() == nombre.ToLower());

                if (existe)
                {
                    _logger.LogWarning("Ingrediente ya existe: Ref {Ref}, Nombre {Nombre}", referencia, nombre);
                }
                else
                {
                    _logger.LogInformation("Ingrediente no existe: Ref {Ref}, Nombre {Nombre}", referencia, nombre);
                }

                return existe;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar existencia de ingrediente: Ref {Ref}, Nombre {Nombre}", referencia, nombre);
                throw;
            }
        }

        /// <summary>
        /// Crea un nuevo ingrediente en la base de datos
        /// </summary>
        /// <param name="ingrediente">DTO del ingrediente a crear</param>
        /// <returns>Entidad Ingrediente creada</returns>
        public async Task<Domain.Entities.Ingrediente> CrearIngrediente(IngredienteDTODriver ingrediente)
        {
            try
            {
                _logger.LogInformation("Creando nuevo ingrediente: {@Ingrediente}", 
                    new { ingrediente.NameIngredient, ingrediente.Ref });

                if (ingrediente.Identidad == Guid.Empty)
                    ingrediente.Identidad = Guid.NewGuid();

                var nuevoIngrediente = new IngredienteDTODriven(
                    tblId: ingrediente.Identidad,
                    tblReferencia: ingrediente.Ref,
                    tblNombreIngrediente: ingrediente.NameIngredient,
                    tblCantidad: ingrediente.Quantity,
                    tblPrecioPaquete: ingrediente.PrecioPack,
                    tblPrecioUnitario: ingrediente.PrecioUnidad
                );

                _dbContext.tblIngredientes.Add(nuevoIngrediente);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Ingrediente creado exitosamente: {@IngredienteCreado}", 
                    new { nuevoIngrediente.tblReferencia, nuevoIngrediente.tblNombreIngrediente });

                return new Domain.Entities.Ingrediente
                {
                    Id = nuevoIngrediente.tblId,
                    Referencia = nuevoIngrediente.tblReferencia,
                    NombreIngrediente = nuevoIngrediente.tblNombreIngrediente,
                    Cantidad = nuevoIngrediente.tblCantidad,
                    PrecioPaquete = nuevoIngrediente.tblPrecioPaquete,
                    PrecioUnitario = nuevoIngrediente.tblPrecioUnitario
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear ingrediente: {@Ingrediente}", 
                    new { ingrediente.NameIngredient, ingrediente.Ref });
                throw;
            }
        }
    }
}