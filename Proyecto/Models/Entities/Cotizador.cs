using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proyecto.Models.Entities
{
    public class Cotizador
    {
        [Key]
        public int Id { get; set; }

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

        // Matematicas para los plazos
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