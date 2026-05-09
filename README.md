# MiniMarket — Sistema de Punto de Venta (POS/ERP)

Sistema web completo de gestión para mini mercados y comercios minoristas. Cubre el ciclo completo de operaciones: compras, ventas, inventario, caja y reportes.

## Stack Tecnológico

| Capa | Tecnología |
|------|-----------|
| Framework | ASP.NET Core 10 MVC |
| ORM | Entity Framework Core 10 (Code-First) |
| Base de datos | SQL Server |
| Autenticación | ASP.NET Core Identity |
| UI | Razor Views + Bootstrap |

## Funcionalidades

### Módulos principales
- **Punto de Venta (POS)** — Carrito interactivo con búsqueda de productos y clientes, generación de ticket
- **Gestión de Compras** — Registro de compras por proveedor con actualización automática de stock
- **Control de Caja** — Apertura y cierre por usuario con arqueo de caja
- **Kardex de Inventario** — Registro automático de movimientos (ENTRADA/SALIDA) por cada venta y compra
- **Gestión de Productos** — CRUD con imágenes, código de barras y control de stock mínimo
- **Gestión de Clientes y Proveedores** — CRUD con activación/desactivación
- **Dashboard** — KPIs en tiempo real, top productos, gráfico de ventas
- **Gestión de Usuarios** — Creación y asignación de roles (solo Administrador)
- **Configuración** — Datos de empresa, RUC, IGV, moneda

### Roles
| Rol | Permisos |
|-----|---------|
| Administrador | Acceso completo, gestión de usuarios y configuración |
| Cajero | Ventas, compras y caja propia |

## Base de Datos

**Motor:** SQL Server  
**Nombre:** `MiniMarketDB`

### Entidades principales
`Producto` · `Categoria` · `Proveedor` · `Cliente` · `Venta` · `DetalleVenta` · `Compra` · `DetalleCompra` · `MovimientoInventario` · `AperturaCaja` · `Configuracion`

## Arquitectura

Patrón **MVC** con acceso a datos directo via EF Core DbContext. Transacciones ACID en operaciones de venta/compra para garantizar consistencia entre stock, Kardex y caja.

```
MiniMarket.Web/
├── Controllers/     # 10 controllers MVC
├── Models/          # 11 entidades de dominio
├── Data/            # ApplicationDbContext + Seeder
├── Views/           # Vistas Razor
├── Areas/Identity/  # Autenticación (Razor Pages)
├── Migrations/      # Migraciones EF Core
└── wwwroot/         # Estáticos (CSS, JS, imágenes)
```

## Configuración y Puesta en Marcha

### Prerrequisitos
- .NET 10 SDK
- SQL Server (local o remoto)

### Pasos

1. Clonar el repositorio
2. Actualizar la connection string en `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=TU_SERVIDOR;Database=MiniMarketDB;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```
3. Aplicar migraciones:
   ```bash
   dotnet ef database update
   ```
4. Ejecutar la aplicación:
   ```bash
   dotnet run
   ```

### Credenciales por defecto
| Campo | Valor |
|-------|-------|
| Email | `admin@minimarket.com` |
| Contraseña | `Admin123*` |
| Rol | Administrador |

## Capturas de Pantalla

> *(Agregar capturas del dashboard, POS y módulo de ventas)*

## Licencia

Proyecto de portafolio personal — uso educativo.
