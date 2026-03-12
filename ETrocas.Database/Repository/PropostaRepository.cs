using ETrocas.Domain.Entities;
using ETrocas.Domain.Enums;
using ETrocas.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ETrocas.Database.Repository
{
    public class PropostaRepository : IPropostaRepository
    {
        private readonly ETrocasDbContext _eTrocasDbContext;

        public PropostaRepository(ETrocasDbContext eTrocasDbContext)
        {
            _eTrocasDbContext = eTrocasDbContext;
        }

        public async Task<Proposta> FazerPropostaAsync(Proposta proposta)
        {
            await _eTrocasDbContext.Propostas.AddAsync(proposta);
            await _eTrocasDbContext.SaveChangesAsync();
            return proposta;
        }

        public async Task<Proposta?> GetByIdAsync(Guid propostaId)
        {
            return await _eTrocasDbContext.Propostas
                .FirstOrDefaultAsync(p => p.Id == propostaId);
        }

        public async Task<IReadOnlyCollection<Proposta>> ListarEnviadasAsync(Guid usuarioId)
        {
            return await _eTrocasDbContext.Propostas
                .Where(p => p.UsuarioPropostaId == usuarioId)
                .OrderByDescending(p => p.DataProposta)
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<Proposta>> ListarRecebidasAsync(Guid usuarioId)
        {
            return await _eTrocasDbContext.Propostas
                .Where(p => p.UsuarioRecebedorId == usuarioId)
                .OrderByDescending(p => p.DataProposta)
                .ToListAsync();
        }

        public async Task<Proposta> AtualizarStatusAsync(Proposta proposta, EStatusProposta status)
        {
            proposta.StatusProposta = status;
            proposta.DataResposta = DateTime.UtcNow;
            _eTrocasDbContext.Propostas.Update(proposta);
            await _eTrocasDbContext.SaveChangesAsync();
            return proposta;
        }
    }
}
