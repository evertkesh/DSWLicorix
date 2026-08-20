namespace LICORIX_PROYECT.Helpers;

public static class ImagenHelper
{
    public static string Url(string? imagenUrl, string nombre, string carpeta = "productos")
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
                if (imagenUrl.StartsWith("/img/categorias/", StringComparison.OrdinalIgnoreCase))
                {
                    return imagenUrl.ToLowerInvariant() switch
                    {
                        "/img/categorias/whisky.png" => "~/imagenes/productos/whisky.jpg",
                        "/img/categorias/ron.png" => "~/imagenes/productos/RonZacapa23.jpg",
                        "/img/categorias/vodka.png" => "~/imagenes/productos/absolut.jpg",
                        "/img/categorias/tequila.png" => "~/imagenes/productos/TequilaReposado.jpg",
                        "/img/categorias/vino.png" => "~/imagenes/productos/vino.jpg",
                        "/img/categorias/pisco.png" => "~/imagenes/productos/PiscoTaberneroAcholado.jpg",
                        "/img/categorias/aguardiente.png" => "~/imagenes/productos/Aguardiente_Antioqueño_Azul.jpg",
                        "/img/categorias/cerveza.png" => "~/imagenes/productos/cerveza.jpg",
                        _ => Placeholder(nombre)
                    };
                }

                return "~" + imagenUrl;
            }

            
            return $"~/imagenes/{carpeta}/{imagenUrl}";
        }

        return Placeholder(nombre);
    }

    private static string Placeholder(string nombre)
    {
        var texto = Uri.EscapeDataString(string.IsNullOrWhiteSpace(nombre) ? "Licorix" : nombre);
        return $"https://placehold.co/600x800/1c1b1b/fed65b?text={texto}&font=oswald";
    }
}