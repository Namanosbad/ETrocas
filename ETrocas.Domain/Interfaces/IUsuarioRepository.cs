using ETrocas.Domain.Entities;

namespace ETrocas.Domain.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario> RegistrarUsuarioAsync(Usuario usuario);

    Task<Usuario>LoginUsuarioAsync (Usuario usuario);
} 