namespace LICORIX_PROYECT.Models
{
    
    public class Evento
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Hora { get; set; } = string.Empty;
        public string Lugar { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string ImagenURL { get; set; } = string.Empty;
        public int PlazasRestantes { get; set; }
        public bool Destacado { get; set; }
    }
}