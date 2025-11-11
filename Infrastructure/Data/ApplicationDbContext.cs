using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Rol> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the Rol entity
            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasKey(e => e.Identificacion);
                entity.Property(e => e.Identificacion).HasColumnName("tblIdentificacion");
                entity.Property(e => e.Nombre).HasColumnName("tblNombre").IsRequired();
                entity.Property(e => e.Tipo).HasColumnName("tblTipo").IsRequired();
            });
        }
    }
}
