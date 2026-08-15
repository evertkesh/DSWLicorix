using Microsoft.Data.SqlClient;
using LICORIX_PROYECT.Models;
using LICORIX_PROYECT.Data.Interfaces;

namespace LICORIX_PROYECT.Data;

public class UsuarioRepositorio : IUsuarioRepositorio
{
    private readonly ConexionBD _bd;
    public UsuarioRepositorio(ConexionBD bd) => _bd = bd;

    public List<Usuario> Listar()
    {
        var lista = new List<Usuario>();
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand(@"SELECT u.IdUsuario, u.Nombres, u.Apellidos, u.Correo, u.Contrasena,
                                                u.Telefono, u.Direccion, u.FechaRegistro, u.Estado, u.IdRol, r.Nombre AS NombreRol
                                         FROM Usuario u INNER JOIN Rol r ON u.IdRol = r.IdRol
                                         WHERE u.Estado = 1 ORDER BY u.Nombres", cn);
        using var dr = cmd.ExecuteReader();
        while (dr.Read()) lista.Add(Map(dr));
        return lista;
    }

    public Usuario? ObtenerPorId(int id)
    {
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand(@"SELECT u.IdUsuario, u.Nombres, u.Apellidos, u.Correo, u.Contrasena,
                                                u.Telefono, u.Direccion, u.FechaRegistro, u.Estado, u.IdRol, r.Nombre AS NombreRol
                                         FROM Usuario u INNER JOIN Rol r ON u.IdRol = r.IdRol
                                         WHERE u.IdUsuario = @Id", cn);
        cmd.Parameters.AddWithValue("@Id", id);
        using var dr = cmd.ExecuteReader();
        if (dr.Read()) return Map(dr);
        return null;
    }

    public Usuario? ValidarLogin(string correo, string contrasena)
    {
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand(@"SELECT u.IdUsuario, u.Nombres, u.Apellidos, u.Correo, u.Contrasena,
                                                u.Telefono, u.Direccion, u.FechaRegistro, u.Estado, u.IdRol, r.Nombre AS NombreRol
                                         FROM Usuario u INNER JOIN Rol r ON u.IdRol = r.IdRol
                                         WHERE u.Correo = @Correo AND u.Contrasena = @Contrasena AND u.Estado = 1", cn);
        cmd.Parameters.AddWithValue("@Correo", correo);
        cmd.Parameters.AddWithValue("@Contrasena", contrasena);
        using var dr = cmd.ExecuteReader();
        if (dr.Read()) return Map(dr);
        return null;
    }

    public bool ExisteCorreo(string correo)
    {
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand("SELECT COUNT(*) FROM Usuario WHERE Correo = @Correo", cn);
        cmd.Parameters.AddWithValue("@Correo", correo);
        return (int)cmd.ExecuteScalar() > 0;
    }

    public void Insertar(Usuario usuario)
    {
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand(@"INSERT INTO Usuario (Nombres, Apellidos, Correo, Contrasena, Telefono, Direccion, FechaRegistro, Estado, IdRol)
                                        VALUES (@Nombres, @Apellidos, @Correo, @Contrasena, @Telefono, @Direccion, GETDATE(), 1, @IdRol)", cn);
        cmd.Parameters.AddWithValue("@Nombres", usuario.Nombres);
        cmd.Parameters.AddWithValue("@Apellidos", usuario.Apellidos);
        cmd.Parameters.AddWithValue("@Correo", usuario.Correo);
        cmd.Parameters.AddWithValue("@Contrasena", usuario.Contrasena);
        cmd.Parameters.AddWithValue("@Telefono", (object?)usuario.Telefono ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Direccion", (object?)usuario.Direccion ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@IdRol", usuario.IdRol);
        cmd.ExecuteNonQuery();
    }

    public void Actualizar(Usuario usuario)
    {
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand(@"UPDATE Usuario SET Nombres=@Nombres, Apellidos=@Apellidos, Correo=@Correo,
                                        Contrasena=@Contrasena, Telefono=@Telefono, Direccion=@Direccion, Estado=@Estado, IdRol=@IdRol
                                        WHERE IdUsuario=@IdUsuario", cn);
        cmd.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);
        cmd.Parameters.AddWithValue("@Nombres", usuario.Nombres);
        cmd.Parameters.AddWithValue("@Apellidos", usuario.Apellidos);
        cmd.Parameters.AddWithValue("@Correo", usuario.Correo);
        cmd.Parameters.AddWithValue("@Contrasena", usuario.Contrasena);
        cmd.Parameters.AddWithValue("@Telefono", (object?)usuario.Telefono ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Direccion", (object?)usuario.Direccion ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Estado", usuario.Estado);
        cmd.Parameters.AddWithValue("@IdRol", usuario.IdRol);
        cmd.ExecuteNonQuery();
    }

    public void Eliminar(int id)
    {
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand("UPDATE Usuario SET Estado = 0 WHERE IdUsuario = @Id", cn);
        cmd.Parameters.AddWithValue("@Id", id);
        cmd.ExecuteNonQuery();
    }

    public List<Usuario> ListarPaginado(int page, int pageSize, out int totalCount)
    {
        var lista = new List<Usuario>();
        totalCount = 0;
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand("sp_ListarUsuariosPaginado", cn);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Page", page);
        cmd.Parameters.AddWithValue("@PageSize", pageSize);
        var pTotal = new Microsoft.Data.SqlClient.SqlParameter("@TotalCount", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
        cmd.Parameters.Add(pTotal);
        using var dr = cmd.ExecuteReader();
        
        static string GetOptionalString(SqlDataReader reader, string col)
        {
            try
            {
                var ord = reader.GetOrdinal(col);
                if (ord < 0 || reader.IsDBNull(ord)) return string.Empty;
                return reader.GetString(ord);
            }
            catch
            {
                return string.Empty;
            }
        }

        while (dr.Read())
        {
            lista.Add(new Usuario
            {
                IdUsuario = dr.GetInt32(dr.GetOrdinal("IdUsuario")),
                Nombres = GetOptionalString(dr, "Nombres"),
                Apellidos = GetOptionalString(dr, "Apellidos"),
                Correo = GetOptionalString(dr, "Correo"),
                Contrasena = GetOptionalString(dr, "Contrasena"),
                Telefono = GetOptionalString(dr, "Telefono"),
                Direccion = GetOptionalString(dr, "Direccion"),
                FechaRegistro = dr.IsDBNull(dr.GetOrdinal("FechaRegistro")) ? System.DateTime.Now : dr.GetDateTime(dr.GetOrdinal("FechaRegistro")),
                Estado = dr.IsDBNull(dr.GetOrdinal("Estado")) ? true : dr.GetBoolean(dr.GetOrdinal("Estado")),
                IdRol = dr.IsDBNull(dr.GetOrdinal("IdRol")) ? 2 : dr.GetInt32(dr.GetOrdinal("IdRol")),
                NombreRol = GetOptionalString(dr, "NombreRol")
            });
        }
        dr.Close();
        totalCount = pTotal.Value == DBNull.Value ? 0 : (int)pTotal.Value;
        return lista;
    }

    public void CrearAdministradorSP(int idUsuarioSolicitante, Usuario usuario)
    {
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand("sp_CrearAdministrador", cn);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@IdUsuarioSolicitante", idUsuarioSolicitante);
        cmd.Parameters.AddWithValue("@Nombres", usuario.Nombres ?? string.Empty);
        cmd.Parameters.AddWithValue("@Apellidos", usuario.Apellidos ?? string.Empty);
        cmd.Parameters.AddWithValue("@Correo", usuario.Correo ?? string.Empty);
        cmd.Parameters.AddWithValue("@Contrasena", usuario.Contrasena ?? string.Empty);
        cmd.Parameters.AddWithValue("@Telefono", (object?)usuario.Telefono ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Direccion", (object?)usuario.Direccion ?? DBNull.Value);
        cmd.ExecuteNonQuery();
    }

    public void PromoverAAdministradorSP(int idUsuarioSolicitante, int idUsuarioTarget)
    {
        var u = ObtenerPorId(idUsuarioTarget);
        if (u == null) throw new InvalidOperationException("Usuario objetivo no encontrado.");
        
        CrearAdministradorSP(idUsuarioSolicitante, u);
    }

    public void RemoverAdministradorSP(int idUsuarioSolicitante, int idUsuarioTarget)
    {
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand("sp_RemoverAdministrador", cn);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@IdUsuarioSolicitante", idUsuarioSolicitante);
        cmd.Parameters.AddWithValue("@IdUsuarioTarget", idUsuarioTarget);
        cmd.ExecuteNonQuery();
    }

    public void DesactivarUsuarioSP(int idUsuarioSolicitante, int idUsuarioTarget)
    {
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand("sp_DesactivarUsuario", cn);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@IdUsuarioSolicitante", idUsuarioSolicitante);
        cmd.Parameters.AddWithValue("@IdUsuarioTarget", idUsuarioTarget);
        cmd.ExecuteNonQuery();
    }

    public void ActivarUsuarioSP(int idUsuarioSolicitante, int idUsuarioTarget)
    {
        using var cn = _bd.ObtenerConexion();
        cn.Open();
        using var cmd = new SqlCommand("sp_ActivarUsuario", cn);
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@IdUsuarioSolicitante", idUsuarioSolicitante);
        cmd.Parameters.AddWithValue("@IdUsuarioTarget", idUsuarioTarget);
        cmd.ExecuteNonQuery();
    }

    private static Usuario Map(SqlDataReader dr) => new()
    {
        IdUsuario = dr.GetInt32(dr.GetOrdinal("IdUsuario")),
        Nombres = dr.GetString(dr.GetOrdinal("Nombres")),
        Apellidos = dr.GetString(dr.GetOrdinal("Apellidos")),
        Correo = dr.GetString(dr.GetOrdinal("Correo")),
        Contrasena = dr.GetString(dr.GetOrdinal("Contrasena")),
        Telefono = dr.IsDBNull(dr.GetOrdinal("Telefono")) ? string.Empty : dr.GetString(dr.GetOrdinal("Telefono")),
        Direccion = dr.IsDBNull(dr.GetOrdinal("Direccion")) ? string.Empty : dr.GetString(dr.GetOrdinal("Direccion")),
        FechaRegistro = dr.GetDateTime(dr.GetOrdinal("FechaRegistro")),
        Estado = dr.GetBoolean(dr.GetOrdinal("Estado")),
        IdRol = dr.GetInt32(dr.GetOrdinal("IdRol")),
        NombreRol = dr.IsDBNull(dr.GetOrdinal("NombreRol")) ? string.Empty : dr.GetString(dr.GetOrdinal("NombreRol"))
    };
}