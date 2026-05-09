using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiniMarket.Web.Data;
using MiniMarket.Web.Models;

namespace MiniMarket.Web.Controllers
{
    public class KardexController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KardexController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? productoId)
        {
            // Cargar lista de productos para el filtro
            ViewBag.Productos = new SelectList(_context.Productos, "Id", "Nombre");
            ViewBag.ProductoSeleccionado = productoId;

            // Si no hay producto seleccionado, mostrar vacío o todo (mejor vacío por rendimiento)
            if (productoId == null)
            {
                return View(new List<MovimientoInventario>());
            }

            // Filtrar movimientos del producto
            var movimientos = await _context.MovimientosInventario
                .Include(m => m.Producto)
                .Where(m => m.ProductoId == productoId)
                .OrderByDescending(m => m.Fecha) // Más reciente primero
                .ToListAsync();

            return View(movimientos);
        }
    }
}