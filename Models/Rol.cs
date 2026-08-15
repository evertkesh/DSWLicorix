namespace LICORIX_PROYECT.Models
{
    public class Rol
    {       
        public int IdRol {get;set;}
        public string Nombre{get;set;}=string.Empty;
        public string Descripcion{get;set;}=string.Empty;
        public bool Estado{get;set;} = true; 
    }
}