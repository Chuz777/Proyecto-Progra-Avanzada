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

            // 1. Si hay un usuario en sesión se asigna su ID
            if (Session["UsuarioId"] != null)
            {
                reserva.UsuarioId = Convert.ToInt32(Session["UsuarioId"]);
            }
            else
            {
                // 2. SOLUCIÓN AL ERROR DE SQL: Si es un visitante casual (sin sesión),
                // busca el primer usuario registrado en la BD para cumplir la restricción NOT NULL de SQL.
                var primerUsuario = _context.Usuarios.FirstOrDefault();
                if (primerUsuario != null)
                {
                    reserva.UsuarioId = primerUsuario.Id;
                }
            }

            // Remueve las propiedades del modelo para evitar que falle el ModelState.IsValid
            ModelState.Remove("Vehiculo");
            ModelState.Remove("Usuario");
            ModelState.Remove("EstadoReserva");
            ModelState.Remove("UsuarioId");

            if (ModelState.IsValid)
            {
                _context.ReservasVisitas.Add(reserva);
                _context.SaveChanges(); // <-- Guarda correctamente sin error de NULL

                return RedirectToAction("Detalles", new { id = reserva.Id });
            }

            // Si falla la validación, recargamos el vehículo y la vista
            ViewBag.Vehiculo = _context.Vehiculos.Find(reserva.VehiculoId);
            return View(reserva);
        }

        // GET: Reservas/Details/5 (Vista de confirmación para el cliente)
        public ActionResult Detalles(int? id)
        {
            if (id == null) return HttpNotFound();

            var reserva = _context.ReservasVisitas
                .Include(r => r.Vehiculo)
                .FirstOrDefault(r => r.Id == id);

            if (reserva == null) return HttpNotFound();

            return View(reserva);
        }

        // GET: Reservas (Panel de Administración)
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