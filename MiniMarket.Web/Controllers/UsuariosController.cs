using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiniMarket.Web.Models; // Usamos los modelos definidos abajo
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniMarket.Web.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsuariosController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsuariosController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // --- LISTAR ---
        public async Task<IActionResult> Index()
        {
            var usuarios = await _userManager.Users.ToListAsync();
            var lista = new List<UsuarioViewModel>();

            foreach (var user in usuarios)
            {
                var roles = await _userManager.GetRolesAsync(user);
                lista.Add(new UsuarioViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    Rol = roles.FirstOrDefault() ?? "Sin Rol"
                });
            }

            return View(lista);
        }

        // --- CREAR (VISTA) ---
        public IActionResult Create()
        {
            ViewBag.Roles = new SelectList(_roleManager.Roles, "Name", "Name");
            return View();
        }

        // --- CREAR (POST) ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioRegistroModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email, EmailConfirmed = true };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.Rol))
                    {
                        await _userManager.AddToRoleAsync(user, model.Rol);
                    }
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            ViewBag.Roles = new SelectList(_roleManager.Roles, "Name", "Name");
            return View(model);
        }
    }
}

// =========================================================
//  MODELOS AUXILIARES (DEFINIDOS EN EL NAMESPACE DE MODELS)
// =========================================================
namespace MiniMarket.Web.Models
{
    // Modelo para VER la lista
    public class UsuarioViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }
    }

    // Modelo para CREAR un usuario
    public class UsuarioRegistroModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; }
    }
}