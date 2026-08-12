using System;
using System.Linq;
using System.Web.Mvc;
using Proyecto.infrastructure.DbContexts;
using Proyecto.infrastructure.Services;
using Proyecto.Models.DTOs;

namespace Proyecto.Controllers
{
    public class VehiculosController : BaseController
    {
        private readonly IVehiculoService _vehiculoService;
        private readonly ConcesionarioDbContext _db;

        public VehiculosController()
        {
            _vehiculoService = new VehiculoService();
            _db = new ConcesionarioDbContext();
        }

        // GET: Vehiculos/Details/5
        public ActionResult Details(int id)
        {
            var vehiculo = _vehiculoService.ObtenerPorId(id);
            if (vehiculo == null)
            {
                return HttpNotFound();
            }
            return View(vehiculo);
        }

        // GET: Vehiculos/Create
        public ActionResult Create()
        {
            CargarSelectLists();
            return View();
        }

        // POST: Vehiculos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateVehiculoDTO dto)
        {
            if (ModelState.IsValid)
            {
                var resultado = _vehiculoService.CrearVehiculo(dto);
                if (resultado != null && resultado.Success)
                {
                    return RedirectToAction("Index", DestinoSegunCategoria(dto.CategoriaId));
                }

                ModelState.AddModelError("", "Error al guardar el vehículo.");
            }

            CargarSelectLists(dto.CategoriaId, dto.SucursalId);
            return View(dto);
        }

        // GET: Vehiculos/Edit
        public ActionResult Edit(int id)
        {
            var vehiculo = _vehiculoService.ObtenerPorId(id);
            if (vehiculo == null)
            {
                return HttpNotFound();
            }

            CargarSelectLists(vehiculo.CategoriaId, vehiculo.SucursalId);
            return View(vehiculo);
        }

        // POST: Vehiculos/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VehiculoDTO dto)
        {
            if (ModelState.IsValid)
            {
                var resultado = _vehiculoService.ActualizarVehiculo(dto);
                if (resultado != null && resultado.Success)
                {
                    return RedirectToAction("Index", DestinoSegunCategoria(dto.CategoriaId));
                }

                ModelState.AddModelError("", "Error al actualizar el vehículo.");
            }

            CargarSelectLists(dto.CategoriaId, dto.SucursalId);
            return View(dto);
        }

        // GET: Vehiculos/Delete
        public ActionResult Delete(int id)
        {
            var vehiculo = _vehiculoService.ObtenerPorId(id);
            if (vehiculo == null)
            {
                return HttpNotFound();
            }
            return View(vehiculo);
        }

        // POST: Vehiculos/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var vehiculo = _vehiculoService.ObtenerPorId(id);
            var destino = vehiculo != null ? DestinoSegunCategoria(vehiculo.CategoriaId) : "Autos";

            var resultado = _vehiculoService.EliminarVehiculo(id);
            if (resultado == null || !resultado.Success)
            {
                TempData["Error"] = "Error al eliminar el vehículo.";
            }

            return RedirectToAction("Index", destino);
        }

        private string DestinoSegunCategoria(int categoriaId)
        {
            var categoria = _db.Categorias.Find(categoriaId);
            bool esMoto = categoria != null && categoria.Nombre == "Moto";
            return esMoto ? "Motos" : "Autos";
        }

        private void CargarSelectLists(int? categoriaId = null, int? sucursalId = null)
        {
            ViewBag.CategoriaId = new SelectList(_db.Categorias.ToList(), "Id", "Nombre", categoriaId);
            ViewBag.SucursalId = new SelectList(_db.Sucursales.ToList(), "Id", "Nombre", sucursalId);
        }
    }
}