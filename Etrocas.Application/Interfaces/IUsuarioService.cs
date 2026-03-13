using ETrocas.Application.Requests;
using ETrocas.Application.Responses;

namespace ETrocas.Application.Interfaces;

public interface IUsuarioService
{
    Task<RegistrarUsuarioResponse> RegistrarUsuarioAsync(RegistrarUsuarioRequest request);
    Task<LoginUsuarioResponse> LoginUsuarioAsync(LoginUsuarioRequest request);
    Task<UsuariosPaginadosResponse> ListarUsuariosAsync(int pagina, int tamanhoPagina);
    Task<UsuarioPublicoResponse> ObterUsuarioPorIdAsync(Guid usuarioId);
    Task<UsuarioResponse> ObterPerfilUsuarioAsync(Guid usuarioId);
    Task<UsuarioResponse> AtualizarUsuarioAsync(Guid usuarioId, AtualizarUsuarioRequest request);
}
