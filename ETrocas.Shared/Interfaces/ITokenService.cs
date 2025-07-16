using ETrocas.Domain.Entities;

namespace ETrocas.Ioc;

public interface ITokenService
{
    public string Gerar(Usuario usuario);
}