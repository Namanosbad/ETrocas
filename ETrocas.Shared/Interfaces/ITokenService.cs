using ETrocas.Domain.Entities;

namespace ETrocas.Shared.Interfaces;

public interface ITokenService
{
    public string Gerar(Usuario usuario);
}