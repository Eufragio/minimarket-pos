using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MiniMarket.Web.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MiniMarket.Web.Data
{
    public static class SembradorDatos
    {
        public static async Task Inicializar(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // 1. Crear Base de Datos si no existe
            context.Database.EnsureCreated();

            // 2. CREAR ROLES (NUEVO)
            string[] roles = { "Administrador", "Cajero" };
            foreach (var rol in roles)
            {
                if (!await roleManager.RoleExistsAsync(rol))
                {
                    await roleManager.CreateAsync(new IdentityRole(rol));
                }
            }

            // 3. ASEGURAR QUE EL ADMIN TENGA EL ROL DE JEFE
            // Busca tu usuario por correo (ajusta el correo si usas otro diferente)
            var emailAdmin = "admin@minimarket.com"; 
            var usuarioAdmin = await userManager.FindByEmailAsync(emailAdmin);

            if (usuarioAdmin != null)
            {
                // Si existe el usuario, le asignamos el rol Administrador si no lo tiene
                if (!await userManager.IsInRoleAsync(usuarioAdmin, "Administrador"))
                {
                    await userManager.AddToRoleAsync(usuarioAdmin, "Administrador");
                }
            }
            else
            {
                // Si es la primera vez y no existe usuario, lo creamos (Opcional)
                var nuevoAdmin = new IdentityUser { UserName = emailAdmin, Email = emailAdmin, EmailConfirmed = true };
                var resultado = await userManager.CreateAsync(nuevoAdmin, "Admin123*"); // Contraseña temporal
                if (resultado.Succeeded)
                {
                    await userManager.AddToRoleAsync(nuevoAdmin, "Administrador");
                }
            }

            // 4. (Opcional) Datos de Prueba para Productos/Categorias si está vacío
            if (!context.Categorias.Any())
            {
                context.Categorias.Add(new Categoria { Nombre = "Bebidas", Descripcion = "Gaseosas y Jugos" });
                context.Categorias.Add(new Categoria { Nombre = "Abarrotes", Descripcion = "Arroz, Azucar, Aceite" });
                await context.SaveChangesAsync();
            }
        }
    }
}