using Microsoft.Data.SqlClient;
using System.Data;
using LICORIX_PROYECT.Models;
using LICORIX_PROYECT.Data.Interfaces;

namespace LICORIX_PROYECT.Data;

public class CategoriaRepositorio : ICategoriaRepositorio
{
    private readonly ConexionBD _bd;

    public CategoriaRepositorio(ConexionBD bd)
    {
        _bd = bd;
    }

    public List<Categoria> Listar()
    {
        var lista = new List<Categoria>();

        using (var cn = _bd.ObtenerConexion())
        {
            cn.Open();
            using (var cmd = new SqlCommand("sp_ListarCategorias", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Categoria
                        {
                            IdCategoria = dr.GetInt32(dr.GetOrdinal("IdCategoria")),
                            Nombre = dr.GetString(dr.GetOrdinal("Nombre")),
                            Descripcion = dr.IsDBNull(dr.GetOrdinal("Descripcion")) ? string.Empty : dr.GetString(dr.GetOrdinal("Descripcion")),
                            ImagenURL = dr.IsDBNull(dr.GetOrdinal("ImagenURL")) ? "/img/categorias/default.png" : dr.GetString(dr.GetOrdinal("ImagenURL")),
                            Estado = dr.GetBoolean(dr.GetOrdinal("Estado"))
                        });
                    }
                }
            }
        }

        return lista;
    }

    public Categoria? ObtenerPorId(int id) => throw new NotImplementedException();
    public void Insertar(Categoria categoria) => throw new NotImplementedException();
    public void Actualizar(Categoria categoria) => throw new NotImplementedException();
    public void Eliminar(int id) => throw new NotImplementedException();
}