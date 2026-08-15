using LICORIX_PROYECT.Models;

namespace LICORIX_PROYECT.Data.Interfaces;

public interface IRolRepositorio
{
    List<Rol> Listar();
    Rol? ObtenerPorId(int id);
    void Insertar(Rol rol);
    void Actualizar(Rol rol);
    void Eliminar(int id);
}