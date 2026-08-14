
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Proyecto.Models;
using Proyecto.Models.Entities;
using Proyecto.infrastructure.DbContexts; // <--- Importante para ConcesionarioDbContext

namespace Proyecto.infrastructure.DbContexts
{
    public class ConcesionarioDbContext : DbContext
    {
        // Pasa la cadena de conexión del Web.config
        public ConcesionarioDbContext() : base("ConcesionarioDbContext")
        {
            this.Configuration.LazyLoadingEnabled = true;
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ConcesionarioDbContext>());
        }
        

        // --- Tablas de la Base de Datos ---
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Sucursal> Sucursales { get; set; }
        public DbSet<Vehiculo> Vehiculos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ReservaVisita> ReservasVisitas { get; set; }
        public DbSet<Cotizador> Cotizaciones { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Evita nombres de tablas en plural de inglés automáticamente
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Configuración de precisión para valores monetarios en SQL
            modelBuilder.Entity<Vehiculo>()
                .Property(v => v.Precio)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Cotizador>()
                .Property(c => c.PrecioFinal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Cotizador>()
                .Property(c => c.PrimaSugerida)
                .HasPrecision(18, 2);

            // Relaciones de Integridad Referencial
            modelBuilder.Entity<Vehiculo>()
                .HasRequired(v => v.Categoria)
                .WithMany(c => c.Vehiculos)
                .HasForeignKey(v => v.CategoriaId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Vehiculo>()
                .HasRequired(v => v.Sucursal)
                .WithMany(s => s.Vehiculos)
                .HasForeignKey(v => v.SucursalId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReservaVisita>()
                .HasRequired(r => r.Vehiculo)
                .WithMany()
                .HasForeignKey(r => r.VehiculoId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReservaVisita>()
                .HasRequired(r => r.Usuario)
                .WithMany()
                .HasForeignKey(r => r.UsuarioId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);



        }
    }
}