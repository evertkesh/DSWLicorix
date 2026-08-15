using LICORIX_PROYECT.Data.Interfaces;
using LICORIX_PROYECT.Models;
using LICORIX_PROYECT.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LICORIX_PROYECT.Controllers;

public class CatalogoController : Controller
{
    private readonly IProductoRepositorio _productos;
    private readonly ICategoriaRepositorio _categorias;

    public CatalogoController(IProductoRepositorio productos, ICategoriaRepositorio categorias)
    {
        _productos = productos;
        _categorias = categorias;
    }

    public IActionResult Index(int? idCategoria, string? texto, string? ordenar, decimal? precioMin, decimal? precioMax, int page = 1, int pageSize = 12)
    {
        List<Producto> productos = idCategoria.HasValue
            ? _productos.ListarPorCategoria(idCategoria.Value)
            : (string.IsNullOrWhiteSpace(texto)
                ? _productos.Listar()
                : _productos.BuscarPorTexto(texto));

        
        productos = productos.Where(p => p.Estado).ToList();

        
        if (precioMin.HasValue) productos = productos.Where(p => p.Precio >= precioMin.Value).ToList();
        if (precioMax.HasValue) productos = productos.Where(p => p.Precio <= precioMax.Value).ToList();

        
        productos = ordenar switch
        {
            "precio-asc" => productos.OrderBy(p => p.Precio).ToList(),
            "precio-desc" => productos.OrderByDescending(p => p.Precio).ToList(),
            _ => productos.OrderBy(p => p.Nombre).ToList()
        };

        
        var totalCount = productos.Count;
        if (page < 1) page = 1;
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        if (page > totalPages && totalPages > 0) page = totalPages;
        var items = productos.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        var vm = new CatalogoViewModel
        {
            Productos = items,
            Categorias = _categorias.Listar(),
            IdCategoriaSeleccionada = idCategoria,
            TextoBusqueda = texto,
            Orden = ordenar ?? "nombre",
            PrecioMin = precioMin,
            PrecioMax = precioMax,
            CurrentPage = page,
            PageSize = pageSize,
            TotalPages = totalPages == 0 ? 1 : totalPages,
            TotalCount = totalCount
        };
        return View("~/Views/catalogo_licorix_es/catalogo.cshtml", vm);
    }

    [HttpGet]
    public IActionResult Buscar(string texto)
    {
        return RedirectToAction(nameof(Index), new { texto });
    }
}