using Proyecto.Common;
using Proyecto.infrastructure.DbContexts;
using Proyecto.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto.infrastructure.Services
{
    public class UsuarioService : IUsuarioService
    {
        public static readonly string[] RolesValidos = { "Admin", "Asesor", "Cliente" };

        private readonly ConcesionarioDbContext _db;

        public UsuarioService()
        {
            _db = new ConcesionarioDbContext();
        }

        public IEnumerable<UsuarioDTO> ObtenerTodos()
        {
            return _db.Usuarios
                .OrderBy(u => u.Username)
                .Select(u => new UsuarioDTO
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    Rol = u.Rol
                }).ToList();
        }

        public UsuarioDTO ObtenerPorId(int id)
        {
            var usuario = _db.Usuarios.Find(id);
            if (usuario == null) return null;

            return new UsuarioDTO
            {
                Id = usuario.Id,
                Username = usuario.Username,
                Email = usuario.Email,
                Rol = usuario.Rol
            };
        }

        public OperationResult CambiarRol(int id, string nuevoRol)
        {
            try
            {
                if (!RolesValidos.Contains(nuevoRol))
                {
                    return OperationResult.Fail("El rol seleccionado no es válido.");
                }

                var usuario = _db.Usuarios.Find(id);
                if (usuario == null)
                {
                    return OperationResult.Fail("El usuario no existe.");
                }

                usuario.Rol = nuevoRol;
                _db.SaveChanges();
                return OperationResult.Ok();
            }
            catch (Exception ex)
            {
                return OperationResult.Fail("Error al cambiar el rol: " + ex.Message);
            }
        }

        public OperationResult EliminarUsuario(int id, int idUsuarioActual)
        {
            try
            {
                if (id == idUsuarioActual)
                {
                    return OperationResult.Fail("No puede eliminar su propia cuenta.");
                }

                var usuario = _db.Usuarios.Find(id);
                if (usuario == null)
                {
                    return OperationResult.Fail("El usuario no existe.");
                }

                _db.Usuarios.Remove(usuario);
                _db.SaveChanges();
                return OperationResult.Ok();
            }
            catch (Exception ex)
            {
                return OperationResult.Fail("Error al eliminar el usuario: " + ex.Message);
            }
        }
    }
}