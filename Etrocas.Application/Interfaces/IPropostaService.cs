using ETrocas.Application.Requests;
using ETrocas.Application.Responses;
using ETrocas.Domain.Enums;

namespace ETrocas.Application.Interfaces;

public interface IPropostaService
{
    Task<PropostaResponse> FazerPropostaAsync(PropostaRequest request, Guid usuarioId);
    Task<IReadOnlyCollection<PropostaResponse>> ListarEnviadasAsync(Guid usuarioId);
    Task<IReadOnlyCollection<PropostaResponse>> ListarRecebidasAsync(Guid usuarioId);
    Task<PropostaResponse> AtualizarStatusAsync(Guid propostaId, Guid usuarioId, EStatusProposta status);
}
