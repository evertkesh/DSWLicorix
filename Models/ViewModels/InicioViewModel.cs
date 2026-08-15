namespace LICORIX_PROYECT.Models.ViewModels;

public class InicioViewModel
{
    public List<Producto> Destacados { get; set; } = new();
    public List<Categoria> Categorias { get; set; } = new();
    public List<Producto> NuevosIngresos { get; set; } = new();
    public List<Evento> Eventos { get; set; } = new();

}