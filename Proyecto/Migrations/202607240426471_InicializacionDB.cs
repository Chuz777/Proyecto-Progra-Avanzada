namespace Proyecto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InicializacionDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categoria",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Vehiculo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Modelo = c.String(nullable: false, maxLength: 100),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ImagenUrl = c.String(maxLength: 250),
                        Descripcion = c.String(nullable: false, maxLength: 500),
                        Estado = c.String(nullable: false, maxLength: 20),
                        CategoriaId = c.Int(nullable: false),
                        SucursalId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categoria", t => t.CategoriaId)
                .ForeignKey("dbo.Sucursal", t => t.SucursalId)
                .Index(t => t.CategoriaId)
                .Index(t => t.SucursalId);
            
            CreateTable(
                "dbo.Sucursal",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Direccion = c.String(nullable: false, maxLength: 200),
                        Telefono = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Cotizador",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PrecioFinal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PrimaSugerida = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PlazoMeses = c.Int(nullable: false),
                        FechaCotizacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ReservaVisita",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FechaVista = c.DateTime(nullable: false),
                        EstadoReserva = c.String(nullable: false, maxLength: 20),
                        VehiculoId = c.Int(nullable: false),
                        UsuarioId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuario", t => t.UsuarioId)
                .ForeignKey("dbo.Vehiculo", t => t.VehiculoId)
                .Index(t => t.VehiculoId)
                .Index(t => t.UsuarioId);
            
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        PasswordHash = c.String(),
                        Email = c.String(),
                        Rol = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReservaVisita", "VehiculoId", "dbo.Vehiculo");
            DropForeignKey("dbo.ReservaVisita", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.Vehiculo", "SucursalId", "dbo.Sucursal");
            DropForeignKey("dbo.Vehiculo", "CategoriaId", "dbo.Categoria");
            DropIndex("dbo.ReservaVisita", new[] { "UsuarioId" });
            DropIndex("dbo.ReservaVisita", new[] { "VehiculoId" });
            DropIndex("dbo.Vehiculo", new[] { "SucursalId" });
            DropIndex("dbo.Vehiculo", new[] { "CategoriaId" });
            DropTable("dbo.Usuario");
            DropTable("dbo.ReservaVisita");
            DropTable("dbo.Cotizador");
            DropTable("dbo.Sucursal");
            DropTable("dbo.Vehiculo");
            DropTable("dbo.Categoria");
        }
    }
}
