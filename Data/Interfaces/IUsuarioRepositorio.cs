using LICORIX_PROYECT.Models;

namespace LICORIX_PROYECT.Data.Interfaces;

public interface IUsuarioRepositorio
{
    List<Usuario> Listar();
    List<Usuario> ListarPaginado(int page, int pageSize, out int totalCount);

    
    void CrearAdministradorSP(int idUsuarioSolicitante, Usuario usuario);
    void PromoverAAdministradorSP(int idUsuarioSolicitante, int idUsuarioTarget);
    void RemoverAdministradorSP(int idUsuarioSolicitante, int idUsuarioTarget);
    void DesactivarUsuarioSP(int idUsuarioSolicitante, int idUsuarioTarget);
    void ActivarUsuarioSP(int idUsuarioSolicitante, int idUsuarioTarget);
    Usuario? ObtenerPorId(int id);
    Usuario? ValidarLogin(string correo, string contrasena);
    bool ExisteCorreo(string correo);
    void Insertar(Usuario usuario);
    void Actualizar(Usuario usuario);
    void Eliminar(int id);
}