using LICORIX_PROYECT.Data.Interfaces;
using LICORIX_PROYECT.Models;
using LICORIX_PROYECT.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LICORIX_PROYECT.Controllers;

public class OfertasController : Controller
{
    private readonly IProductoRepositorio _productos;
    private readonly ICategoriaRepositorio _categorias;
    private readonly IMarcaRepositorio _marcas;

    public OfertasController(IProductoRepositorio productos, ICategoriaRepositorio categorias, IMarcaRepositorio marcas)
    {
        _productos = productos;
        _categorias = categorias;
        _marcas = marcas;
    }

    public IActionResult Index()
    {
        var ofertas = _productos.ListarEnOferta();

        
        var todas = _productos.Listar();
        var cats = _categorias.Listar().ToDictionary(c => c.IdCategoria, c => c.Nombre);
        var marcas = _marcas.Listar().ToDictionary(m => m.IdMarca, m => m.Nombre);

        foreach (var o in ofertas)
        {
            var p = todas.FirstOrDefault(x => x.IdProducto == o.IdProducto);
            if (p != null)
            {
                o.ImagenURL = p.ImagenURL;
                o.Stock = p.Stock;
                o.Categoria = cats.TryGetValue(p.IdCategoria, out var cn) ? cn : string.Empty;
            }
        }

        return View("~/Views/ofertas_licorix_es/ofertas.cshtml", new OfertasViewModel { Productos = ofertas });
    }
}