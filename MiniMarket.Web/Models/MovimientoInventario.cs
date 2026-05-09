using System;

namespace MiniMarket.Web.Models
{
    public class MovimientoInventario
    {
        public int Id { get; set; }
        
        public DateTime Fecha { get; set; }
        
        public int ProductoId { get; set; }
        public virtual Producto Producto { get; set; }

        public string TipoMovimiento { get; set; } // "ENTRADA" o "SALIDA"
        
        public int Cantidad { get; set; }
        
        public string Usuario { get; set; }

        // CORRECCIÓN: Usamos "Referencia" en lugar de "Detalle" 
        // para que coincida con ComprasController y Kardex/Index.cshtml
        public string Referencia { get; set; } 
    }
}