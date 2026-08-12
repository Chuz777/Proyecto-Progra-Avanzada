using System.Web.Mvc;
using Proyecto.Filters;

namespace Proyecto.Controllers
{
    [RequiereRol("Admin")]
    public class AdminController : BaseController
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
    }
}