using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniMarket.Web.Data;
using MiniMarket.Web.Models;
using System.Threading.Tasks;

namespace MiniMarket.Web.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ConfiguracionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConfiguracionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Muestra el formulario con los datos actuales
        public async Task<IActionResult> Index()
        {
            var configEntidad = await _context.Configuraciones.FirstOrDefaultAsync();
            var model = new ConfiguracionViewModel();

            if (configEntidad != null)
            {
                // Cargar datos de la BD al formulario
                model.NombreEmpresa = configEntidad.NombreEmpresa;
                model.Ruc = configEntidad.Ruc;
                model.Direccion = configEntidad.Direccion;
                model.Telefono = configEntidad.Telefono;
                model.EmailContacto = configEntidad.EmailContacto;
                model.IgvPorcentaje = configEntidad.IgvPorcentaje;
                model.MonedaSimbolo = configEntidad.MonedaSimbolo;
            }
            else
            {
                // Valores por defecto si es la primera vez
                model.NombreEmpresa = "Mi MiniMarket";
                model.Ruc = "00000000000";
                model.IgvPorcentaje = 18;
            }

            return View(model);
        }

        // POST: Guarda los cambios
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Guardar(ConfiguracionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var configEntidad = await _context.Configuraciones.FirstOrDefaultAsync();

                if (configEntidad == null)
                {
                    // Si no existe, CREAMOS uno nuevo
                    configEntidad = new Configuracion
                    {
                        NombreEmpresa = model.NombreEmpresa,
                        Ruc = model.Ruc,
                        Direccion = model.Direccion,
                        Telefono = model.Telefono,
                        EmailContacto = model.EmailContacto,
                        IgvPorcentaje = model.IgvPorcentaje,
                        MonedaSimbolo = model.MonedaSimbolo
                    };
                    _context.Configuraciones.Add(configEntidad);
                }
                else
                {
                    // Si ya existe, ACTUALIZAMOS los datos
                    configEntidad.NombreEmpresa = model.NombreEmpresa;
                    configEntidad.Ruc = model.Ruc;
                    configEntidad.Direccion = model.Direccion;
                    configEntidad.Telefono = model.Telefono;
                    configEntidad.EmailContacto = model.EmailContacto;
                    configEntidad.IgvPorcentaje = model.IgvPorcentaje;
                    configEntidad.MonedaSimbolo = model.MonedaSimbolo;

                    _context.Configuraciones.Update(configEntidad);
                }

                await _context.SaveChangesAsync();
                
                TempData["MensajeExito"] = "¡Configuración guardada correctamente en la Base de Datos!";
                return View("Index", model);
            }

            return View("Index", model);
        }
    }
}