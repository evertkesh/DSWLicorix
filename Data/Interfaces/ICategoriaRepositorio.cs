using LICORIX_PROYECT.Models;

namespace LICORIX_PROYECT.Data.Interfaces;

public interface ICategoriaRepositorio
{
    List<Categoria> Listar();
    Categoria? ObtenerPorId(int id);
    void Insertar(Categoria categoria);     
    void Eliminar(int id);
}