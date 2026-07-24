using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models.Entities
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la categoría es requerido.")]
        [StringLength(50)]
        [Display(Name = "Categoría")]
        public string Nombre { get; set; }

        // Relación: Una categoría tiene muchos vehículos
        public virtual ICollection<Vehiculo> Vehiculos { get; set; }
    }
}