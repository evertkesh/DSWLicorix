using LICORIX_PROYECT.Models;

namespace LICORIX_PROYECT.Data.Interfaces;

public interface IProductoRepositorio
{
    List<Producto> Listar();
    Producto? ObtenerPorId(int id);
    void Insertar(Producto producto);
    void Actualizar(Producto producto);
    void Eliminar(int id);

    
    List<Producto> ListarPaginado(int page, int pageSize, out int totalCount);

    
    List<Producto> ListarDestacados();
    List<Producto> ListarNuevosIngresos();
    List<ProductoEnOferta> ListarEnOferta();
    List<Producto> BuscarPorTexto(string texto);
    List<Producto> ListarPorCategoria(int idCategoria);
}