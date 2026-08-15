using LICORIX_PROYECT.Data.Interfaces;
using LICORIX_PROYECT.Models;
using LICORIX_PROYECT.Services;
using Microsoft.AspNetCore.Mvc;

namespace LICORIX_PROYECT.Controllers;

public class AdminController : Controller
{
    private readonly IProductoRepositorio _productos;
    private readonly IPromocionRepositorio _promociones;
    private readonly IUsuarioRepositorio _usuarios;
    private readonly ICategoriaRepositorio _categorias;
    private readonly IMarcaRepositorio _marcas;
    private readonly SesionUsuario _sesion;

    public AdminController(IProductoRepositorio productos,
                           IPromocionRepositorio promociones,
                           IUsuarioRepositorio usuarios,
                           ICategoriaRepositorio categorias,
                           IMarcaRepositorio marcas,
                           SesionUsuario sesion)
    {
        _productos = productos;
        _promociones = promociones;
        _usuarios = usuarios;
        _categorias = categorias;
        _marcas = marcas;
        _sesion = sesion;
    }

    private IActionResult? VerificarAdmin()
    {
        var u = _sesion.Obtener();
        if (u == null)
        {
            TempData["Error"] = "Debes iniciar sesión.";
            return RedirectToAction("Login", "Cuenta");
        }
        if (u.IdRol != 1 && !u.EsAdmin)
        {
            TempData["Error"] = "No tienes permisos de administrador.";
            return RedirectToAction("Index", "Inicio");
        }
        return null;
    }

