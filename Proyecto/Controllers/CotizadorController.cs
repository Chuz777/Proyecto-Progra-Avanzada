using System;
using System.Web.Mvc;
using Proyecto.infrastructure.DbContexts;
using Proyecto.Models.Entities;
using Proyecto.Models.ViewModels;

namespace Proyecto.Controllers
{
    [AllowAnonymous]
    public class CotizadorController : Controller
    {
        private ConcesionarioDbContext db = new ConcesionarioDbContext();

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
                PrecioFinal = model.PrecioVehiculo,
                PrimaSugerida = model.Prima,
                PlazoMeses = model.PlazoMeses,
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

                db.Set<Cotizador>().Add(cotizador);
                db.SaveChanges();

                return RedirectToAction("Comprobante", new { id = cotizador.Id, cliente = model.NombreCliente, correo = model.EmailCliente });
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Comprobante(int id, string cliente, string correo)
        {
            var cotizacion = db.Set<Cotizador>().Find(id);
            if (cotizacion == null) return HttpNotFound();

            ViewBag.NombreCliente = cliente;
            ViewBag.EmailCliente = correo;

            return View(cotizacion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}