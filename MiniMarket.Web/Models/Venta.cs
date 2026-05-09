using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniMarket.Web.Models
{
    public class Venta
    {
        [Key]
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;

        public int? ClienteId { get; set; }
        [ForeignKey("ClienteId")]
        public virtual Cliente Cliente { get; set; } // Nombre corregido para coincidir con Controller

        public string UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public virtual IdentityUser Usuario { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        public virtual List<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();
    }
}