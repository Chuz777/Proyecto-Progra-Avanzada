using System.Linq;
using System.Web.Mvc;

namespace Proyecto.Filters
{
    public class RequiereRolAttribute : ActionFilterAttribute
    {
        private readonly string[] _rolesPermitidos;

        public RequiereRolAttribute(params string[] rolesPermitidos)
        {
            _rolesPermitidos = rolesPermitidos;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var rol = filterContext.HttpContext.Session["Rol"] as string;

            if (rol == null)
            {
                var returnUrl = filterContext.HttpContext.Request.Url.PathAndQuery;
                filterContext.Result = new RedirectResult("~/Account/Login?returnUrl=" + returnUrl);
                return;
            }

            if (!_rolesPermitidos.Contains(rol))
            {
                filterContext.Result = new RedirectResult("~/Account/AccesoDenegado");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}