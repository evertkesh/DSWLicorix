using Microsoft.Data.SqlClient;
using LICORIX_PROYECT.Models;
using LICORIX_PROYECT.Data.Interfaces;

namespace LICORIX_PROYECT.Data;

public class PromocionRepositorio : IPromocionRepositorio
{
    private readonly ConexionBD _bd;
    public PromocionRepositorio(ConexionBD bd) => _bd = bd;

    public List<Promocion> Listar()
    {
        var lista = new List<Promocion>();
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand("SELECT IdPromocion, Nombre, Descripcion, TipoDescuento, ValorDescuento, FechaInicio, FechaFin, Estado FROM Promocion WHERE Estado = 1 ORDER BY FechaInicio DESC", cn);
        using var dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            lista.Add(Map(dr));
        }
        return lista;
    }

    public Promocion? ObtenerPorId(int id)
    {
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand("SELECT IdPromocion, Nombre, Descripcion, TipoDescuento, ValorDescuento, FechaInicio, FechaFin, Estado FROM Promocion WHERE IdPromocion = @Id", cn);
        cmd.Parameters.AddWithValue("@Id", id);
        using var dr = cmd.ExecuteReader();
        if (dr.Read()) return Map(dr);
        return null;
    }

    public int Insertar(Promocion promocion)
    {
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand(@"INSERT INTO Promocion (Nombre, Descripcion, TipoDescuento, ValorDescuento, FechaInicio, FechaFin, Estado)
                                        OUTPUT INSERTED.IdPromocion
                                        VALUES (@Nombre, @Descripcion, @TipoDescuento, @ValorDescuento, @FechaInicio, @FechaFin, @Estado)", cn);
        cmd.Parameters.AddWithValue("@Nombre", promocion.Nombre);
        cmd.Parameters.AddWithValue("@Descripcion", (object?)promocion.Descripcion ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@TipoDescuento", promocion.TipoDescuento);
        cmd.Parameters.AddWithValue("@ValorDescuento", promocion.ValorDescuento);
        cmd.Parameters.AddWithValue("@FechaInicio", promocion.FechaInicio);
        cmd.Parameters.AddWithValue("@FechaFin", promocion.FechaFin);
        cmd.Parameters.AddWithValue("@Estado", promocion.Estado);
        var result = cmd.ExecuteScalar();
        return Convert.ToInt32(result);
    }

    public void AgregarProductoAPromocion(int idProducto, int idPromocion)
    {
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand(@"IF NOT EXISTS(SELECT 1 FROM ProductoPromocion WHERE IdProducto = @IdProducto AND IdPromocion = @IdPromocion)
                                        INSERT INTO ProductoPromocion (IdProducto, IdPromocion) VALUES (@IdProducto, @IdPromocion)", cn);
        cmd.Parameters.AddWithValue("@IdProducto", idProducto);
        cmd.Parameters.AddWithValue("@IdPromocion", idPromocion);
        cmd.ExecuteNonQuery();
    }

    public void RemoverProductoDePromocion(int idProducto, int idPromocion)
    {
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand("DELETE FROM ProductoPromocion WHERE IdProducto = @IdProducto AND IdPromocion = @IdPromocion", cn);
        cmd.Parameters.AddWithValue("@IdProducto", idProducto);
        cmd.Parameters.AddWithValue("@IdPromocion", idPromocion);
        cmd.ExecuteNonQuery();
    }

    public void RemoverTodosProductosDePromocion(int idPromocion)
    {
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand("DELETE FROM ProductoPromocion WHERE IdPromocion = @IdPromocion", cn);
        cmd.Parameters.AddWithValue("@IdPromocion", idPromocion);
        cmd.ExecuteNonQuery();
    }

    public List<int> ListarProductosPorPromocion(int idPromocion)
    {
        var lista = new List<int>();
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand("SELECT IdProducto FROM ProductoPromocion WHERE IdPromocion = @IdPromocion", cn);
        cmd.Parameters.AddWithValue("@IdPromocion", idPromocion);
        using var dr = cmd.ExecuteReader();
        while (dr.Read()) lista.Add(dr.GetInt32(0));
        return lista;
    }

    public void Actualizar(Promocion promocion)
    {
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand(@"UPDATE Promocion SET Nombre=@Nombre, Descripcion=@Descripcion, TipoDescuento=@TipoDescuento,
                                        ValorDescuento=@ValorDescuento, FechaInicio=@FechaInicio, FechaFin=@FechaFin, Estado=@Estado
                                        WHERE IdPromocion=@IdPromocion", cn);
        cmd.Parameters.AddWithValue("@IdPromocion", promocion.IdPromocion);
        cmd.Parameters.AddWithValue("@Nombre", promocion.Nombre);
        cmd.Parameters.AddWithValue("@Descripcion", (object?)promocion.Descripcion ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@TipoDescuento", promocion.TipoDescuento);
        cmd.Parameters.AddWithValue("@ValorDescuento", promocion.ValorDescuento);
        cmd.Parameters.AddWithValue("@FechaInicio", promocion.FechaInicio);
        cmd.Parameters.AddWithValue("@FechaFin", promocion.FechaFin);
        cmd.Parameters.AddWithValue("@Estado", promocion.Estado);
        cmd.ExecuteNonQuery();
    }

    public void Eliminar(int id)
    {
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand("UPDATE Promocion SET Estado = 0 WHERE IdPromocion = @Id", cn);
        cmd.Parameters.AddWithValue("@Id", id);
        cmd.ExecuteNonQuery();
    }

    private static Promocion Map(SqlDataReader dr) => new()
    {
        IdPromocion = dr.GetInt32(dr.GetOrdinal("IdPromocion")),
        Nombre = dr.GetString(dr.GetOrdinal("Nombre")),
        Descripcion = dr.IsDBNull(dr.GetOrdinal("Descripcion")) ? string.Empty : dr.GetString(dr.GetOrdinal("Descripcion")),
        TipoDescuento = dr.GetString(dr.GetOrdinal("TipoDescuento")),
        ValorDescuento = dr.GetDecimal(dr.GetOrdinal("ValorDescuento")),
        FechaInicio = dr.GetDateTime(dr.GetOrdinal("FechaInicio")),
        FechaFin = dr.GetDateTime(dr.GetOrdinal("FechaFin")),
        Estado = dr.GetBoolean(dr.GetOrdinal("Estado"))
    };
}