    public IActionResult Index()
    {
        var r = VerificarAdmin(); if (r != null) return r;
        var productos = _productos.Listar();
        var promos = _promociones.Listar();
        var admins = _usuarios.Listar();
        ViewBag.TotalProductos = productos.Count;
        ViewBag.TotalStock = productos.Sum(p => p.Stock);
        ViewBag.TotalPromos = promos.Count;
        ViewBag.TotalAdmins = admins.Count;
        return View("~/Views/Admin/Index.cshtml");
    }

    
    public IActionResult Stock(int page = 1, int pageSize = 10)
    {
        const int fixedPageSize = 10;
        var r = VerificarAdmin(); if (r != null) return r;
        if (page < 1) page = 1;
        pageSize = fixedPageSize;

        try
        {
            int totalCount;
            var items = _productos.ListarPaginado(page, pageSize, out totalCount);
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            if (totalPages < 1) totalPages = 1;
            if (page > totalPages && totalPages > 0) page = totalPages;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;
            return View("~/Views/Admin/Stock.cshtml", items);
        }
        catch (System.Exception ex)
        {
            TempData["Error"] = "Error al obtener productos paginados desde la base de datos. Asegúrate de ejecutar LICOREX-PROYECT.sql. Mostrando productos sin paginación. (" + ex.Message + ")";
            var items = _productos.Listar();
            var totalPages = (int)Math.Ceiling(items.Count / (double)fixedPageSize);
            if (totalPages < 1) totalPages = 1;
            if (page > totalPages && totalPages > 0) page = totalPages;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = fixedPageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = items.Count;
            var pagedItems = items.Skip((page - 1) * fixedPageSize).Take(fixedPageSize).ToList();
            return View("~/Views/Admin/Stock.cshtml", pagedItems);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ActualizarStock(int id, int stock)
    {
        var r = VerificarAdmin(); if (r != null) return r;
        if (stock < 0)
        {
            TempData["Error"] = "El stock no puede ser negativo.";
            return RedirectToAction(nameof(Stock));
        }
        var p = _productos.ObtenerPorId(id);
        if (p == null)
        {
            TempData["Error"] = "Producto no encontrado.";
            return RedirectToAction(nameof(Stock));
        }
        p.Stock = stock;
        _productos.Actualizar(p);
        TempData["Exito"] = $"Stock de '{p.Nombre}' actualizado a {stock} unidades.";
        return RedirectToAction(nameof(Stock));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PonerEnStock(int id, int cantidad)
    {
        var r = VerificarAdmin(); if (r != null) return r;
        if (cantidad <= 0)
        {
            TempData["Error"] = "La cantidad a ingresar debe ser mayor a 0.";
            return RedirectToAction(nameof(Stock));
        }
        var p = _productos.ObtenerPorId(id);
        if (p == null)
        {
            TempData["Error"] = "Producto no encontrado.";
            return RedirectToAction(nameof(Stock));
        }
        p.Stock += cantidad;
        _productos.Actualizar(p);
        TempData["Exito"] = $"Se agregaron {cantidad} unidades a '{p.Nombre}'. Stock actual: {p.Stock}.";
        return RedirectToAction(nameof(Stock));
    }

    
    private static List<EventoAdmin> _eventos = new()
    {
        new EventoAdmin { Id = 1, Titulo = "Cata Maestra: Macallan Rare Cask", Descripcion = "Experiencia sensorial guiada por nuestro sommelier.", Fecha = new DateTime(2026, 9, 14), Hora = "19:00", Lugar = "Licorix Casa Central", Categoria = "Cata Presencial", ImagenURL = "https://images.unsplash.com/photo-1569529465841-dfecdab7503b?w=900", PlazasRestantes = 8, Activo = true, Destacado = true },
        new EventoAdmin { Id = 2, Titulo = "Taller de Coctelería: Negroni Perfecto", Descripcion = "Técnicas profesionales para cócteles clásicos.", Fecha = new DateTime(2026, 9, 21), Hora = "20:00", Lugar = "Licorix Lima - Sky Bar", Categoria = "Taller Coctelería", ImagenURL = "https://images.unsplash.com/photo-1551538827-9c037cb4f32a?w=900", PlazasRestantes = 12, Activo = true }
    };

    public IActionResult Eventos()
    {
        var r = VerificarAdmin(); if (r != null) return r;
        return View("~/Views/Admin/Eventos.cshtml", _eventos.OrderBy(e => e.Fecha).ToList());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ToggleEvento(int id)
    {
        var r = VerificarAdmin(); if (r != null) return r;
        var e = _eventos.FirstOrDefault(x => x.Id == id);
        if (e == null)
        {
            TempData["Error"] = "Evento no encontrado.";
            return RedirectToAction(nameof(Eventos));
        }
        e.Activo = !e.Activo;
        TempData["Exito"] = e.Activo
            ? $"Evento '{e.Titulo}' reactivado."
            : $"Evento '{e.Titulo}' desactivado.";
        return RedirectToAction(nameof(Eventos));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AgregarEvento(EventoAdmin modelo)
    {
        var r = VerificarAdmin(); if (r != null) return r;
        if (string.IsNullOrWhiteSpace(modelo.Titulo))
        {
            TempData["Error"] = "El título es obligatorio.";
            return RedirectToAction(nameof(Eventos));
        }
        modelo.Id = _eventos.Count == 0 ? 1 : _eventos.Max(x => x.Id) + 1;
        modelo.Activo = true;
        _eventos.Add(modelo);
        TempData["Exito"] = $"Evento '{modelo.Titulo}' agregado.";
        return RedirectToAction(nameof(Eventos));
    }

    
    public IActionResult Ofertas()
    {
        var r = VerificarAdmin(); if (r != null) return r;
        return View("~/Views/Admin/Ofertas.cshtml", _promociones.Listar());
    }

    
    public IActionResult CrearOferta()
    {
        var r = VerificarAdmin(); if (r != null) return r;
        var productos = _productos.Listar();
        ViewBag.Productos = productos;
        ViewBag.ProductosSeleccionados = new List<int>();
        return View("~/Views/Admin/CrearOferta.cshtml", new Promocion { FechaInicio = DateTime.Today, FechaFin = DateTime.Today.AddDays(7), TipoDescuento = "p", Estado = true });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CrearOferta(Promocion modelo, int[]? productosSeleccionados)
    {
        var r = VerificarAdmin(); if (r != null) return r;
        if (string.IsNullOrWhiteSpace(modelo.Nombre))
        {
            TempData["Error"] = "El nombre de la promoción es obligatorio.";
            return RedirectToAction(nameof(CrearOferta));
        }
        if (modelo.TipoDescuento != "p" && modelo.TipoDescuento != "m")
        {
            TempData["Error"] = "Tipo de descuento inválido.";
            return RedirectToAction(nameof(CrearOferta));
        }
        if (modelo.ValorDescuento <= 0)
        {
            TempData["Error"] = "El valor del descuento debe ser mayor a 0.";
            return RedirectToAction(nameof(CrearOferta));
        }
        if (modelo.FechaFin < modelo.FechaInicio)
        {
            TempData["Error"] = "La fecha de fin debe ser igual o posterior a la fecha de inicio.";
            return RedirectToAction(nameof(CrearOferta));
        }

        
        var idProm = _promociones.Insertar(modelo);
        if (productosSeleccionados != null && productosSeleccionados.Length > 0)
        {
            foreach (var idProd in productosSeleccionados)
            {
                _promociones.AgregarProductoAPromocion(idProd, idProm);
            }
        }
        TempData["Exito"] = $"Promoción '{modelo.Nombre}' creada.";
        return RedirectToAction(nameof(Ofertas));
    }

    
    public IActionResult EditarOferta(int id)
    {
        var r = VerificarAdmin(); if (r != null) return r;
        var p = _promociones.ObtenerPorId(id);
        if (p == null) return RedirectToAction(nameof(Ofertas));
        ViewBag.Productos = _productos.Listar();
        ViewBag.ProductosSeleccionados = _promociones.ListarProductosPorPromocion(id);
        return View("~/Views/Admin/CrearOferta.cshtml", p);
    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditarOferta(Promocion modelo, int[]? productosSeleccionados)
    {
        var r = VerificarAdmin(); if (r != null) return r;
        if (modelo == null || modelo.IdPromocion == 0) return RedirectToAction(nameof(Ofertas));
        _promociones.Actualizar(modelo);
        
        _promociones.RemoverTodosProductosDePromocion(modelo.IdPromocion);
        if (productosSeleccionados != null)
        {
            foreach (var idProd in productosSeleccionados) _promociones.AgregarProductoAPromocion(idProd, modelo.IdPromocion);
        }
        TempData["Exito"] = $"Promoción '{modelo.Nombre}' actualizada.";
        return RedirectToAction(nameof(Ofertas));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EliminarOferta(int id)
    {
        var r = VerificarAdmin(); if (r != null) return r;
        var p = _promociones.ObtenerPorId(id);
        if (p == null)
        {
            TempData["Error"] = "Promoción no encontrada.";
            return RedirectToAction(nameof(Ofertas));
        }
        _promociones.Eliminar(id);
        TempData["Exito"] = $"Promoción '{p.Nombre}' eliminada.";
        return RedirectToAction(nameof(Ofertas));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ToggleOferta(int id)
    {
        var r = VerificarAdmin(); if (r != null) return r;
        var p = _promociones.ObtenerPorId(id);
        if (p == null)
        {
            TempData["Error"] = "Promoción no encontrada.";
            return RedirectToAction(nameof(Ofertas));
        }
        p.Estado = !p.Estado;
        _promociones.Actualizar(p);
        TempData["Exito"] = p.Estado
            ? $"Promoción '{p.Nombre}' activada."
            : $"Promoción '{p.Nombre}' desactivada.";
        return RedirectToAction(nameof(Ofertas));
    }

    
    public IActionResult CrearProducto()
    {
        var r = VerificarAdmin(); if (r != null) return r;
        ViewBag.Categorias = _categorias.Listar();
        ViewBag.Marcas = _marcas.Listar();
        return View("~/Views/Admin/CrearProducto.cshtml", new LICORIX_PROYECT.Models.Producto { FechaRegistro = DateTime.Now, Estado = true });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CrearProducto(LICORIX_PROYECT.Models.Producto modelo)
    {
        var r = VerificarAdmin(); if (r != null) return r;
        if (string.IsNullOrWhiteSpace(modelo.Nombre))
        {
            TempData["Error"] = "El nombre del producto es obligatorio.";
            return RedirectToAction(nameof(CrearProducto));
        }
        if (modelo.Precio <= 0)
        {
            TempData["Error"] = "El precio debe ser mayor a 0.";
            return RedirectToAction(nameof(CrearProducto));
        }
        if (modelo.Stock < 0)
        {
            TempData["Error"] = "El stock no puede ser negativo.";
            return RedirectToAction(nameof(CrearProducto));
        }
        _productos.Insertar(modelo);
        TempData["Exito"] = $"Producto '{modelo.Nombre}' agregado.";
        return RedirectToAction(nameof(Stock));
    }

    
    public IActionResult EditarProducto(int id)
    {
        var r = VerificarAdmin(); if (r != null) return r;
        var p = _productos.ObtenerPorId(id);
        if (p == null) return RedirectToAction(nameof(Stock));
        ViewBag.Categorias = _categorias.Listar();
        ViewBag.Marcas = _marcas.Listar();
        return View("~/Views/Admin/CrearProducto.cshtml", p);
    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditarProducto(LICORIX_PROYECT.Models.Producto modelo)
    {
        var r = VerificarAdmin(); if (r != null) return r;
        if (modelo == null || modelo.IdProducto == 0) return RedirectToAction(nameof(Stock));
        _productos.Actualizar(modelo);
        TempData["Exito"] = $"Producto '{modelo.Nombre}' actualizado.";
        return RedirectToAction(nameof(Stock));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EliminarProducto(int id)
    {
        var r = VerificarAdmin(); if (r != null) return r;
        var p = _productos.ObtenerPorId(id);
        if (p == null)
        {
            TempData["Error"] = "Producto no encontrado.";
            return RedirectToAction(nameof(Stock));
        }
        _productos.Eliminar(id);
        TempData["Exito"] = $"Producto '{p.Nombre}' eliminado.";
        return RedirectToAction(nameof(Stock));
    }

    
    public IActionResult Usuarios(int page = 1, int pageSize = 10)
    {
        var r = VerificarAdmin(); if (r != null) return r;
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        try
        {
            int totalCount;
            var items = _usuarios.ListarPaginado(page, pageSize, out totalCount);
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;
            return View("~/Views/Admin/Usuarios.cshtml", items);
        }
        catch (System.Exception ex)
        {
            
            TempData["Error"] = "Error al obtener la lista paginada de usuarios. Asegúrate de ejecutar LICOREX-PROYECT.sql en la base de datos. Mostrando usuarios sin paginación. (" + ex.Message + ")";
            var items = _usuarios.Listar();
            ViewBag.CurrentPage = 1;
            ViewBag.PageSize = items.Count;
            ViewBag.TotalPages = 1;
            ViewBag.TotalCount = items.Count;
            return View("~/Views/Admin/Usuarios.cshtml", items);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PromoverAdministrador(int id)
    {
        var r = VerificarAdmin(); if (r != null) return r;
        var solicitante = _sesion.Obtener(); if (solicitante == null) { TempData["Error"] = "Debes iniciar sesión."; return RedirectToAction(nameof(Usuarios)); }
        try
        {
            _usuarios.PromoverAAdministradorSP(solicitante.IdUsuario, id);
            var u = _usuarios.ObtenerPorId(id);
            TempData["Exito"] = u != null ? $"Usuario '{u.Nombres} {u.Apellidos}' promovido a Administrador." : "Usuario promovido a Administrador.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(Usuarios));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RemoverAdministrador(int id)
    {
        var r = VerificarAdmin(); if (r != null) return r;
        var solicitante = _sesion.Obtener(); if (solicitante == null) { TempData["Error"] = "Debes iniciar sesión."; return RedirectToAction(nameof(Usuarios)); }
        try
        {
            _usuarios.RemoverAdministradorSP(solicitante.IdUsuario, id);
            var u = _usuarios.ObtenerPorId(id);
            TempData["Exito"] = u != null ? $"Usuario '{u.Nombres} {u.Apellidos}' ya no es Administrador." : "Rol de administrador removido.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(Usuarios));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DesactivarUsuario(int id)
    {
        var r = VerificarAdmin(); if (r != null) return r;
        var solicitante = _sesion.Obtener(); if (solicitante == null) { TempData["Error"] = "Debes iniciar sesión."; return RedirectToAction(nameof(Usuarios)); }
        try
        {
            _usuarios.DesactivarUsuarioSP(solicitante.IdUsuario, id);
            var u = _usuarios.ObtenerPorId(id);
            TempData["Exito"] = u != null ? $"Usuario '{u.Nombres} {u.Apellidos}' desactivado." : "Usuario desactivado.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(Usuarios));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ActivarUsuario(int id)
    {
        var r = VerificarAdmin(); if (r != null) return r;
        var solicitante = _sesion.Obtener(); if (solicitante == null) { TempData["Error"] = "Debes iniciar sesión."; return RedirectToAction(nameof(Usuarios)); }
        try
        {
            _usuarios.ActivarUsuarioSP(solicitante.IdUsuario, id);
            var u = _usuarios.ObtenerPorId(id);
            TempData["Exito"] = u != null ? $"Usuario '{u.Nombres} {u.Apellidos}' activado." : "Usuario activado.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(Usuarios));
    }

    
    public IActionResult CrearAdministrador()
    {
        var r = VerificarAdmin(); if (r != null) return r;
        return View("~/Views/Admin/CrearAdministrador.cshtml");
    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CrearAdministrador(LICORIX_PROYECT.Models.Usuario modelo)
    {
        var r = VerificarAdmin(); if (r != null) return r;
        if (string.IsNullOrWhiteSpace(modelo.Correo) || string.IsNullOrWhiteSpace(modelo.Nombres))
        {
            TempData["Error"] = "Nombre y correo son obligatorios.";
            return RedirectToAction(nameof(CrearAdministrador));
        }
        var solicitante = _sesion.Obtener(); if (solicitante == null) { TempData["Error"] = "Debes iniciar sesión."; return RedirectToAction(nameof(CrearAdministrador)); }
        try
        {
            
            _usuarios.CrearAdministradorSP(solicitante.IdUsuario, modelo);
            TempData["Exito"] = $"Administrador '{modelo.Nombres} {modelo.Apellidos}' creado/promovido.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(Usuarios));
    }
}

public class EventoAdmin
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public DateTime Fecha { get; set; } = DateTime.Today.AddDays(7);
    public string Hora { get; set; } = "19:00";
    public string Lugar { get; set; } = string.Empty;
    public string Categoria { get; set; } = "Cata Presencial";
    public string ImagenURL { get; set; } = "https://images.unsplash.com/photo-1569529465841-dfecdab7503b?w=900";
    public int PlazasRestantes { get; set; } = 10;
    public bool Activo { get; set; } = true;
    public bool Destacado { get; set; }
}
