using Microsoft.Data.SqlClient;

namespace LICORIX_PROYECT.Data;

public class ConexionBD
{
    private readonly string _cadena;

    public ConexionBD(IConfiguration configuration)
    {
        _cadena = configuration.GetConnectionString("LicorixDB")
                  ?? throw new InvalidOperationException("No se encontró la cadena 'LicorixDB'.");
    }

    public SqlConnection ObtenerConexion() => new SqlConnection(_cadena);

    public bool ProbarConexion(out string mensaje)
    {
        try
        {
            using var cn = ObtenerConexion();
            cn.Open();
            mensaje = $"Conexión exitosa a '{cn.Database}' en '{cn.DataSource}'.";
            return true;
        }
        catch (SqlException ex)
        {
            mensaje = $"Error de SQL Server: {ex.Message}";
            return false;
        }
        catch (Exception ex)
        {
            mensaje = $"Error inesperado: {ex.Message}";
            return false;
        }
    }

    public int ContarProductos()
    {
        using var cn = ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand("SELECT COUNT(*) FROM dbo.Producto", cn);
        return (int)cmd.ExecuteScalar();
    }
}