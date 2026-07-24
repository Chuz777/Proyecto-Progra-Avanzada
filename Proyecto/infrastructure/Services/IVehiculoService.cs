using System.Collections.Generic;
using Proyecto.Common;
using Proyecto.Models.DTOs;

namespace Proyecto.infrastructure.Services
{
    public interface IVehiculoService
    {
        IEnumerable<VehiculoDTO> ObtenerTodos();
        VehiculoDTO ObtenerPorId(int id);
        OperationResult CrearVehiculo(CreateVehiculoDTO dto);
        OperationResult ActualizarVehiculo(VehiculoDTO dto);
        OperationResult EliminarVehiculo(int id);
    }
}