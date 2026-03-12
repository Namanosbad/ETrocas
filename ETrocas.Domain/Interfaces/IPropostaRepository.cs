using ETrocas.Domain.Entities;
using ETrocas.Domain.Enums;

namespace ETrocas.Domain.Interfaces;

public interface IPropostaRepository
{
    Task<Proposta> FazerPropostaAsync(Proposta proposta);
    Task<Proposta?> GetByIdAsync(Guid propostaId);
    Task<IReadOnlyCollection<Proposta>> ListarEnviadasAsync(Guid usuarioId);
    Task<IReadOnlyCollection<Proposta>> ListarRecebidasAsync(Guid usuarioId);
    Task<Proposta> AtualizarStatusAsync(Proposta proposta, EStatusProposta status);
}
