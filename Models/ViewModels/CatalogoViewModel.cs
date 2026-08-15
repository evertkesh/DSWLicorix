namespace LICORIX_PROYECT.Models.ViewModels;

public class CatalogoViewModel
{
    public List<Producto> Productos { get; set; } = new();
    public List<Categoria> Categorias { get; set; } = new();
    public int? IdCategoriaSeleccionada { get; set; }
    public string? TextoBusqueda { get; set; }
    public string Orden { get; set; } = "nombre";   
    public decimal? PrecioMin { get; set; }
    public decimal? PrecioMax { get; set; }

    
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 12;
    public int TotalPages { get; set; } = 1;
    public int TotalCount { get; set; } = 0;
}