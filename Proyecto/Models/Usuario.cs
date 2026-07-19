using System;

namespace WebApplication1.Models
{
	public class Usuario
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public string PasswordHash { get; set; }
		public string Email { get; set; }
		public string Rol { get; set; } // "Admin", "Asesor", "Cliente"

		public bool ValidarPassword(string password)
		{
			// TODO: implementar la verificación real (hash) — esto es solo temporal
			return true;
		}

		public bool EsAdmin()
		{
			return Rol == "Admin";
		}
	}
}