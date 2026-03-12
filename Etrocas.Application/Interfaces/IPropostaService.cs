using ETrocas.Application.Requests;
using ETrocas.Application.Responses;

namespace ETrocas.Application.Interfaces;

public interface IPropostaService
{
    Task<PropostaResponse> FazerPropostaAsync(PropostaRequest request, Guid usuarioId);
    Task<IReadOnlyCollection<PropostaResponse>> ListarEnviadasAsync(Guid usuarioId);
    Task<IReadOnlyCollection<PropostaResponse>> ListarRecebidasAsync(Guid usuarioId);
    Task<PropostaResponse> AceitarAsync(Guid propostaId, Guid usuarioId);
    Task<PropostaResponse> RecusarAsync(Guid propostaId, Guid usuarioId);
    Task<PropostaResponse> CancelarAsync(Guid propostaId, Guid usuarioId);
    Task<PropostaResponse> ConcluirAsync(Guid propostaId, Guid usuarioId);
}
