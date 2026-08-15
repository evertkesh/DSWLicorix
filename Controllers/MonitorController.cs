using LICORIX_PROYECT.Models;
using LICORIX_PROYECT.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LICORIX_PROYECT.Controllers;

public class MonitorController : Controller
{
    public IActionResult Index(string? idPedido)
    {
        var pedido = new PedidoViewModel
        {
            IdPedido = idPedido ?? $"LX-{DateTime.Now:yyyyMMddHHmmss}",
            Fecha = DateTime.Now,
            EtapaActual = 4
        };

        if (!string.IsNullOrEmpty(idPedido))
        {
            pedido.NombreCliente = HttpContext.Session.GetString($"Pedido.{idPedido}.Nombre") ?? "Cliente Licorix";
            pedido.Direccion = HttpContext.Session.GetString($"Pedido.{idPedido}.Direccion") ?? string.Empty;
            pedido.MetodoPago = HttpContext.Session.GetString($"Pedido.{idPedido}.Pago") ?? "tarjeta";
            var total = HttpContext.Session.GetString($"Pedido.{idPedido}.Total");
            pedido.Total = decimal.TryParse(total, out var t) ? t : 0;

            var itemsJson = HttpContext.Session.GetString($"Pedido.{idPedido}.Items");
            if (!string.IsNullOrEmpty(itemsJson))
            {
                pedido.Items = System.Text.Json.JsonSerializer.Deserialize<List<CarritoItem>>(itemsJson) ?? new();
            }
        }

        
        var ahora = DateTime.Now;
        pedido.Etapas = new List<EtapaPedido>
        {
            new() { Numero = 1, Titulo = "Pedido Confirmado",   Icono = "check_circle",    HoraEstimada = ahora.AddMinutes(-15).ToString("HH:mm"), Completado = true },
            new() { Numero = 2, Titulo = "En Preparación",      Icono = "inventory_2",     HoraEstimada = ahora.AddMinutes(-10).ToString("HH:mm"), Completado = true },
            new() { Numero = 3, Titulo = "Saliendo de Bodega",  Icono = "departure_board", HoraEstimada = ahora.AddMinutes(-5).ToString("HH:mm"),  Completado = true },
            new() { Numero = 4, Titulo = "En Camino",           Icono = "delivery_dining", HoraEstimada = ahora.AddMinutes(15).ToString("HH:mm"),  Activo = true },
            new() { Numero = 5, Titulo = "Entregado",           Icono = "home",            HoraEstimada = ahora.AddMinutes(45).ToString("HH:mm") }
        };

        return View("~/Views/monitor_de_pedidos_licorix_es/monitor_de_pedidos.cshtml", pedido);
    }
}