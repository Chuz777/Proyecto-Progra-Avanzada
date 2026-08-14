using System;
using System.Linq;
using System.Web.Mvc;
using Proyecto.infrastructure.DbContexts;
using Proyecto.Models.Entities;
using Proyecto.Models.ViewModels;

namespace Proyecto.Controllers
{
    [AllowAnonymous]
    public class CotizadorController : Controller
    {
        private readonly ConcesionarioDbContext db = new ConcesionarioDbContext();

        // GET: Cotizador
        // Muestra el listado/historial para el Administrador y Vendedor
        [HttpGet]
        public ActionResult Index()
        {
            var cotizaciones = db.Cotizaciones
                                 .OrderByDescending(c => c.FechaCotizacion)
                                 .ToList();

            return View(cotizaciones);
        }

        [HttpGet]
        public ActionResult Calcular(int vehiculoId)
        {
            var vehiculo = db.Vehiculos.Find(vehiculoId);
            if (vehiculo == null) return HttpNotFound();

            var model = new CotizacionViewModel
            {
                VehiculoId = vehiculo.Id,
                Marca = vehiculo.Marca,
                Modelo = vehiculo.Modelo,
                ImagenUrl = vehiculo.ImagenUrl,
                PrecioVehiculo = vehiculo.Precio,
                Prima = vehiculo.Precio * 0.20m,
                PlazoMeses = 48,
                TasaAnual = 8.5m
            };

            var cotizador = new Cotizador
            {
                PrecioFinal = model.PrecioVehiculo,
                PrimaSugerida = model.Prima,
                PlazoMeses = model.PlazoMeses
            };

            model.CuotaMensual = cotizador.CalcularCuotaMensual(model.TasaAnual);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Calcular(CotizacionViewModel model, string accion)
        {
            var cotizador = new Cotizador
            {
                VehiculoId = model.VehiculoId,
                PrecioFinal = model.PrecioVehiculo,
                PrimaSugerida = model.Prima,
                PlazoMeses = model.PlazoMeses,
                NombreCliente = model.NombreCliente,
                EmailCliente = model.EmailCliente,
                FechaCotizacion = DateTime.Now
            };

            model.CuotaMensual = cotizador.CalcularCuotaMensual(model.TasaAnual);

            if (accion == "guardar")
            {
                if (string.IsNullOrWhiteSpace(model.NombreCliente) || string.IsNullOrWhiteSpace(model.EmailCliente))
                {
                    ModelState.AddModelError("", "Debe ingresar su Nombre y Correo para guardar y generar la hoja de cotización.");
                    return View(model);
                }

                db.Cotizaciones.Add(cotizador);
                db.SaveChanges();

                return RedirectToAction("Comprobante", new { id = cotizador.Id });
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Comprobante(int id)
        {
            var cotizacion = db.Cotizaciones.Find(id);
            if (cotizacion == null) return HttpNotFound();

            return View(cotizacion);
        }

        // POST: Cotizador/Eliminar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id)
        {
            var cotizacion = db.Cotizaciones.Find(id);
            if (cotizacion != null)
            {
                db.Cotizaciones.Remove(cotizacion);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}