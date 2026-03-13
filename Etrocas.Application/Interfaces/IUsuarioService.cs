using ETrocas.Application.Requests;
using ETrocas.Application.Responses;

namespace ETrocas.Application.Interfaces;

public interface IUsuarioService
{
    Task<RegistrarUsuarioResponse> RegistrarUsuarioAsync(RegistrarUsuarioRequest request);
    Task<LoginUsuarioResponse> LoginUsuarioAsync(LoginUsuarioRequest request);
    Task<IEnumerable<UsuarioResponse>> ListarUsuariosAsync();
    Task<UsuarioResponse> ObterUsuarioPorIdAsync(Guid usuarioId);
    Task<UsuarioResponse> AtualizarUsuarioAsync(Guid usuarioId, AtualizarUsuarioRequest request);
}
