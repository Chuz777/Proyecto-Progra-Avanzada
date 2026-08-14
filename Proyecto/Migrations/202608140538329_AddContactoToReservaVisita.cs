namespace Proyecto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddContactoToReservaVisita : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReservaVisita", "NombreContacto", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.ReservaVisita", "EmailContacto", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.ReservaVisita", "TelefonoContacto", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReservaVisita", "TelefonoContacto");
            DropColumn("dbo.ReservaVisita", "EmailContacto");
            DropColumn("dbo.ReservaVisita", "NombreContacto");
        }
    }
}
