using LICORIX_PROYECT.Models.ViewModels;
using LICORIX_PROYECT.Services;
using Microsoft.AspNetCore.Mvc;

namespace LICORIX_PROYECT.Controllers;

public class Pago_y_EnvioController : Controller
{
    private readonly CarritoSesion _carrito;
    private readonly SesionUsuario _sesion;

    public Pago_y_EnvioController(CarritoSesion carrito, SesionUsuario sesion)
    {
        _carrito = carrito;
        _sesion = sesion;
    }

    [HttpGet]
    public IActionResult Index()
    {
        
        if (!_sesion.EstaAutenticado)
        {
            
            TempData["Aviso"] = "Debes iniciar sesión para proceder al pago.";
            
            
            return RedirectToAction("Login", "Cuenta", new { returnUrl = Url.Action("Index", "Pago_y_Envio") });
        }

        
        var items = _carrito.Obtener();
        if (items.Count == 0)
        {
            TempData["Aviso"] = "Tu carrito está vacío.";
            return RedirectToAction("Index", "Carrito");
        }
        var vm = new PagoEnvioViewModel
        {
            Items = items,
            Subtotal = _carrito.Subtotal(),
            NombreCompleto = _sesion.Obtener()?.NombreCompleto ?? string.Empty,
            Email = _sesion.Obtener()?.Correo ?? string.Empty
        };
        return View("~/Views/pago_y_envio_licorix_es/pago_y_envio.cshtml", vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Procesar(PagoEnvioViewModel vm)
    {
        var items = _carrito.Obtener();
        if (items.Count == 0) return RedirectToAction("Index", "Carrito");

        vm.Items = items;
        vm.Subtotal = _carrito.Subtotal();
        vm.Envio = vm.MetodoEnvio == "express" ? 9.90m : 4.90m;

        if (!ModelState.IsValid)
        {
            vm.Items = items;
            vm.Subtotal = _carrito.Subtotal();
            return View("~/Views/pago_y_envio_licorix_es/pago_y_envio.cshtml", vm);
        }

        
        var idPedido = $"LX-{DateTime.Now:yyyyMMddHHmmssfff}";

        
        HttpContext.Session.SetString($"Pedido.{idPedido}.Nombre", vm.NombreCompleto);
        HttpContext.Session.SetString($"Pedido.{idPedido}.Direccion", $"{vm.Direccion}, {vm.Ciudad}");
        HttpContext.Session.SetString($"Pedido.{idPedido}.Pago", vm.MetodoPago);
        HttpContext.Session.SetString($"Pedido.{idPedido}.Total", vm.Total.ToString("F2"));
        HttpContext.Session.SetString($"Pedido.{idPedido}.Items", System.Text.Json.JsonSerializer.Serialize(items));

        
        _carrito.Limpiar();

        TempData["Exito"] = "¡Pedido realizado con éxito!";
        return RedirectToAction("Index", "Monitor", new { idPedido });
    }
}