using System.Linq;
using System.Web.Mvc;
using Proyecto.infrastructure.DbContexts;
using Proyecto.Models.Entities;

namespace Proyecto.Controllers
{
    public class AccountController : BaseController
    {
        private readonly ConcesionarioDbContext _db;

        public AccountController()
        {
            _db = new ConcesionarioDbContext();
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password, string returnUrl)
        {
            var usuario = _db.Usuarios.FirstOrDefault(u => u.Username == username);

            if (usuario == null || !usuario.ValidarPassword(password))
            {
                ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            Session["UsuarioId"] = usuario.Id;
            Session["Username"] = usuario.Username;
            Session["Rol"] = usuario.Rol;

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(string username, string email, string password, string confirmarPassword)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Usuario y contraseña son obligatorios.");
                return View();
            }

            if (password != confirmarPassword)
            {
                ModelState.AddModelError("", "Las contraseñas no coinciden.");
                return View();
            }

            bool existeUsuario = _db.Usuarios.Any(u => u.Username == username);
            if (existeUsuario)
            {
                ModelState.AddModelError("", "Ese nombre de usuario ya está en uso.");
                return View();
            }

            var nuevoUsuario = new Usuario
            {
                Username = username,
                Email = email,
                PasswordHash = Usuario.HashPassword(password),
                Rol = "Cliente"
            };

            _db.Usuarios.Add(nuevoUsuario);
            _db.SaveChanges();

            Session["UsuarioId"] = nuevoUsuario.Id;
            Session["Username"] = nuevoUsuario.Username;
            Session["Rol"] = nuevoUsuario.Rol;

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult AccesoDenegado()
        {
            return View();
        }
    }
}