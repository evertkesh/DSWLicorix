namespace LICORIX_PROYECT.Models
{
    
    public class CarritoItem
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string ImagenURL { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public int Stock { get; set; }

        public decimal Subtotal => Precio * Cantidad;
    }
}