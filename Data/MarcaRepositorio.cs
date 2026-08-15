using Microsoft.Data.SqlClient;
using LICORIX_PROYECT.Models;
using LICORIX_PROYECT.Data.Interfaces;

namespace LICORIX_PROYECT.Data;

public class MarcaRepositorio : IMarcaRepositorio
{
    private readonly ConexionBD _bd;
    public MarcaRepositorio(ConexionBD bd) => _bd = bd;

    public List<Marca> Listar()
    {
        var lista = new List<Marca>();
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand("SELECT IdMarca, Nombre, PaisOrigen, Descripcion, Estado FROM Marca WHERE Estado = 1 ORDER BY Nombre", cn);
        using var dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            lista.Add(new Marca
            {
                IdMarca = dr.GetInt32(dr.GetOrdinal("IdMarca")),
                Nombre = dr.GetString(dr.GetOrdinal("Nombre")),
                PaisOrigen = dr.IsDBNull(dr.GetOrdinal("PaisOrigen")) ? string.Empty : dr.GetString(dr.GetOrdinal("PaisOrigen")),
                Descripcion = dr.IsDBNull(dr.GetOrdinal("Descripcion")) ? string.Empty : dr.GetString(dr.GetOrdinal("Descripcion")),
                Estado = dr.GetBoolean(dr.GetOrdinal("Estado"))
            });
        }
        return lista;
    }

    public Marca? ObtenerPorId(int id)
    {
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand("SELECT IdMarca, Nombre, PaisOrigen, Descripcion, Estado FROM Marca WHERE IdMarca = @Id", cn);
        cmd.Parameters.AddWithValue("@Id", id);
        using var dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            return new Marca
            {
                IdMarca = dr.GetInt32(dr.GetOrdinal("IdMarca")),
                Nombre = dr.GetString(dr.GetOrdinal("Nombre")),
                PaisOrigen = dr.IsDBNull(dr.GetOrdinal("PaisOrigen")) ? string.Empty : dr.GetString(dr.GetOrdinal("PaisOrigen")),
                Descripcion = dr.IsDBNull(dr.GetOrdinal("Descripcion")) ? string.Empty : dr.GetString(dr.GetOrdinal("Descripcion")),
                Estado = dr.GetBoolean(dr.GetOrdinal("Estado"))
            };
        }
        return null;
    }

    public void Insertar(Marca marca)
    {
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand(@"INSERT INTO Marca (Nombre, PaisOrigen, Descripcion, Estado)
                                        VALUES (@Nombre, @PaisOrigen, @Descripcion, @Estado)", cn);
        cmd.Parameters.AddWithValue("@Nombre", marca.Nombre);
        cmd.Parameters.AddWithValue("@PaisOrigen", (object?)marca.PaisOrigen ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Descripcion", (object?)marca.Descripcion ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Estado", marca.Estado);
        cmd.ExecuteNonQuery();
    }

    public void Actualizar(Marca marca)
    {
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand(@"UPDATE Marca SET Nombre=@Nombre, PaisOrigen=@PaisOrigen,
                                        Descripcion=@Descripcion, Estado=@Estado WHERE IdMarca=@IdMarca", cn);
        cmd.Parameters.AddWithValue("@IdMarca", marca.IdMarca);
        cmd.Parameters.AddWithValue("@Nombre", marca.Nombre);
        cmd.Parameters.AddWithValue("@PaisOrigen", (object?)marca.PaisOrigen ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Descripcion", (object?)marca.Descripcion ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Estado", marca.Estado);
        cmd.ExecuteNonQuery();
    }

    public void Eliminar(int id)
    {
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand("UPDATE Marca SET Estado = 0 WHERE IdMarca = @Id", cn);
        cmd.Parameters.AddWithValue("@Id", id);
        cmd.ExecuteNonQuery();
    }
}