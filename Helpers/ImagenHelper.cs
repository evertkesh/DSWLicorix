namespace LICORIX_PROYECT.Helpers;

public static class ImagenHelper
{
    
    
    public static string Url(string? imagenUrl, string nombre)
    {
        if (!string.IsNullOrWhiteSpace(imagenUrl))
        {
            if (imagenUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                imagenUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                return imagenUrl; 
            }

            
            if (imagenUrl.StartsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                return "~" + imagenUrl;
            }

            
            return "~/imagenes/productos/" + imagenUrl;
        }

        var texto = Uri.EscapeDataString(string.IsNullOrWhiteSpace(nombre) ? "Licorix" : nombre);
        return $"https://placehold.co/600x800/1c1b1b/fed65b?text={texto}&font=oswald";
    }
}