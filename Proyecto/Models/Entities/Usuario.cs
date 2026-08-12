using System;
using System.Security.Cryptography;
using System.Text;

namespace Proyecto.Models.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; } // "Admin", "Asesor", "Cliente"



        // TODO: implementar la verificación real (hash) — esto es solo temporal

        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        public bool ValidarPassword(string password)
        {
            return PasswordHash == HashPassword(password);
        }

        public bool EsAdmin()
        {
            return Rol == "Admin";
        }
    }
}