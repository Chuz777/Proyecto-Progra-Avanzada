using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models.DTOs
{
    public class CreateVehiculoDTO
    {
        [Required(ErrorMessage = "La marca es obligatoria.")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "El modelo es obligatorio.")]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "El año es obligatorio.")]
        public int Anio { get; set; }

        [Required(ErrorMessage = "El VIN es obligatorio.")]
        public string VIN { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(1, 100000000, ErrorMessage = "Precio inválido.")]
        public decimal Precio { get; set; }

        public string ImagenUrl { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; }

        public string Estado { get; set; } = "Disponible";

        [Required(ErrorMessage = "Debe seleccionar una categoría.")]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una sucursal.")]
        public int SucursalId { get; set; }
    }
}