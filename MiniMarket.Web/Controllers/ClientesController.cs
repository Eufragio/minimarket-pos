using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniMarket.Web.Data;
using MiniMarket.Web.Models;

namespace MiniMarket.Web.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Clientes.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                // Evitar duplicados por Documento
                if (await _context.Clientes.AnyAsync(c => c.Documento == cliente.Documento))
                {
                    ModelState.AddModelError("Documento", "Ya existe un cliente con este documento.");
                    return View(cliente);
                }

                cliente.Estado = true;
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                
                TempData["Titulo"] = "¡Registrado!";
                TempData["Mensaje"] = "Cliente guardado exitosamente.";
                TempData["Tipo"] = "success";
                
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cliente cliente)
        {
            if (id != cliente.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                    TempData["Titulo"] = "¡Actualizado!";
                    TempData["Mensaje"] = "Datos del cliente actualizados.";
                    TempData["Tipo"] = "success";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Clientes.Any(e => e.Id == cliente.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        public async Task<IActionResult> CambiarEstado(int? id)
        {
            if (id == null) return NotFound();
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();

            cliente.Estado = !cliente.Estado;
            await _context.SaveChangesAsync();

            TempData["Titulo"] = cliente.Estado ? "¡Habilitado!" : "¡Inhabilitado!";
            TempData["Mensaje"] = "El estado del cliente ha cambiado.";
            TempData["Tipo"] = cliente.Estado ? "success" : "warning";

            return RedirectToAction(nameof(Index));
        }
    }
}