using LICORIX_PROYECT.Data.Interfaces;
using LICORIX_PROYECT.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LICORIX_PROYECT.Controllers;

public class DetalleProductoController : Controller
{
    private readonly IProductoRepositorio _productos;
    private readonly ICategoriaRepositorio _categorias;
    private readonly IMarcaRepositorio _marcas;

    public DetalleProductoController(IProductoRepositorio productos, ICategoriaRepositorio categorias, IMarcaRepositorio marcas)
    {
        _productos = productos;
        _categorias = categorias;
        _marcas = marcas;
    }

    public IActionResult Index(int id)
    {
        var producto = _productos.ObtenerPorId(id);
        if (producto is null) return NotFound();

        var categoria = _categorias.Listar().FirstOrDefault(c => c.IdCategoria == producto.IdCategoria);
        var marca = _marcas.Listar().FirstOrDefault(m => m.IdMarca == producto.IdMarca);
        var relacionados = _productos.ListarPorCategoria(producto.IdCategoria)
            .Where(p => p.IdProducto != id)
            .Take(4)
            .ToList();

        var vm = new DetalleProductoViewModel
        {
            Producto = producto,
            Relacionados = relacionados,
            NombreCategoria = categoria?.Nombre ?? string.Empty,
            NombreMarca = marca?.Nombre ?? string.Empty,
            PaisOrigen = marca?.PaisOrigen ?? string.Empty
        };
        return View("~/Views/detalle_producto/detalle_producto.cshtml", vm);
    }
}