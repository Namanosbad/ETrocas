using ETrocas.Domain.Entities;

namespace ETrocas.Domain.Interfaces;

public interface IProdutoRepository
{
    Task<Produtos> CadastrarProdutoAsync(Produtos produtos);
}