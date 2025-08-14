using ETrocas.Domain.Entities;
using ETrocas.Domain.Interfaces;

namespace ETrocas.Database.Repository
{
    public class PropostaRepository : IPropostaRepository
    {
        //injeção de deprendencia.
        private readonly ETrocasDbContext _eTrocasDbContext;

        public PropostaRepository(ETrocasDbContext eTrocasDbContext)
        {
            _eTrocasDbContext = eTrocasDbContext;
        }

        //logica de manipulação no banco de dados.
        public async Task<Proposta> FazerPropostaAsync(Proposta proposta)
        {
            //adiciono essa proposta no b.d e salvo ela.
            await _eTrocasDbContext.AddAsync(proposta);
            await _eTrocasDbContext.SaveChangesAsync();
            return proposta;
        }
    }
}