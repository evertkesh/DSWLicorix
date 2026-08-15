using System.Text.Json;
using LICORIX_PROYECT.Models;

namespace LICORIX_PROYECT.Services;

public class UsuarioSesion
{
    public int IdUsuario { get; set; }
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public int IdRol { get; set; }
    public string NombreRol { get; set; } = string.Empty;

    public string NombreCompleto => $"{Nombres} {Apellidos}".Trim();
    public bool EsAdmin => NombreRol.Equals("Administrador", StringComparison.OrdinalIgnoreCase);
}

public class SesionUsuario
{
    private const string Key = "Licorix.Usuario";
    private readonly IHttpContextAccessor _http;

    public SesionUsuario(IHttpContextAccessor http) => _http = http;

    private ISession Session => _http.HttpContext!.Session;

    public UsuarioSesion? Obtener()
    {
        var json = Session.GetString(Key);
        if (string.IsNullOrEmpty(json)) return null;
        return JsonSerializer.Deserialize<UsuarioSesion>(json);
    }

    public void Iniciar(Usuario usuario)
    {
        var s = new UsuarioSesion
        {
            IdUsuario = usuario.IdUsuario,
            Nombres = usuario.Nombres,
            Apellidos = usuario.Apellidos,
            Correo = usuario.Correo,
            IdRol = usuario.IdRol,
            NombreRol = usuario.NombreRol ?? string.Empty
        };
        Session.SetString(Key, JsonSerializer.Serialize(s));
    }

    public void Cerrar()
    {
        Session.Remove(Key);
    }

    public bool EstaAutenticado => Obtener() != null;
}