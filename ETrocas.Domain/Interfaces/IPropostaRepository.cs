using ETrocas.Domain.Entities;

namespace ETrocas.Domain.Interfaces;

public interface IPropostaRepository
{
    Task<Proposta> FazerPropostaAsync (Proposta proposta);
}