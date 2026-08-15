using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LICORIX_PROYECT.Models;

namespace LICORIX_PROYECT.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly LICORIX_PROYECT.Data.ConexionBD _conexion;
    private readonly LICORIX_PROYECT.Data.Interfaces.IProductoRepositorio _productos;

    public HomeController(ILogger<HomeController> logger, LICORIX_PROYECT.Data.ConexionBD conexion, LICORIX_PROYECT.Data.Interfaces.IProductoRepositorio productos)
    {
        _logger = logger;
        _conexion = conexion;
        _productos = productos;
    }

    public IActionResult Index() => RedirectToAction("Index", "Inicio");

    public IActionResult Privacy() => View();

    
    
    public IActionResult TestDb()
    {
        if (_conexion.ProbarConexion(out var msg))
            return Content($"Conexión OK: {msg}");
        return Content($"Conexión FALLIDA: {msg}");
    }

    
    public IActionResult ListImages()
    {
        try
        {
            var lista = _productos.Listar().Take(20).Select(p => new { p.IdProducto, p.Nombre, p.ImagenURL }).ToList();
            return Json(lista);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message);
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}