using System.Web.Mvc;

namespace Proyecto.Controllers
{
    public class BaseController : Controller
    {
        protected string RolActual => Session["Rol"] as string;
        protected string UsuarioActual => Session["Username"] as string;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.UsuarioActual = UsuarioActual;
            ViewBag.RolActual = RolActual;
        }
    }
}