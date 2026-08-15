using System;
using Microsoft.Data.SqlClient;
using System.Data;
using LICORIX_PROYECT.Models;
using LICORIX_PROYECT.Data.Interfaces;

namespace LICORIX_PROYECT.Data;

public class ProductoRepositorio : IProductoRepositorio
{
    private readonly ConexionBD _bd;
    public ProductoRepositorio(ConexionBD bd) => _bd = bd;

    public List<Producto> Listar()
    {
        var lista = new List<Producto>();
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand("sp_ListarProductos", cn);
        cmd.CommandType = CommandType.StoredProcedure;
        using var dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            lista.Add(MapProductoConJoin(dr));
        }
        return lista;
    }

    public Producto? ObtenerPorId(int id)
    {
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand("sp_ObtenerProductoPorId", cn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@IdProducto", id);
        using var dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            var p = new Producto
            {
                IdProducto = dr.GetInt32(dr.GetOrdinal("IdProducto")),
                Nombre = dr.GetString(dr.GetOrdinal("Nombre")),
                Descripcion = dr.IsDBNull(dr.GetOrdinal("Descripcion")) ? string.Empty : dr.GetString(dr.GetOrdinal("Descripcion")),
                Precio = dr.GetDecimal(dr.GetOrdinal("Precio")),
                Stock = dr.GetInt32(dr.GetOrdinal("Stock")),
                ImagenURL = dr.IsDBNull(dr.GetOrdinal("ImagenURL")) ? string.Empty : dr.GetString(dr.GetOrdinal("ImagenURL")),
                GraduacionAlcoholica = dr.IsDBNull(dr.GetOrdinal("GraduacionAlcoholica")) ? 0 : dr.GetDecimal(dr.GetOrdinal("GraduacionAlcoholica")),
                VolumenML = dr.IsDBNull(dr.GetOrdinal("VolumenML")) ? 750 : dr.GetInt32(dr.GetOrdinal("VolumenML")),
                FechaRegistro = dr.GetDateTime(dr.GetOrdinal("FechaRegistro")),
                Destacado = dr.GetBoolean(dr.GetOrdinal("Destacado")),
                Estado = dr.GetBoolean(dr.GetOrdinal("Estado")),
                IdCategoria = dr.GetInt32(dr.GetOrdinal("IdCategoria")),
                IdMarca = dr.GetInt32(dr.GetOrdinal("IdMarca"))
            };
            return p;
        }
        return null;
    }

    public void Insertar(Producto producto)
    {
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand("sp_InsertarProducto", cn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
        cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion ?? string.Empty);
        cmd.Parameters.AddWithValue("@Precio", producto.Precio);
        cmd.Parameters.AddWithValue("@Stock", producto.Stock);
        cmd.Parameters.AddWithValue("@ImagenURL", (object?)producto.ImagenURL ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@GraduacionAlcoholica", producto.GraduacionAlcoholica);
        cmd.Parameters.AddWithValue("@VolumenML", producto.VolumenML);
        cmd.Parameters.AddWithValue("@Destacado", producto.Destacado);
        cmd.Parameters.AddWithValue("@IdCategoria", producto.IdCategoria);
        cmd.Parameters.AddWithValue("@IdMarca", producto.IdMarca);
        cmd.ExecuteNonQuery();
    }

    public void Actualizar(Producto producto)
    {
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand("sp_ActualizarProducto", cn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@IdProducto", producto.IdProducto);
        cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
        cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion ?? string.Empty);
        cmd.Parameters.AddWithValue("@Precio", producto.Precio);
        cmd.Parameters.AddWithValue("@Stock", producto.Stock);
        cmd.Parameters.AddWithValue("@ImagenURL", (object?)producto.ImagenURL ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@GraduacionAlcoholica", producto.GraduacionAlcoholica);
        cmd.Parameters.AddWithValue("@VolumenML", producto.VolumenML);
        cmd.Parameters.AddWithValue("@Destacado", producto.Destacado);
        cmd.Parameters.AddWithValue("@IdCategoria", producto.IdCategoria);
        cmd.Parameters.AddWithValue("@IdMarca", producto.IdMarca);
        cmd.ExecuteNonQuery();
    }

    public void Eliminar(int id)
    {
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand("sp_EliminarProducto", cn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@IdProducto", id);
        cmd.ExecuteNonQuery();
    }

    

    public List<Producto> ListarDestacados()
    {
        var lista = new List<Producto>();
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand("sp_ListarProductosDestacados", cn);
        cmd.CommandType = CommandType.StoredProcedure;
        using var dr = cmd.ExecuteReader();
        while (dr.Read()) lista.Add(MapProducto(dr));
        return lista;
    }

    public List<Producto> ListarNuevosIngresos()
    {
        var lista = new List<Producto>();
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand("sp_ListarNuevosIngresos", cn);
        cmd.CommandType = CommandType.StoredProcedure;
        using var dr = cmd.ExecuteReader();
        while (dr.Read()) lista.Add(MapProducto(dr));
        return lista;
    }

    public List<ProductoEnOferta> ListarEnOferta()
    {
        var lista = new List<ProductoEnOferta>();
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand("sp_ListarProductosEnOferta", cn);
        cmd.CommandType = CommandType.StoredProcedure;
        using var dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            var p = new ProductoEnOferta
            {
                IdProducto = dr.GetInt32(dr.GetOrdinal("IdProducto")),
                Nombre = dr.GetString(dr.GetOrdinal("Nombre")),
                Precio = dr.GetDecimal(dr.GetOrdinal("Precio")),
                Promocion = dr.IsDBNull(dr.GetOrdinal("Promocion")) ? string.Empty : dr.GetString(dr.GetOrdinal("Promocion")),
                TipoDescuento = dr.IsDBNull(dr.GetOrdinal("TipoDescuento")) ? "p" : dr.GetString(dr.GetOrdinal("TipoDescuento")),
                ValorDescuento = dr.IsDBNull(dr.GetOrdinal("ValorDescuento")) ? 0 : dr.GetDecimal(dr.GetOrdinal("ValorDescuento"))
            };
            p.PrecioConDescuento = p.TipoDescuento == "p"
                ? p.Precio - (p.Precio * p.ValorDescuento / 100m)
                : Math.Max(0, p.Precio - p.ValorDescuento);
            lista.Add(p);
        }
        return lista;
    }

    public List<Producto> BuscarPorTexto(string texto)
    {
        var lista = new List<Producto>();
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand("sp_BuscarProductos", cn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Texto", texto ?? string.Empty);
        using var dr = cmd.ExecuteReader();
        while (dr.Read()) lista.Add(MapProducto(dr));
        return lista;
    }

    public List<Producto> ListarPorCategoria(int idCategoria)
    {
        var lista = new List<Producto>();
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand("sp_ListarProductosPorCategoria", cn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@IdCategoria", idCategoria);
        using var dr = cmd.ExecuteReader();
        while (dr.Read()) lista.Add(MapProducto(dr));
        return lista;
    }

    

    public List<Producto> ListarPaginado(int page, int pageSize, out int totalCount)
    {
        var lista = new List<Producto>();
        totalCount = 0;
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand("sp_ListarProductosPaginado", cn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Page", page);
        cmd.Parameters.AddWithValue("@PageSize", pageSize);
        var pTotal = new SqlParameter("@TotalCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
        cmd.Parameters.Add(pTotal);
        using var dr = cmd.ExecuteReader();
        while (dr.Read()) lista.Add(MapProductoConJoin(dr));
        dr.Close();
        totalCount = pTotal.Value == DBNull.Value ? 0 : (int)pTotal.Value;
        return lista;
    }

    

    private static int Ord(SqlDataReader dr, string col)
    {
        try { return dr.GetOrdinal(col); }
        catch (IndexOutOfRangeException) { return -1; }
    }

    private static string? GetStr(SqlDataReader dr, string col)
    {
        var o = Ord(dr, col);
        if (o < 0 || dr.IsDBNull(o)) return null;
        return dr.GetString(o);
    }

    private static int GetInt(SqlDataReader dr, string col, int def = 0)
    {
        var o = Ord(dr, col);
        if (o < 0 || dr.IsDBNull(o)) return def;
        return dr.GetInt32(o);
    }

    private static decimal GetDec(SqlDataReader dr, string col, decimal def = 0)
    {
        var o = Ord(dr, col);
        if (o < 0 || dr.IsDBNull(o)) return def;
        return dr.GetDecimal(o);
    }

    private static bool GetBool(SqlDataReader dr, string col, bool def = false)
    {
        var o = Ord(dr, col);
        if (o < 0 || dr.IsDBNull(o)) return def;
        return dr.GetBoolean(o);
    }

    private static DateTime GetDt(SqlDataReader dr, string col, DateTime? def = null)
    {
        var o = Ord(dr, col);
        if (o < 0 || dr.IsDBNull(o)) return def ?? DateTime.Now;
        return dr.GetDateTime(o);
    }

    private static Producto MapProducto(SqlDataReader dr)
    {
        return new Producto
        {
            IdProducto = GetInt(dr, "IdProducto"),
            Nombre = GetStr(dr, "Nombre") ?? string.Empty,
            Descripcion = GetStr(dr, "Descripcion") ?? string.Empty,
            Precio = GetDec(dr, "Precio"),
            Stock = GetInt(dr, "Stock"),
            ImagenURL = GetStr(dr, "ImagenURL") ?? string.Empty,
            GraduacionAlcoholica = GetDec(dr, "GraduacionAlcoholica"),
            VolumenML = GetInt(dr, "VolumenML", 750),
            FechaRegistro = GetDt(dr, "FechaRegistro"),
            Destacado = GetBool(dr, "Destacado"),
            Estado = GetBool(dr, "Estado", true),
            IdCategoria = GetInt(dr, "IdCategoria"),
            IdMarca = GetInt(dr, "IdMarca")
        };
    }

    
    private static Producto MapProductoConJoin(SqlDataReader dr)
    {
        var p = MapProducto(dr);
        
        return p;
    }
}