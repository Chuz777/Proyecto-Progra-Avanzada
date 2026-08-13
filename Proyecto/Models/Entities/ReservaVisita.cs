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

        [Required]
        public int VehiculoId { get; set; }
        [ForeignKey("VehiculoId")]
        public virtual Vehiculo Vehiculo { get; set; }

        [Required]
        public int UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }

        public void Confirmar() { this.EstadoReserva = "Confirmada"; }
        public void Cancelar() { this.EstadoReserva = "Cancelada"; }

    }
}
