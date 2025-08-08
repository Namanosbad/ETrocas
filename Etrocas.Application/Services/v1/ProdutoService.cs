using ETrocas.Application.Interfaces;
using ETrocas.Application.Requests;
using ETrocas.Application.Responses;
using ETrocas.Domain.Entities;
using ETrocas.Domain.Interfaces;
//logica do negocio
namespace ETrocas.Application.Services.v1
{
    //herda da interface
    public class ProdutoService : IProdutoService
    {
        //injeção de dependencia
        private readonly IProdutoRepository _produtoRepository;
        private readonly IRepository<Produtos> _repo;

        public ProdutoService(IProdutoRepository produtoRepository, IRepository<Produtos> repo)
        {
            _produtoRepository = produtoRepository;
            _repo = repo;
        }

        //logica do cadastro de produto.
        public async Task<CadastrarProdutoResponse> CadastrarProdutoAsync(CadastrarProdutoRequest cadastrarProdutoRequest, Guid usuarioGuid)
        {
            //Aqui eu cadastro o produto a linha 23 é o objeto de entrada que representa o request.
            var CadastrarProduto = new Produtos
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

            //Aqui eu salvo no banco esse produto cadastrado. Uso o _produtoRepository e o metodo no banco para lidar com o banco 
            var ProdutoCriado = await _produtoRepository.CadastrarProdutoAsync(CadastrarProduto);

            //O que vai retornar pro usuario.
            return new CadastrarProdutoResponse
            {
                Id = ProdutoCriado.Id,
                Produto = ProdutoCriado.Produto,
                Tipo = ProdutoCriado.Tipo,
                Valor = ProdutoCriado.Valor,
                Descricao = ProdutoCriado.Descricao,
                Disponivel = ProdutoCriado.Disponivel,
                DataCadastro = ProdutoCriado.DataCadastro,
                ImageUrl = ProdutoCriado.ImageUrl,
                UsuarioId = ProdutoCriado.UsuarioId,
            };
        }


        //logica para deletar um produto.
        public async Task DeletarProdutoAsync(Guid id)
        {

            var exist = await _repo.ExistAsync(id);
            if (!exist)
                throw new InvalidOperationException("Produto nao encontrado.");

            await _repo.DeleteAsync(id);
        }

        //logica para puxar produto pelo Id
        public async Task<Produtos> GetByIdAsync(Guid id)
        {
            var produto = await _repo.GetByIdAsync(id);
            if (produto == null)
                throw new InvalidOperationException("Produto nao encontrado.");
            return produto;
        }

        //logica para puxar todos os produtos.
        public async Task<IEnumerable<Produtos>> GetAllAsync()
        {
            var produtos = await _repo.GetAllAsync();
            return produtos;
        }
    }
}