using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Sucursal
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la sucursal es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(200)]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El teléfono de la sucursal es obligatorio.")]
        [StringLength(20)]
        public string Telefono { get; set; }

        // La sucursal despacha muchos vehículos
        public virtual ICollection<Vehiculo> Vehiculos { get; set; }

    }
}