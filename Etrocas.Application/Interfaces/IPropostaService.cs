using ETrocas.Application.Requests;
using ETrocas.Application.Responses;

namespace ETrocas.Application.Interfaces;

public interface IPropostaService
{
    Task<PropostaResponse> FazerPropostaAsync(PropostaRequest request);
}