using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Proyecto.Models.Entities
{
    public class Vehiculo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "La marca es obligatoria.")]
        [StringLength(100)]
        public string Marca { get; set; }

        [Required(ErrorMessage = "El modelo es obligatorio.")]
        [StringLength(100)]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "El año es obligatorio.")]
        public int Anio { get; set; }

        [StringLength(50)]
        [Display(Name = "Número de VIN / Chasis")]
        public string VIN { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(1, 100000000, ErrorMessage = "Precio inválido.")]
        public decimal Precio { get; set; }

        [StringLength(250)]
        [Display(Name = "Ruta de la Imagen")]
        public string ImagenUrl { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(500)]
        public string Descripcion { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = "Disponible"; // "Disponible", "Reservado", "Vendido"

        // Relaciones DB
        [Required(ErrorMessage = "Debe asignar una categoría.")]
        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public virtual Categoria Categoria { get; set; }

        [Required(ErrorMessage = "Debe asignar una sucursal.")]
        public int SucursalId { get; set; }
        [ForeignKey("SucursalId")]
        public virtual Sucursal Sucursal { get; set; }

        // Lógica de dominio
        public void MarcarVendido() { this.Estado = "Vendido"; }
        public bool EstaDisponible() { return this.Estado == "Disponible"; }
        public decimal AplicarDescuento(decimal porcentaje)
        {
            return this.Precio - (this.Precio * (porcentaje / 100));
        }
    }
}