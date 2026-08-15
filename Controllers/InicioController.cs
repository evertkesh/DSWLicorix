using LICORIX_PROYECT.Data.Interfaces;
using LICORIX_PROYECT.Models;
using LICORIX_PROYECT.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LICORIX_PROYECT.Controllers;

public class InicioController : Controller
{
    private readonly IProductoRepositorio _productos;
    private readonly ICategoriaRepositorio _categorias;

    public InicioController(IProductoRepositorio productos, ICategoriaRepositorio categorias)
    {
        _productos = productos;
        _categorias = categorias;
    }

    public IActionResult Index()
    {
        var vm = new InicioViewModel
        {
            Destacados = _productos.ListarDestacados().Take(4).ToList(),
            NuevosIngresos = _productos.ListarNuevosIngresos().Take(4).ToList(),
            Categorias = _categorias.Listar(),
            Eventos = ObtenerEventosDestacados().Take(4).ToList()        };
        return View("~/Views/inicio_licorix_es/inicio.cshtml", vm);
    }
    private static List<Evento> ObtenerEventosDestacados() => new()
    {
        new() { Id = 1, Titulo = "Cata Maestra: Macallan Rare Cask", Descripcion = "Una experiencia sensorial guiada por nuestro sommelier de cabecera. Maridaje de 5 maltas con chocolates de autor.", Fecha = new DateTime(2026, 9, 14), Hora = "19:00", Lugar = "Licorix Casa Central - Sala de Catas", Categoria = "Cata Presencial", ImagenURL = "https://lh3.googleusercontent.com/aida-public/AB6AXuCWBy_SKmooyuJAtflkydivM0Vx-4uuFPkoZeunfncjyo_-6078eJNUXdDBntdwZxdLXT2yCphAG9dKqrOqkQbG0YVGeq_TQa1-9ihof5CgceaYNTv6QrYYkA77NvWQX7tg8gsLe6XGBRXBIeNJf4Nz8ojZsOegW0hfehtFYCPQ88PVh7v2ica6rAZx0FfDGMBC6xY-KOBs3Xzj3eScm7ESC-ZaCTjUijs9lP9c-Ck9ZIIy3saXl6AK", PlazasRestantes = 8, Destacado = true },
        new() { Id = 2, Titulo = "Taller de Coctelería: Negroni Perfecto", Descripcion = "Aprende las técnicas profesionales para balancear amargor, dulzor y carácter en un cóctel clásico.", Fecha = new DateTime(2026, 9, 21), Hora = "20:00", Lugar = "Licorix Lima - Sky Bar", Categoria = "Taller Coctelería", ImagenURL = "https://images.unsplash.com/photo-1551538827-9c037cb4f32a?w=900", PlazasRestantes = 12 },
        new() { Id = 3, Titulo = "Lanzamiento: Edición Cosecha 2024", Descripcion = "Descubre en exclusiva nuestra nueva cosecha de piscos y single malts seleccionados.", Fecha = new DateTime(2026, 10, 5), Hora = "18:30", Lugar = "Licorix Casa Central - Atrio Principal", Categoria = "Lanzamiento", ImagenURL = "https://images.unsplash.com/photo-1569529465841-dfecdab7503b?w=900", PlazasRestantes = 25 },
        new() { Id = 4, Titulo = "Maridaje de Whiskies y Quesos", Descripcion = "Un recorrido por las regiones productoras de whisky acompañado de una tabla de quesos artesanales.", Fecha = new DateTime(2026, 10, 19), Hora = "19:30", Lugar = "Licorix Casa Central - Sala de Catas", Categoria = "Cata Presencial", ImagenURL = "https://images.unsplash.com/photo-1543007630-9710e4a00a20?w=900", PlazasRestantes = 6 }
    };
}