namespace Proyecto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActualizacionCotizador : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cotizador", "VehiculoId", c => c.Int());
            AddColumn("dbo.Cotizador", "NombreCliente", c => c.String());
            AddColumn("dbo.Cotizador", "EmailCliente", c => c.String());
            CreateIndex("dbo.Cotizador", "VehiculoId");
            AddForeignKey("dbo.Cotizador", "VehiculoId", "dbo.Vehiculo", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cotizador", "VehiculoId", "dbo.Vehiculo");
            DropIndex("dbo.Cotizador", new[] { "VehiculoId" });
            DropColumn("dbo.Cotizador", "EmailCliente");
            DropColumn("dbo.Cotizador", "NombreCliente");
            DropColumn("dbo.Cotizador", "VehiculoId");
        }
    }
}
