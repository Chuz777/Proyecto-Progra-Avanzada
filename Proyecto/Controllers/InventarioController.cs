using System.Web.Mvc;
using Proyecto.Filters;

namespace Proyecto.Controllers
{
    public class InventarioController : BaseController
    {
        // GET: Inventario
        public ActionResult Index()
        {
            return View("Inventario");
        }


        }
    }

