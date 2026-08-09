using System.Linq;
using System.Net;
using System.Web.Mvc;
using Proyecto.infrastructure.DbContexts;
using Proyecto.Models.Entities;

namespace Proyecto.Controllers
{
    public class SucursalesController : BaseController
    {
        private readonly ConcesionarioDbContext _db;

        public SucursalesController()
        {
            _db = new ConcesionarioDbContext();
        }

        // GET: Sucursales
        public ActionResult Index()
        {
            var sucursales = _db.Sucursales.ToList();
            return View(sucursales);
        }

        // GET: Sucursales/Details/5
        public ActionResult Details(int id)
        {
            var sucursal = _db.Sucursales.Find(id);
            if (sucursal == null)
            {
                return HttpNotFound();
            }
            return View(sucursal);
        }

        // GET: Sucursales/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sucursales/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Sucursal sucursal)
        {
            if (ModelState.IsValid)
            {
                _db.Sucursales.Add(sucursal);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sucursal);
        }

        // GET: Sucursales/Edit/5
        public ActionResult Edit(int id)
        {
            var sucursal = _db.Sucursales.Find(id);
            if (sucursal == null)
            {
                return HttpNotFound();
            }
            return View(sucursal);
        }

        // POST: Sucursales/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Sucursal sucursal)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(sucursal).State = System.Data.Entity.EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sucursal);
        }

        // GET: Sucursales/Delete/5
        public ActionResult Delete(int id)
        {
            var sucursal = _db.Sucursales.Find(id);
            if (sucursal == null)
            {
                return HttpNotFound();
            }
            return View(sucursal);
        }

        // POST: Sucursales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var sucursal = _db.Sucursales.Find(id);
            if (sucursal != null)
            {
                try
                {
                    _db.Sucursales.Remove(sucursal);
                    _db.SaveChanges();
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException)
                {
                    TempData["Error"] = "No se puede eliminar esta sucursal porque tiene vehículos asignados. Primero reasigná o eliminá esos vehículos.";
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
    }
}