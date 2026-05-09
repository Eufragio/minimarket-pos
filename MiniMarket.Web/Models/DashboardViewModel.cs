using System.Collections.Generic;

namespace MiniMarket.Web.Models
{
    public class DashboardViewModel
    {
        // Tarjetas Superiores
        public decimal VentasHoy { get; set; }
        public int TotalProductos { get; set; }
        public int TotalCategorias { get; set; }
        public decimal CapitalEstimado { get; set; } // Nuevo: Valor total del inventario

        // Gráfico de Líneas (Tendencia)
        public List<string> FechasGrafico { get; set; }
        public List<decimal> VentasGrafico { get; set; }

        // Gráfico Circular (Top Productos)
        public List<string> TopProductosNombres { get; set; }
        public List<int> TopProductosCantidades { get; set; }
    }
}