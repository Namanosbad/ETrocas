using ETrocas.Domain.Entities;
using ETrocas.Domain.Interfaces;

//acesso e manipulação do banco de dados.
namespace ETrocas.Database.Repository
{
    //herda da interface
    public class ProdutoRepository : IProdutoRepository
    {
        //injetando dependencia.
        private readonly ETrocasDbContext _context;
        //construtor da dependencia.
        public ProdutoRepository(ETrocasDbContext context)
        {
            _context = context;
        }
        //como o banco vai lidar com o serviço/adicionar e salvar o produto
        public async Task<Produtos> CadastrarProdutoAsync(Produtos produtos)
        {
            _context.AddAsync(produtos);
            await _context.SaveChangesAsync(); 
            return produtos;
        }
    }
}
