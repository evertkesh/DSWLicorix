using LICORIX_PROYECT.Models;
using Microsoft.AspNetCore.Mvc;

namespace LICORIX_PROYECT.Controllers;

public class ClienteController : Controller
{
    private readonly IHttpClientFactory _factory;
    private readonly IHttpContextAccessor _http;

    public ClienteController(IHttpClientFactory factory, IHttpContextAccessor http)
    {
        _factory = factory;
        _http = http;
    }

    public async Task<IActionResult> Index()
    {
        var client = CrearCliente();
        List<Producto> productos = new();
        try
        {
            productos = await client.GetFromJsonAsync<List<Producto>>("api/productosapi") ?? new();
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"No se pudo contactar el API: {ex.Message}";
        }
        return View(productos);
    }

    public async Task<IActionResult> Detalle(int id)
    {
        var client = CrearCliente();
        try
        {
            var producto = await client.GetFromJsonAsync<Producto>($"api/productosapi/{id}");
            if (producto == null) return NotFound();
            return View(producto);
        }
        catch
        {
            return NotFound();
        }
    }

    private HttpClient CrearCliente()
    {
        var client = _factory.CreateClient("api");

        var req = _http.HttpContext!.Request;
        client.BaseAddress = new Uri($"{req.Scheme}://{req.Host}/");
        return client;
    }
}