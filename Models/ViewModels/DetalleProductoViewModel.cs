namespace LICORIX_PROYECT.Models.ViewModels;

public class DetalleProductoViewModel
{
    public Producto Producto { get; set; } = new();
    public List<Producto> Relacionados { get; set; } = new();
    public string NombreCategoria { get; set; } = string.Empty;
    public string NombreMarca { get; set; } = string.Empty;
    public string PaisOrigen { get; set; } = string.Empty;
}