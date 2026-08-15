using LICORIX_PROYECT.Models.ViewModels;
using LICORIX_PROYECT.Services;
using Microsoft.AspNetCore.Mvc;

namespace LICORIX_PROYECT.Controllers;

public class CarritoController : Controller
{
    private readonly CarritoSesion _carrito;
    public CarritoController(CarritoSesion carrito) => _carrito = carrito;

    public IActionResult Index() => View("~/Views/carrito/carrito.cshtml", new CarritoViewModel { Items = _carrito.Obtener() });

    [HttpPost]
    public IActionResult Agregar(int id, int cantidad = 1)
    {
        _carrito.Agregar(id, cantidad);
        TempData["Exito"] = "Producto agregado al carrito.";
        
        var referer = Request.Headers["Referer"].ToString();
        if (!string.IsNullOrEmpty(referer) && referer.Contains("/DetalleProducto/"))
            return Redirect(referer);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Actualizar(int id, int cantidad)
    {
        _carrito.Actualizar(id, cantidad);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Eliminar(int id)
    {
        _carrito.Eliminar(id);
        TempData["Exito"] = "Producto eliminado del carrito.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Limpiar()
    {
        _carrito.Limpiar();
        return RedirectToAction(nameof(Index));
    }
}