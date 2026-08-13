using System.Web.Mvc;

namespace Proyecto.Controllers
{
    public class BaseController : Controller
    {
        protected string RolActual => Session["Rol"] as string;
        protected string UsuarioActual => Session["Username"] as string;

        protected bool EsAdmin => RolActual == "Admin";
        protected bool EsVendedor => RolActual == "Admin" || RolActual == "Asesor"; 

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.UsuarioActual = UsuarioActual;
            ViewBag.RolActual = RolActual;
            ViewBag.EsAdmin = EsAdmin;
            ViewBag.EsVendedor = EsVendedor;
        }
    }
}