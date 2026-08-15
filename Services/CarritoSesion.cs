using System.Text.Json;
using LICORIX_PROYECT.Data.Interfaces;
using LICORIX_PROYECT.Models;

namespace LICORIX_PROYECT.Services;

public class CarritoSesion
{
    private const string Key = "Licorix.Carrito";
    private readonly IHttpContextAccessor _http;
    private readonly IProductoRepositorio _productos;

    public CarritoSesion(IHttpContextAccessor http, IProductoRepositorio productos)
    {
        _http = http;
        _productos = productos;
    }

    private ISession Session => _http.HttpContext!.Session;

    public List<CarritoItem> Obtener()
    {
        var json = Session.GetString(Key);
        if (string.IsNullOrEmpty(json)) return new List<CarritoItem>();
        return JsonSerializer.Deserialize<List<CarritoItem>>(json) ?? new List<CarritoItem>();
    }

    private void Guardar(List<CarritoItem> items)
    {
        Session.SetString(Key, JsonSerializer.Serialize(items));
    }

    public void Agregar(int idProducto, int cantidad = 1)
    {
        var producto = _productos.ObtenerPorId(idProducto);
        if (producto is null || producto.Estado == false || producto.Stock <= 0) return;

        var items = Obtener();
        var existente = items.FirstOrDefault(i => i.IdProducto == idProducto);
        if (existente != null)
        {
            existente.Cantidad = Math.Min(existente.Cantidad + cantidad, producto.Stock);
        }
        else
        {
            items.Add(new CarritoItem
            {
                IdProducto = producto.IdProducto,
                Nombre = producto.Nombre,
                ImagenURL = producto.ImagenURL,
                Precio = producto.Precio,
                Cantidad = Math.Min(cantidad, producto.Stock),
                Stock = producto.Stock
            });
        }
        Guardar(items);
    }

    public void Actualizar(int idProducto, int cantidad)
    {
        var items = Obtener();
        var item = items.FirstOrDefault(i => i.IdProducto == idProducto);
        if (item != null)
        {
            var producto = _productos.ObtenerPorId(idProducto);
            if (producto != null)
            {
                item.Precio = producto.Precio;
                item.Stock = producto.Stock;
                item.Cantidad = Math.Clamp(cantidad, 1, Math.Max(1, producto.Stock));
            }
            else
            {
                item.Cantidad = Math.Clamp(cantidad, 1, Math.Max(1, item.Stock));
            }
            Guardar(items);
        }
    }

    public void Eliminar(int idProducto)
    {
        var items = Obtener();
        items.RemoveAll(i => i.IdProducto == idProducto);
        Guardar(items);
    }

    public void Limpiar()
    {
        Session.Remove(Key);
    }

    public int CantidadTotal() => Obtener().Sum(i => i.Cantidad);
    public decimal Subtotal() => Obtener().Sum(i => i.Precio * i.Cantidad);
}