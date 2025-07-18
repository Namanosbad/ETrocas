using ETrocas.Application.Requests;
using ETrocas.Application.Responses;

namespace ETrocas.Application.Interfaces;

public interface IUsuarioService
{
    Task<RegistrarUsuarioResponse> RegistrarUsuarioAsync(RegistrarUsuarioRequest request);
    Task<LoginUsuarioResponse>LoginUsuarioAsync(LoginUsuarioRequest request);
}