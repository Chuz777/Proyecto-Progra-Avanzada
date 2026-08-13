using System;
using System.Linq;
using System.Web.Mvc;
using Proyecto.infrastructure.DbContexts;
using Proyecto.Models.Entities;

namespace Proyecto.Controllers
{
    public class ReservasController : BaseController
    {
        private readonly ConcesionarioDbContext _db;

        public ReservasController()
        {
            _db = new ConcesionarioDbContext();
        }

        // GET: Reservas - Listado de vehículos disponibles para reservar (SIN login requerido)
        public ActionResult Index()
        {
            try
            {
                var vehiculos = _db.Vehiculos
                    .OrderBy(v => v.Marca)
                    .ToList();

                // Log para debugging
                System.Diagnostics.Debug.WriteLine($"[RESERVAS INDEX] Total de vehículos cargados: {vehiculos.Count}");
                foreach (var v in vehiculos)
                {
                    System.Diagnostics.Debug.WriteLine($"  - {v.Marca} {v.Modelo} (ID: {v.Id}, Estado: {v.Estado})");
                }

                return View(vehiculos);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[RESERVAS INDEX ERROR] {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[RESERVAS INDEX ERROR] {ex.InnerException?.Message}");
                throw;
            }
        }

        // GET: Reservas/Create - Formulario para crear reserva (SIN login requerido)
        public ActionResult Create(int vehiculoId)
        {
            var vehiculo = _db.Vehiculos.FirstOrDefault(v => v.Id == vehiculoId);
            if (vehiculo == null)
            {
                return HttpNotFound();
            }

            var reserva = new ReservaVisita
            {
                VehiculoId = vehiculoId
            };

            ViewBag.VehiculoId = vehiculoId;
            ViewBag.Vehiculo = vehiculo;
            return View(reserva);
        }

        // POST: Reservas/Create - Guardar reserva (SIN login requerido)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReservaVisita reserva, string NombreContacto, string EmailContacto, string TelefonoContacto)
        {
            var vehiculo = _db.Vehiculos.FirstOrDefault(v => v.Id == reserva.VehiculoId);
            if (vehiculo == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                // Validar que la fecha de visita sea en el futuro
                if (reserva.FechaVista <= DateTime.Now)
                {
                    ModelState.AddModelError("FechaVista", "La fecha de la cita debe ser en el futuro.");
                }
                else if (string.IsNullOrWhiteSpace(NombreContacto))
                {
                    ModelState.AddModelError("NombreContacto", "El nombre es obligatorio.");
                }
                else if (string.IsNullOrWhiteSpace(EmailContacto))
                {
                    ModelState.AddModelError("EmailContacto", "El email es obligatorio.");
                }
                else if (string.IsNullOrWhiteSpace(TelefonoContacto))
                {
                    ModelState.AddModelError("TelefonoContacto", "El teléfono es obligatorio.");
                }
                else
                {
                    // Si el usuario está autenticado, usar su ID
                    if (!string.IsNullOrEmpty(UsuarioActual))
                    {
                        var usuarioActual = _db.Usuarios.FirstOrDefault(u => u.Username == UsuarioActual);
                        if (usuarioActual != null)
                        {
                            reserva.UsuarioId = usuarioActual.Id;
                        }
                        else
                        {
                            // Crear usuario temporal o usar datos de contacto
                            var usuarioTemporal = new Usuario
                            {
                                Username = EmailContacto,
                                Email = EmailContacto,
                                Rol = "Cliente"
                            };
                            _db.Usuarios.Add(usuarioTemporal);
                            _db.SaveChanges();
                            reserva.UsuarioId = usuarioTemporal.Id;
                        }
                    }
                    else
                    {
                        // Usuario no autenticado: buscar o crear usuario con los datos de contacto
                        var usuarioExistente = _db.Usuarios.FirstOrDefault(u => u.Email == EmailContacto);
                        if (usuarioExistente != null)
                        {
                            reserva.UsuarioId = usuarioExistente.Id;
                        }
                        else
                        {
                            var usuarioNuevo = new Usuario
                            {
                                Username = EmailContacto,
                                Email = EmailContacto,
                                Rol = "Cliente"
                            };
                            _db.Usuarios.Add(usuarioNuevo);
                            _db.SaveChanges();
                            reserva.UsuarioId = usuarioNuevo.Id;
                        }
                    }

                    reserva.EstadoReserva = "Pendiente";

                    _db.ReservasVisitas.Add(reserva);
                    _db.SaveChanges();

                    return RedirectToAction("Confirmacion", new { id = reserva.Id });
                }
            }

            // Si hay error, volver al formulario
            ViewBag.VehiculoId = reserva.VehiculoId;
            ViewBag.Vehiculo = vehiculo;

            return View(reserva);
        }

        // GET: Reservas/Confirmacion/5
        public ActionResult Confirmacion(int id)
        {
            var reserva = _db.ReservasVisitas.FirstOrDefault(r => r.Id == id);
            if (reserva == null)
            {
                return HttpNotFound();
            }

            return View(reserva);
        }

        // GET: Reservas/Detalles/5 - Requiere login
        public ActionResult Detalles(int id)
        {
            if (string.IsNullOrEmpty(UsuarioActual))
            {
                return RedirectToAction("Login", "Account");
            }

            var reserva = _db.ReservasVisitas.FirstOrDefault(r => r.Id == id);
            if (reserva == null)
            {
                return HttpNotFound();
            }

            var usuarioActual = _db.Usuarios.FirstOrDefault(u => u.Username == UsuarioActual);
            if (usuarioActual == null || (reserva.UsuarioId != usuarioActual.Id && RolActual != "Admin"))
            {
                return new HttpUnauthorizedResult();
            }

            return View(reserva);
        }

        // GET: Reservas/MisReservas - Listar reservas del usuario autenticado
        public ActionResult MisReservas()
        {
            if (string.IsNullOrEmpty(UsuarioActual))
            {
                return RedirectToAction("Login", "Account");
            }

            var usuarioActual = _db.Usuarios.FirstOrDefault(u => u.Username == UsuarioActual);
            if (usuarioActual == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var reservas = _db.ReservasVisitas
                .Where(r => r.UsuarioId == usuarioActual.Id)
                .OrderByDescending(r => r.FechaVista)
                .ToList();

            return View("Index", reservas);
        }

        // POST: Reservas/Cancelar/5 - Requiere login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancelar(int id)
        {
            if (string.IsNullOrEmpty(UsuarioActual))
            {
                return RedirectToAction("Login", "Account");
            }

            var usuarioActual = _db.Usuarios.FirstOrDefault(u => u.Username == UsuarioActual);
            if (usuarioActual == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var reserva = _db.ReservasVisitas.FirstOrDefault(r => r.Id == id);
            if (reserva == null)
            {
                return HttpNotFound();
            }

            // Verificar que sea el propietario o un admin
            if (reserva.UsuarioId != usuarioActual.Id && RolActual != "Admin")
            {
                return new HttpUnauthorizedResult();
            }

            // Cambiar estado a Cancelada
            reserva.Cancelar();
            _db.SaveChanges();

            return RedirectToAction("MisReservas");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
