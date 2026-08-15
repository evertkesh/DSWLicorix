namespace LICORIX_PROYECT.Models.ViewModels;

public class OfertasViewModel
{
    public List<ProductoEnOferta> Productos { get; set; } = new();
    public int Total => Productos.Count;
}