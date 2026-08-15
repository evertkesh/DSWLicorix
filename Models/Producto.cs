namespace LICORIX_PROYECT.Models
{
    public class Producto
    {
        public int IdProducto{get;set;}
        public string Nombre{get;set;}=string.Empty;
        public string Descripcion{get;set;}=string.Empty;
        public decimal Precio{get;set;}
        public int Stock{get;set;}
        public string ImagenURL{get;set;}=string.Empty;
        public decimal GraduacionAlcoholica {get;set;}
        public int VolumenML{get;set;}
        public DateTime FechaRegistro{get;set;}
        public bool Destacado{get;set;}= false;
        public bool Estado{get;set;}=true;
        public int IdCategoria{get;set;}
        public int IdMarca{get;set;}

    }
}