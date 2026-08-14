using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Proyecto.infrastructure.DbContexts;
using Proyecto.Models.Entities;

namespace Proyecto.Controllers
{
    public class ReservasController : Controller
    {
        private ConcesionarioDbContext _context = new ConcesionarioDbContext();


        // GET: Reservas/Create/5
        public ActionResult Create(int? vehiculoId)
        {
            if (vehiculoId == null)
            {
                return RedirectToAction("Index", "Vehiculo");
            }

            var vehiculo = _context.Vehiculos.Find(vehiculoId);
            if (vehiculo == null)
            {
                return HttpNotFound();
            }

            ViewBag.Vehiculo = vehiculo;

            var model = new ReservaVisita
            {
                VehiculoId = vehiculo.Id,
                FechaVista = DateTime.Now.AddDays(1)
            };

            return View(model);
        }

        // POST: Reservas/Create (Procesado por el Cliente)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReservaVisita reserva)
        {
            reserva.EstadoReserva = "Pendiente";

            // Asignar ID de usuario si está logueado en la sesión
            if (Session["UsuarioId"] != null)
            {
                reserva.UsuarioId = Convert.ToInt32(Session["UsuarioId"]);
            }

            // CLAVE: Evita que el ModelBinder falle por las relaciones de EF vacías
            ModelState.Remove("Vehiculo");
            ModelState.Remove("Usuario");
            ModelState.Remove("EstadoReserva");

            if (ModelState.IsValid)
            {
                _context.ReservasVisitas.Add(reserva);
                _context.SaveChanges();

                return RedirectToAction("Details", new { id = reserva.Id });
            }

            // Si falla la validación, recargamos el vehículo y la vista
            ViewBag.Vehiculo = _context.Vehiculos.Find(reserva.VehiculoId);
            return View(reserva);
        }

        // GET: Reservas/Details/5 (Vista de confirmación para el cliente)
        public ActionResult Details(int? id)
        {
            if (id == null) return HttpNotFound();

            var reserva = _context.ReservasVisitas
                .Include(r => r.Vehiculo)
                .FirstOrDefault(r => r.Id == id);

            if (reserva == null) return HttpNotFound();

            return View(reserva);
        }

      
        public ActionResult Index()
        {
            var reservas = _context.ReservasVisitas
                .Include(r => r.Vehiculo)
                .OrderByDescending(r => r.FechaVista)
                .ToList();

            return View(reservas);
        }

        // POST: Reservas/CambiarEstado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CambiarEstado(int id, string nuevoEstado)
        {
            var reserva = _context.ReservasVisitas.Find(id);
            if (reserva != null)
            {
                reserva.EstadoReserva = nuevoEstado;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // POST: Reservas/Delete/5 (Borrar reserva)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var reserva = _context.ReservasVisitas.Find(id);
            if (reserva != null)
            {
                _context.ReservasVisitas.Remove(reserva);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}