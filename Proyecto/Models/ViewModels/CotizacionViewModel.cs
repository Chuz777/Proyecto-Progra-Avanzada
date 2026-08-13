using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models.ViewModels
{
    public class CotizacionViewModel
    {
        public int VehiculoId { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string ImagenUrl { get; set; }
        public decimal PrecioVehiculo { get; set; }

        [Required(ErrorMessage = "La prima es requerida.")]
        [Display(Name = "Prima Sugerida")]
        public decimal Prima { get; set; }

        [Required]
        [Range(12, 84, ErrorMessage = "El plazo debe estar entre 12 y 84 meses.")]
        [Display(Name = "Plazo en Meses")]
        public int PlazoMeses { get; set; } = 48;

        public decimal TasaAnual { get; set; } = 8.5m;
        public decimal CuotaMensual { get; set; }

        [Display(Name = "Nombre Completo")]
        public string NombreCliente { get; set; }

        [EmailAddress(ErrorMessage = "Ingrese un correo válido.")]
        [Display(Name = "Correo Electrónico")]
        public string EmailCliente { get; set; }

        public int? CotizacionId { get; set; }
    }
}