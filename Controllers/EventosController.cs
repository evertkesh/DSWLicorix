using LICORIX_PROYECT.Models;
using Microsoft.AspNetCore.Mvc;

namespace LICORIX_PROYECT.Controllers;

public class EventosController : Controller
{
    public IActionResult Index()
    {
        
        var eventos = new List<Evento>
        {
            new() { Id = 1, Titulo = "Cata Maestra: Macallan Rare Cask", Descripcion = "Una experiencia sensorial guiada por nuestro sommelier de cabecera. Maridaje de 5 maltas con chocolates de autor.", Fecha = new DateTime(2026, 9, 14), Hora = "19:00", Lugar = "Licorix Casa Central - Sala de Catas", Categoria = "Cata Presencial", ImagenURL = "https://lh3.googleusercontent.com/aida-public/AB6AXuCWBy_SKmooyuJAtflkydivM0Vx-4uuFPkoZeunfncjyo_-6078eJNUXdDBntdwZxdLXT2yCphAG9dKqrOqkQbG0YVGeq_TQa1-9ihof5CgceaYNTv6QrYYkA77NvWQX7tg8gsLe6XGBRXBIeNJf4Nz8ojZsOegW0hfehtFYCPQ88PVh7v2ica6rAZx0FfDGMBC6xY-KOBs3Xzj3eScm7ESC-ZaCTjUijs9lP9c-Ck9ZIIy3saXl6AK", PlazasRestantes = 8, Destacado = true },
            new() { Id = 2, Titulo = "Taller de Coctelería: Negroni Perfecto", Descripcion = "Aprende las técnicas profesionales para balancear amargor, dulzor y carácter en un cóctel clásico.", Fecha = new DateTime(2026, 9, 21), Hora = "20:00", Lugar = "Licorix Lima - Sky Bar", Categoria = "Taller Coctelería", ImagenURL = "https://images.unsplash.com/photo-1551538827-9c037cb4f32a?w=900", PlazasRestantes = 12 },
            new() { Id = 3, Titulo = "Lanzamiento: Edición Cosecha 2024", Descripcion = "Descubre en exclusiva nuestra nueva cosecha de piscos y single malts seleccionados.", Fecha = new DateTime(2026, 10, 5), Hora = "18:30", Lugar = "Licorix Casa Central - Atrio Principal", Categoria = "Lanzamiento", ImagenURL = "https://images.unsplash.com/photo-1569529465841-dfecdab7503b?w=900", PlazasRestantes = 25 },
            new() { Id = 4, Titulo = "Maridaje de Whiskies y Quesos", Descripcion = "Un recorrido por las regiones productoras de whisky acompañado de una tabla de quesos artesanales.", Fecha = new DateTime(2026, 10, 19), Hora = "19:30", Lugar = "Licorix Casa Central - Sala de Catas", Categoria = "Cata Presencial", ImagenURL = "https://images.unsplash.com/photo-1543007630-9710e4a00a20?w=900", PlazasRestantes = 6 },
            new() { Id = 5, Titulo = "Tour por la Bodega Subterránea", Descripcion = "Visita guiada a nuestra cava histórica con más de 3,000 etiquetas curadas de los cinco continentes.", Fecha = new DateTime(2026, 11, 2), Hora = "17:00", Lugar = "Licorix Casa Central", Categoria = "Experiencia", ImagenURL = "https://images.unsplash.com/photo-1568213816046-0ee1c42bd559?w=900", PlazasRestantes = 20 },
            new() { Id = 6, Titulo = "Master Class: El Arte del Mezcal", Descripcion = "Profundiza en la producción artesanal del mezcal oaxaqueño con un maestro mezcalero invitado.", Fecha = new DateTime(2026, 11, 16), Hora = "19:00", Lugar = "Licorix Lima - Sky Bar", Categoria = "Taller Coctelería", ImagenURL = "https://images.unsplash.com/photo-1514362545857-3bc16c4c7d1b?w=900", PlazasRestantes = 15 }
        };

        ViewBag.Eventos = eventos;
        return View("~/Views/eventos_licorix_es/eventos.cshtml", eventos);
    }
}