using Proyecto.infrastructure.DbContexts;
using Proyecto.Models.Entities;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Proyecto.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ConcesionarioDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ConcesionarioDbContext context)
        {
            context.Categorias.AddOrUpdate(
                c => c.Nombre,
                new Categoria { Nombre = "Sedán" },
                new Categoria { Nombre = "SUV" },
                new Categoria { Nombre = "Pick-up" },
                new Categoria { Nombre = "Hatchback" }
            );

            context.Sucursales.AddOrUpdate(
                s => s.Nombre,
                new Sucursal { Nombre = "Sucursal Central", Direccion = "San José Centro", Telefono = "2222-0000" },
                new Sucursal { Nombre = "Sucursal Este", Direccion = "San Pedro", Telefono = "2222-1111" }
            );

            context.SaveChanges();
        }
    }
}