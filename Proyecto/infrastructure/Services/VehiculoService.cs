using System;
using System.Collections.Generic;
using System.Linq;
using Proyecto.Common;
using Proyecto.infrastructure.DbContexts;
using Proyecto.Models.DTOs;
using Proyecto.Models.Entities;

namespace Proyecto.infrastructure.Services
{
    public class VehiculoService : IVehiculoService
    {
        private readonly ConcesionarioDbContext _db;

        public VehiculoService()
        {
            _db = new ConcesionarioDbContext();
        }

        public IEnumerable<VehiculoDTO> ObtenerTodos()
        {
            return _db.Vehiculos.Select(v => new VehiculoDTO
            {
                Id = v.Id,
                Marca = v.Marca,
                Modelo = v.Modelo,
                Anio = v.Anio,
                VIN = v.VIN,
                Precio = v.Precio,
                ImagenUrl = v.ImagenUrl,
                Descripcion = v.Descripcion,
                Estado = v.Estado,
                CategoriaId = v.CategoriaId,
                SucursalId = v.SucursalId
            }).ToList();
        }

        public VehiculoDTO ObtenerPorId(int id)
        {
            var vehiculo = _db.Vehiculos.Find(id);
            if (vehiculo == null) return null;

            return new VehiculoDTO
            {
                Id = vehiculo.Id,
                Marca = vehiculo.Marca,
                Modelo = vehiculo.Modelo,
                Anio = vehiculo.Anio,
                VIN = vehiculo.VIN,
                Precio = vehiculo.Precio,
                ImagenUrl = vehiculo.ImagenUrl,
                Descripcion = vehiculo.Descripcion,
                Estado = vehiculo.Estado,
                CategoriaId = vehiculo.CategoriaId,
                SucursalId = vehiculo.SucursalId
            };
        }

        public OperationResult CrearVehiculo(CreateVehiculoDTO dto)
        {
            try
            {
                bool existeVin = _db.Vehiculos.Any(v => v.VIN == dto.VIN);
                if (existeVin)
                {
                    return OperationResult.Fail("El número de chasis (VIN) ya se encuentra registrado.");
                }

                var nuevoVehiculo = new Vehiculo
                {
                    Marca = dto.Marca,
                    Modelo = dto.Modelo,
                    Anio = dto.Anio,
                    VIN = dto.VIN,
                    Precio = dto.Precio,
                    ImagenUrl = dto.ImagenUrl,
                    Descripcion = dto.Descripcion,
                    Estado = string.IsNullOrEmpty(dto.Estado) ? "Disponible" : dto.Estado,
                    CategoriaId = dto.CategoriaId,
                    SucursalId = dto.SucursalId
                };

                _db.Vehiculos.Add(nuevoVehiculo);
                _db.SaveChanges();

                return OperationResult.Ok();
            }
            catch (Exception ex)
            {
                return OperationResult.Fail("Error al crear el vehículo: " + ex.Message);
            }
        }

        public OperationResult ActualizarVehiculo(VehiculoDTO dto)
        {
            try
            {
                var vehiculoDb = _db.Vehiculos.Find(dto.Id);
                if (vehiculoDb == null)
                {
                    return OperationResult.Fail("El vehículo no existe.");
                }

                vehiculoDb.Marca = dto.Marca;
                vehiculoDb.Modelo = dto.Modelo;
                vehiculoDb.Anio = dto.Anio;
                vehiculoDb.VIN = dto.VIN;
                vehiculoDb.Precio = dto.Precio;
                vehiculoDb.ImagenUrl = dto.ImagenUrl;
                vehiculoDb.Descripcion = dto.Descripcion;
                vehiculoDb.Estado = dto.Estado;
                vehiculoDb.CategoriaId = dto.CategoriaId;
                vehiculoDb.SucursalId = dto.SucursalId;

                _db.SaveChanges();
                return OperationResult.Ok();
            }
            catch (Exception ex)
            {
                return OperationResult.Fail("Error al actualizar: " + ex.Message);
            }
        }

        public OperationResult EliminarVehiculo(int id)
        {
            try
            {
                var vehiculo = _db.Vehiculos.Find(id);
                if (vehiculo == null)
                {
                    return OperationResult.Fail("El vehículo a eliminar no existe.");
                }

                _db.Vehiculos.Remove(vehiculo);
                _db.SaveChanges();
                return OperationResult.Ok();
            }
            catch (Exception ex)
            {
                return OperationResult.Fail("Error al eliminar: " + ex.Message);
            }
        }
    }
}