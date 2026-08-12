using System.Web.Mvc;
using Proyecto.Filters;

namespace Proyecto.Controllers
{
    [RequiereRol("Admin", "Asesor")]
    public class VendedorController : BaseController
    {
        // GET: Vendedor
        public ActionResult Index()
        {
            return View();
        }
    }
}