using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Proyecto.Models.Entities
{
    public class ReservaVisita
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha de visita es obligatoria.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha y Hora de la Visita")]
        public DateTime FechaVista { get; set; }

        [Required]
        [StringLength(20)]
        public string EstadoReserva { get; set; } = "Pendiente"; // "Pendiente", "Confirmada", "Cancelada"


        [Required(ErrorMessage = "El nombre de contacto es obligatorio.")]
        [StringLength(100)]
        [Display(Name = "Nombre Completo")]
        public string NombreContacto { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "Correo electrónico inválido.")]
        [StringLength(100)]
        [Display(Name = "Correo Electrónico")]
        public string EmailContacto { get; set; }

        [Required(ErrorMessage = "El teléfono de contacto es obligatorio.")]
        [StringLength(20)]
        [Display(Name = "Teléfono")]
        public string TelefonoContacto { get; set; }


        [Required]
        public int VehiculoId { get; set; }
        [ForeignKey("VehiculoId")]
        public virtual Vehiculo Vehiculo { get; set; }

     
        public int? UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }

        public void Confirmar() { this.EstadoReserva = "Confirmada"; }
        public void Cancelar() { this.EstadoReserva = "Cancelada"; }
    }
}