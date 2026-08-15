using Microsoft.Data.SqlClient;
using LICORIX_PROYECT.Models;
using LICORIX_PROYECT.Data.Interfaces;

namespace LICORIX_PROYECT.Data;

public class RolRepositorio : IRolRepositorio
{
    private readonly ConexionBD _bd;
    public RolRepositorio(ConexionBD bd) => _bd = bd;

    public List<Rol> Listar()
    {
        var lista = new List<Rol>();
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand("SELECT IdRol, Nombre, Descripcion, Estado FROM Rol WHERE Estado = 1 ORDER BY Nombre", cn);
        using var dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            lista.Add(new Rol
            {
                IdRol = dr.GetInt32(dr.GetOrdinal("IdRol")),
                Nombre = dr.GetString(dr.GetOrdinal("Nombre")),
                Descripcion = dr.IsDBNull(dr.GetOrdinal("Descripcion")) ? string.Empty : dr.GetString(dr.GetOrdinal("Descripcion")),
                Estado = dr.GetBoolean(dr.GetOrdinal("Estado"))
            });
        }
        return lista;
    }

    public Rol? ObtenerPorId(int id)
    {
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand("SELECT IdRol, Nombre, Descripcion, Estado FROM Rol WHERE IdRol = @Id", cn);
        cmd.Parameters.AddWithValue("@Id", id);
        using var dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            return new Rol
            {
                IdRol = dr.GetInt32(dr.GetOrdinal("IdRol")),
                Nombre = dr.GetString(dr.GetOrdinal("Nombre")),
                Descripcion = dr.IsDBNull(dr.GetOrdinal("Descripcion")) ? string.Empty : dr.GetString(dr.GetOrdinal("Descripcion")),
                Estado = dr.GetBoolean(dr.GetOrdinal("Estado"))
            };
        }
        return null;
    }

    public void Insertar(Rol rol)
    {
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand("INSERT INTO Rol (Nombre, Descripcion, Estado) VALUES (@Nombre, @Descripcion, @Estado)", cn);
        cmd.Parameters.AddWithValue("@Nombre", rol.Nombre);
        cmd.Parameters.AddWithValue("@Descripcion", (object?)rol.Descripcion ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Estado", rol.Estado);
        cmd.ExecuteNonQuery();
    }

    public void Actualizar(Rol rol)
    {
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand("UPDATE Rol SET Nombre=@Nombre, Descripcion=@Descripcion, Estado=@Estado WHERE IdRol=@IdRol", cn);
        cmd.Parameters.AddWithValue("@IdRol", rol.IdRol);
        cmd.Parameters.AddWithValue("@Nombre", rol.Nombre);
        cmd.Parameters.AddWithValue("@Descripcion", (object?)rol.Descripcion ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Estado", rol.Estado);
        cmd.ExecuteNonQuery();
    }

    public void Eliminar(int id)
    {
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand("UPDATE Rol SET Estado = 0 WHERE IdRol = @Id", cn);
        cmd.Parameters.AddWithValue("@Id", id);
        cmd.ExecuteNonQuery();
    }
}