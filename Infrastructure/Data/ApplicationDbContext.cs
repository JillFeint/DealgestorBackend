using Application.DTOs.Ingredientes;
using Application.DTOs.Perfiles;
using Application.DTOs.Roles;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<RolDTODriven> tblRoles { get; set; }
        public DbSet<IngredienteDTODriven> tblIngredientes { get; set; }
        public DbSet<PerfilDTODriven> tblPerfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
                                
            modelBuilder.Entity<RolDTODriven>(entity =>
            {
                entity.HasKey(e => e.tblIdentificacion);
                entity.Property(e => e.tblIdentificacion).HasColumnName("tblIdentificacion");
                entity.Property(e => e.tblNombre).HasColumnName("tblNombre").IsRequired();
                entity.Property(e => e.tblTipo).HasColumnName("tblTipo").IsRequired();
            });

            modelBuilder.Entity<IngredienteDTODriven>(entity =>
            {
                entity.HasKey(e => e.tblId);
                entity.Property(e => e.tblReferencia).HasColumnName("tblReferencia");
                entity.Property(e => e.tblNombreIngrediente).HasColumnName("tblNombreIngrediente").IsRequired();
                entity.Property(e => e.tblCantidad).HasColumnName("tblCantidad").IsRequired();
                entity.Property(e => e.tblPrecioPaquete).HasColumnName("tblPrecioPaquete").IsRequired();
                entity.Property(e => e.tblPrecioUnitario).HasColumnName("tblPrecioUnitario").IsRequired();
            });

            modelBuilder.Entity<PerfilDTODriven>(entity =>
            {
                entity.HasKey(e => e.tblIdentidad);
                entity.Property(e => e.tblIdentidad).HasColumnName("tblIdentidad");
                entity.Property(e => e.tblEmail).HasColumnName("tblEmail").IsRequired();
                entity.Property(e => e.tblCodigoSecreto).HasColumnName("tblCodigoSecreto").IsRequired();
                entity.Property(e => e.tblFechaCreacion).HasColumnName("tblFechaCreacion").IsRequired();
                entity.Property(e => e.tblNegocios).HasColumnName("tblNegocios");
                entity.Property(e => e.tblPermisosRolIds).HasColumnName("tblPermisosRolIds");
            });
        }
    }
}