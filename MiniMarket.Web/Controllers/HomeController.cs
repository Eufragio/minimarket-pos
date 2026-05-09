using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniMarket.Web.Data;
using MiniMarket.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MiniMarket.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var hoyInicio = DateTime.Today;
            var hoyFin = DateTime.Today.AddDays(1).AddTicks(-1);

            // 1. TARJETAS SUPERIORES
            var ventasHoy = await _context.Ventas
                .Where(v => v.Fecha >= hoyInicio && v.Fecha <= hoyFin)
                .SumAsync(v => v.Total);

            var totalProductos = await _context.Productos.CountAsync(p => p.Estado == true);
            var totalCategorias = await _context.Categorias.CountAsync();
            
            // Capital Estimado: Suma de (Precio * Stock) de todo el inventario
            var capitalEstimado = await _context.Productos
                .Where(p => p.Estado == true)
                .SumAsync(p => p.Precio * p.Stock);

            // 2. GRÁFICO DE LÍNEAS (Últimos 7 días)
            var fechaInicioGrafico = DateTime.Today.AddDays(-6);
            var ventasUltimos7Dias = await _context.Ventas
                .Where(v => v.Fecha >= fechaInicioGrafico)
                .GroupBy(v => v.Fecha.Date)
                .Select(g => new { Fecha = g.Key, Total = g.Sum(v => v.Total) })
                .ToListAsync();

            var etiquetasFechas = new List<string>();
            var valoresVentas = new List<decimal>();

            for (int i = 0; i < 7; i++)
            {
                var fechaActual = fechaInicioGrafico.AddDays(i);
                var ventaDia = ventasUltimos7Dias.FirstOrDefault(v => v.Fecha == fechaActual);
                etiquetasFechas.Add(fechaActual.ToString("ddd dd", new CultureInfo("es-ES"))); 
                valoresVentas.Add(ventaDia?.Total ?? 0);
            }

            // 3. GRÁFICO CIRCULAR (TOP 5 PRODUCTOS)
            var topProductos = await _context.DetalleVentas
                .Include(d => d.Producto)
                .GroupBy(d => d.Producto.Nombre)
                .Select(g => new { 
                    Producto = g.Key, 
                    Cantidad = g.Sum(d => d.Cantidad) 
                })
                .OrderByDescending(x => x.Cantidad)
                .Take(5)
                .ToListAsync();

            var modelo = new DashboardViewModel
            {
                VentasHoy = ventasHoy,
                TotalProductos = totalProductos,
                TotalCategorias = totalCategorias,
                CapitalEstimado = capitalEstimado,
                FechasGrafico = etiquetasFechas,
                VentasGrafico = valoresVentas,
                TopProductosNombres = topProductos.Select(x => x.Producto).ToList(),
                TopProductosCantidades = topProductos.Select(x => x.Cantidad).ToList()
            };

            return View(modelo);
        }
    }
}