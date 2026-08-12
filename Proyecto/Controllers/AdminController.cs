using Proyecto.Filters;
using Proyecto.infrastructure.Services;
using System.Linq;
using System.Web.Mvc;

namespace Proyecto.Controllers
{
    [RequiereRol("Admin")]
    public class AdminController : BaseController
    {
        private readonly IUsuarioService _usuarioService;

        public AdminController()
        {
            _usuarioService = new UsuarioService();
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin/Usuarios
        public ActionResult Usuarios()
        {
            var usuarios = _usuarioService.ObtenerTodos();
            ViewBag.Roles = UsuarioService.RolesValidos;
            return View(usuarios);
        }

        // POST: Admin/CambiarRol
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CambiarRol(int id, string nuevoRol)
        {
            var resultado = _usuarioService.CambiarRol(id, nuevoRol);
            if (resultado == null || !resultado.Success)
            {
                TempData["Error"] = resultado?.Errors.FirstOrDefault() ?? "Error al cambiar el rol.";
            }
            return RedirectToAction("Usuarios");
        }

        // POST: Admin/EliminarUsuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarUsuario(int id)
        {
            var idUsuarioActual = (int?)Session["UsuarioId"] ?? 0;
            var resultado = _usuarioService.EliminarUsuario(id, idUsuarioActual);
            if (resultado == null || !resultado.Success)
            {
                TempData["Error"] = resultado?.Errors.FirstOrDefault() ?? "Error al eliminar el usuario.";
            }
            return RedirectToAction("Usuarios");
        }
    }
}