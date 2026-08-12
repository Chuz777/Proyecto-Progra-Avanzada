using Proyecto.Common;
using Proyecto.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.infrastructure.Services
{
    public interface IUsuarioService
    {
        IEnumerable<UsuarioDTO> ObtenerTodos();
        UsuarioDTO ObtenerPorId(int id);
        OperationResult CambiarRol(int id, string nuevoRol);
        OperationResult EliminarUsuario(int id, int idUsuarioActual);
    }
}
