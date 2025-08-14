using ETrocas.Application.Requests;
using ETrocas.Application.Responses;
using ETrocas.Domain.Entities;
using ETrocas.Domain.Interfaces;
using System.Reflection.Metadata.Ecma335;

namespace ETrocas.Application.Services.v1
{
    public class PropostaService
    {
        //injeção de depedencia do repo
        private readonly IPropostaRepository _propostaRepository;

        public PropostaService(IPropostaRepository propostaRepository)
        {
            _propostaRepository = propostaRepository;
        }

        public async Task<PropostaResponse> FazerPropostaAsync( PropostaRequest propostaRequest)
        {
            var proposta = new Proposta();
            proposta.ProdutoDesejadoId = propostaRequest.ProdutoDesejadoId;
            proposta.ProdutoOfertadoId = propostaRequest.ProdutoOfertadoid;
            proposta.ValorProposto = propostaRequest.ValorProposto;
            proposta.Mensagem = propostaRequest.Mensagem;
            //adiciona e salva
            var propostaCriada = await _propostaRepository.FazerPropostaAsync(proposta);

            return new PropostaResponse
            {
                Id = proposta.Id,
                Status = proposta.StatusProposta,
                DataProposta = proposta.DataProposta,
                Mensagem = proposta.Mensagem,
            };
        }
    }
}