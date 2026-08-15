namespace LICORIX_PROYECT.Models.ViewModels;

public class CarritoViewModel
{
    public List<CarritoItem> Items { get; set; } = new();
    public decimal Subtotal => Items.Sum(i => i.Precio * i.Cantidad);
    public decimal Envio { get; set; } = 4.90m;
    public decimal Total => Subtotal + Envio;
    public int CantidadTotal => Items.Sum(i => i.Cantidad);
}