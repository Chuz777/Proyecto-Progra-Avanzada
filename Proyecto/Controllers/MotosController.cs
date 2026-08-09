using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto.infrastructure.DbContexts;
using Proyecto.infrastructure.Services;
using Proyecto.Models.DTOs;

namespace Proyecto.Controllers
{
    public class MotosController : BaseController
    {
        private readonly IVehiculoService _vehiculoService;
        private readonly ConcesionarioDbContext _db;

        public MotosController()
        {
            _vehiculoService = new VehiculoService();
            _db = new ConcesionarioDbContext();
        }

        // GET: Motos
        public ActionResult Index()
        {
            var vehiculos = _vehiculoService.ObtenerPorTipo(esMoto: true);
            return View(vehiculos ?? new List<VehiculoDTO>());
        }

        // GET: Motos/Details/5
        public ActionResult Details(int id)
        {
            var vehiculo = _vehiculoService.ObtenerPorId(id);
            if (vehiculo == null)
            {
                return HttpNotFound();
            }
            return View(vehiculo);
        }

        // GET: Motos/Create
        public ActionResult Create()
        {
            CargarSelectLists();
            return View();
        }

        // POST: Motos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateVehiculoDTO dto)
        {
            if (ModelState.IsValid)
            {
                var resultado = _vehiculoService.CrearVehiculo(dto);
                if (resultado != null && resultado.Success)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Error al guardar la moto.");
            }

            CargarSelectLists(dto.CategoriaId, dto.SucursalId);
            return View(dto);
        }

        // GET: Motos/Edit/5
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

        // POST: Motos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VehiculoDTO dto)
        {
            if (ModelState.IsValid)
            {
                var resultado = _vehiculoService.ActualizarVehiculo(dto);
                if (resultado != null && resultado.Success)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Error al actualizar la moto.");
            }

            CargarSelectLists(dto.CategoriaId, dto.SucursalId);
            return View(dto);
        }

        // GET: Motos/Delete/5
        public ActionResult Delete(int id)
        {
            var vehiculo = _vehiculoService.ObtenerPorId(id);
            if (vehiculo == null)
            {
                return HttpNotFound();
            }
            return View(vehiculo);
        }

        // POST: Motos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var resultado = _vehiculoService.EliminarVehiculo(id);
            if (resultado != null && resultado.Success)
            {
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Error al eliminar la moto.";
            return RedirectToAction("Index");
        }

        private void CargarSelectLists(int? categoriaId = null, int? sucursalId = null)
        {
            var categoriasMoto = _db.Categorias.Where(c => c.Nombre == "Moto").ToList();
            ViewBag.CategoriaId = new SelectList(categoriasMoto, "Id", "Nombre", categoriaId);
            ViewBag.SucursalId = new SelectList(_db.Sucursales.ToList(), "Id", "Nombre", sucursalId);
        }
    }
}