namespace LICORIX_PROYECT.Models
{
    public class Marca
    {
        public int IdMarca{get;set;}
        public string Nombre{get;set;}=string.Empty;
        public string PaisOrigen{get;set;}=string.Empty;
        public string Descripcion{get;set;}=string.Empty;
        public bool Estado{get;set;} = true;
     
    }
}