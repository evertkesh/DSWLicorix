using LICORIX_PROYECT.Models;

namespace LICORIX_PROYECT.Data.Interfaces;

public interface IPromocionRepositorio
{
    List<Promocion> Listar();
    Promocion? ObtenerPorId(int id);
    int Insertar(Promocion promocion); 
    void Actualizar(Promocion promocion);
    void Eliminar(int id);

    
    void AgregarProductoAPromocion(int idProducto, int idPromocion);
    void RemoverProductoDePromocion(int idProducto, int idPromocion);
    void RemoverTodosProductosDePromocion(int idPromocion);
    List<int> ListarProductosPorPromocion(int idPromocion);
}