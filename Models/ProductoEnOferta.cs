namespace LICORIX_PROYECT.Models
{
    
    public class ProductoEnOferta
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public string Promocion { get; set; } = string.Empty;
        public string TipoDescuento { get; set; } = "p";   
        public decimal ValorDescuento { get; set; }
        public decimal PrecioConDescuento { get; set; }
        public string ImagenURL { get; set; } = string.Empty;

        public string Categoria { get; set; } = string.Empty;
        public int Stock { get; set; }
    }
}