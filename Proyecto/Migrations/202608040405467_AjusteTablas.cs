namespace Proyecto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjusteTablas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vehiculo", "Marca", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Vehiculo", "Anio", c => c.Int(nullable: false));
            AddColumn("dbo.Vehiculo", "VIN", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vehiculo", "VIN");
            DropColumn("dbo.Vehiculo", "Anio");
            DropColumn("dbo.Vehiculo", "Marca");
        }
    }
}
