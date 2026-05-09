using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniMarket.Web.Data;
using MiniMarket.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniMarket.Web.Controllers
{
    [Authorize]
    public class VentasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VentasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Ventas (Historial)
        public async Task<IActionResult> Index()
        {
            var ventas = await _context.Ventas
                .Include(v => v.Usuario)
                .Include(v => v.Cliente)
                .OrderByDescending(v => v.Fecha)
                .ToListAsync();
            return View(ventas);
        }

        // GET: Punto de Venta
        public IActionResult Create()
        {
            // --- CANDADO DE SEGURIDAD 1: Validar Caja Abierta ---
            // Verificamos si este usuario tiene una caja con Estado = true
            var cajaAbierta = _context.AperturasCaja
                .Any(c => c.UsuarioId == User.Identity.Name && c.Estado == true);

            if (!cajaAbierta)
            {
                // Si está cerrada, lo mandamos al módulo de Caja con un mensaje
                TempData["MensajeError"] = "⚠️ ¡Atención! Debes ABRIR CAJA antes de poder vender.";
                return RedirectToAction("Index", "Caja");
            }
            // ----------------------------------------------------

            return View();
        }

        // API: Buscar Productos
        [HttpGet]
        public async Task<IActionResult> BuscarProductos(string term)
        {
            if (string.IsNullOrEmpty(term)) return Json(new { results = new object[] { } });

            var productos = await _context.Productos
                .Where(p => p.Estado == true && (p.Nombre.Contains(term) || p.CodigoBarras.Contains(term)))
                .Select(p => new
                {
                    id = p.Id,
                    text = p.Nombre + " | Stock: " + p.Stock,
                    precio = p.Precio,
                    stock = p.Stock
                })
                .Take(10)
                .ToListAsync();

            return Json(new { results = productos });
        }

        // API: Buscar Clientes
        [HttpGet]
        public async Task<IActionResult> BuscarClientes(string term)
        {
            if (string.IsNullOrEmpty(term)) return Json(new { results = new object[] { } });

            var clientes = await _context.Clientes
                .Where(c => c.Nombre.Contains(term) || c.Documento.Contains(term))
                .Select(c => new
                {
                    id = c.Id,
                    text = c.Nombre
                })
                .Take(10)
                .ToListAsync();

            return Json(new { results = clientes });
        }

        // POST: Registrar Venta
        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] VentaRequest request)
        {
            // --- CANDADO DE SEGURIDAD 2: Doble verificación ---
            var cajaAbierta = _context.AperturasCaja
                .Any(c => c.UsuarioId == User.Identity.Name && c.Estado == true);

            if (!cajaAbierta)
            {
                return Json(new { exito = false, mensaje = "⛔ LA CAJA ESTÁ CERRADA. No se puede procesar la venta." });
            }
            // --------------------------------------------------

            if (request == null || request.Detalles == null || request.Detalles.Count == 0)
            {
                return Json(new { exito = false, mensaje = "Datos inválidos" });
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // 1. Crear Venta
                    var nuevaVenta = new Venta
                    {
                        Fecha = DateTime.Now,
                        Total = request.Total,
                        UsuarioId = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name)?.Id,
                        ClienteId = request.ClienteId
                    };

                    _context.Ventas.Add(nuevaVenta);
                    await _context.SaveChangesAsync();

                    // 2. Procesar Detalles y Stock
                    foreach (var item in request.Detalles)
                    {
                        var producto = await _context.Productos.FindAsync(item.ProductoId);
                        if (producto == null) throw new Exception("Producto no encontrado");

                        if (producto.Stock < item.Cantidad)
                        {
                            throw new Exception($"Stock insuficiente para {producto.Nombre}");
                        }

                        // Restar Stock
                        producto.Stock -= item.Cantidad;

                        // Guardar Detalle Venta
                        var detalle = new DetalleVenta
                        {
                            VentaId = nuevaVenta.Id,
                            ProductoId = item.ProductoId,
                            Cantidad = item.Cantidad,
                            PrecioUnitario = item.Precio,
                            SubTotal = item.Total
                        };
                        _context.DetalleVentas.Add(detalle);

                        // Kardex (Salida)
                        var kardex = new MovimientoInventario
                        {
                            Fecha = DateTime.Now,
                            ProductoId = item.ProductoId,
                            TipoMovimiento = "SALIDA",
                            Cantidad = item.Cantidad,
                            Usuario = User.Identity.Name,
                            Referencia = $"Venta #{nuevaVenta.Id}" 
                        };
                        _context.MovimientosInventario.Add(kardex);
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Json(new { exito = true, mensaje = "Venta registrada correctamente", idVenta = nuevaVenta.Id });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Json(new { exito = false, mensaje = ex.Message });
                }
            }
        }

        // GET: Ticket de Venta
        public async Task<IActionResult> Ticket(int id)
        {
            var venta = await _context.Ventas
                .Include(v => v.Usuario)
                .Include(v => v.Cliente)
                .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (venta == null)
            {
                return NotFound();
            }

            // OBTENER CONFIGURACIÓN PARA EL TICKET
            var config = await _context.Configuraciones.FirstOrDefaultAsync();
            
            ViewBag.Configuracion = config ?? new Configuracion 
            { 
                NombreEmpresa = "EMPRESA NO CONFIGURADA", 
                Ruc = "00000000000", 
                Direccion = "Configure el sistema" 
            };
            
            return View(venta);
        }
    }

    // Clases auxiliares para recibir el JSON
    public class VentaRequest
    {
        public int? ClienteId { get; set; }
        public decimal Total { get; set; }
        public List<DetalleRequest> Detalles { get; set; }
    }

    public class DetalleRequest
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
    }
}