using LICORIX_PROYECT.Models;

namespace LICORIX_PROYECT.Data.Interfaces;

public interface IMarcaRepositorio
{
    List<Marca> Listar();
    Marca? ObtenerPorId(int id);
    void Insertar(Marca marca);
    void Actualizar(Marca marca);
    void Eliminar(int id);
}