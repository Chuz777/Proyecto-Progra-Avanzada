using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
            // Nota: Si tu Usuario hereda de IdentityUser o estás usando una tabla propia, 
            // ajustamos los tipos a Usuario:
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<Usuario>(new UserStore<Usuario>(context));

            // 1. Definir los roles del concesionario
            string[] roles = { "Admin", "Vendedor", "Cliente" };

            foreach (var roleName in roles)
            {
                if (!roleManager.RoleExists(roleName))
                {
                    roleManager.Create(new IdentityRole(roleName));
                }
            }

            // 2. Crear Usuario Administrador por Defecto
            string adminEmail = "admin@concesionario.com";
            var adminUser = userManager.FindByEmail(adminEmail);

            if (adminUser == null)
            {
                adminUser = new Usuario
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                // Contraseña inicial
                var result = userManager.Create(adminUser, "Admin123!");

                if (result.Succeeded)
                {
                    userManager.AddToRole(adminUser.Id, "Admin");
                }
            }
        }
    }
}