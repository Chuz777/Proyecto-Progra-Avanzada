using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Models.Entities
{
    public class Cotizador
    {
        [Key]
        public int Id { get; set; }

        // --- Propiedades para vincular el vehículo ---
        public int? VehiculoId { get; set; }

        [ForeignKey("VehiculoId")]
        public virtual Vehiculo Vehiculo { get; set; }

        // --- Datos opcionales del cliente ---
        [Display(Name = "Nombre del Cliente")]
        public string NombreCliente { get; set; }

        [Display(Name = "Correo Electrónico")]
        public string EmailCliente { get; set; }

        // --- Propiedades financieras de la cotización ---
        [Required]
        [Display(Name = "Precio Final")]
        public decimal PrecioFinal { get; set; }

        [Required]
        [Display(Name = "Prima Sugerida")]
        public decimal PrimaSugerida { get; set; }

        [Required]
        [Range(12, 84, ErrorMessage = "Los plazos usuales van de 12 a 84 meses.")]
        [Display(Name = "Plazo en Meses")]
        public int PlazoMeses { get; set; }

        [Required]
        public DateTime FechaCotizacion { get; set; } = DateTime.Now;

        // Matemáticas para los plazos
        public decimal CalcularCuotaMensual(decimal tasaInteresAnual = 8.5m)
        {
            decimal montoFinanciar = PrecioFinal - PrimaSugerida;
            if (montoFinanciar <= 0 || PlazoMeses <= 0) return 0;

            decimal tasaMensual = (tasaInteresAnual / 100) / 12;
            double factor = Math.Pow((double)(1 + tasaMensual), PlazoMeses);

            return montoFinanciar * (tasaMensual * (decimal)factor) / (decimal)(factor - 1);
        }
    }
}