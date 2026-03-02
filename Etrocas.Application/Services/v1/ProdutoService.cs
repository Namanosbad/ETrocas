using ETrocas.Application.Interfaces;
using ETrocas.Application.Requests;
using ETrocas.Application.Responses;
using ETrocas.Domain.Entities;
using ETrocas.Domain.Interfaces;

namespace ETrocas.Application.Services.v1
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IRepository<Produtos> _repo;

        public ProdutoService(IProdutoRepository produtoRepository, IRepository<Produtos> repo)
        {
            _produtoRepository = produtoRepository;
            _repo = repo;
        }

        public async Task<CadastrarProdutoResponse> CadastrarProdutoAsync(CadastrarProdutoRequest cadastrarProdutoRequest, Guid usuarioGuid)
        {
            if (cadastrarProdutoRequest == null)
                throw new ArgumentNullException(nameof(cadastrarProdutoRequest));

            if (string.IsNullOrWhiteSpace(cadastrarProdutoRequest.Produto) ||
                string.IsNullOrWhiteSpace(cadastrarProdutoRequest.Tipo))
                throw new ArgumentException("Produto e tipo são obrigatórios.");

            var produtoParaCadastro = new Produtos
            {
                Id = Guid.NewGuid(),
                Produto = cadastrarProdutoRequest.Produto,
                Descricao = cadastrarProdutoRequest.Descricao,
                Valor = cadastrarProdutoRequest.Valor,
                ImageUrl = cadastrarProdutoRequest.Imagem,
                UsuarioId = usuarioGuid,
                Disponivel = true,
                Tipo = cadastrarProdutoRequest.Tipo
            };

            var produtoCriado = await _produtoRepository.CadastrarProdutoAsync(produtoParaCadastro);

            return new CadastrarProdutoResponse
            {
                Id = produtoCriado.Id,
                Produto = produtoCriado.Produto,
                Tipo = produtoCriado.Tipo,
                Valor = produtoCriado.Valor,
                Descricao = produtoCriado.Descricao,
                Disponivel = produtoCriado.Disponivel,
                DataCadastro = produtoCriado.DataCadastro,
                ImageUrl = produtoCriado.ImageUrl,
                UsuarioId = produtoCriado.UsuarioId,
            };
        }


        public async Task DeletarProdutoAsync(Guid id)
        {

            var exist = await _repo.ExistAsync(id);
            if (!exist)
                throw new InvalidOperationException("Produto nao encontrado.");

            await _repo.DeleteAsync(id);
        }

        public async Task<Produtos> GetByIdAsync(Guid id)
        {
            var produto = await _repo.GetByIdAsync(id);
            if (produto == null)
                throw new InvalidOperationException("Produto nao encontrado.");
            return produto;
        }

        public async Task<IEnumerable<Produtos>> GetAllAsync()
        {
            var produtos = await _repo.GetAllAsync();
            return produtos;
        }

        public async Task<AtualizarProdutoResponse> UpdateAsync(Guid id, AtualizarProdutoRequest updateRequest)
        {
            if (updateRequest == null)
                throw new ArgumentNullException(nameof(updateRequest));

            var produto = await _repo.GetByIdAsync(id);

            if (produto == null)
                throw new InvalidOperationException("Produto nao encontrado.");

            produto.Produto = updateRequest.Produto;
            produto.Tipo = updateRequest.Tipo;
            produto.Valor = updateRequest.Valor;
            produto.Descricao = updateRequest.Descricao;
            produto.Disponivel = updateRequest.Disponivel;
            produto.ImageUrl = updateRequest.ImageUrl;

            await _repo.UpdateAsync(produto);

            return new AtualizarProdutoResponse
            {
                Id = produto.Id,
                Produto = produto.Produto,
                Tipo = produto.Tipo,
                Valor = produto.Valor,
                Descricao = produto.Descricao,
                Disponivel = produto.Disponivel,
                ImageUrl = updateRequest.ImageUrl,
                UsuarioId = produto.UsuarioId,
            };
        }
    }
}
