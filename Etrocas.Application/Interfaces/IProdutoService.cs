using ETrocas.Application.Requests;
using ETrocas.Application.Responses;
using ETrocas.Domain.Entities;

namespace ETrocas.Application.Interfaces;
//interface do ProdutoService que vai implementar o repository
public interface IProdutoService
{
    Task<CadastrarProdutoResponse> CadastrarProdutoAsync(CadastrarProdutoRequest cadastrarProdutoRequest, Guid usuarioGuid);
    Task DeletarProdutoAsync(Guid id);
    Task<Produtos> GetByIdAsync(Guid id);
    Task<IEnumerable<Produtos>> GetAllAsync();
}