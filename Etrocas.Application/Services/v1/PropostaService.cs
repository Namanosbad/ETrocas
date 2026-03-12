using AutoMapper;
using ETrocas.Application.Interfaces;
using ETrocas.Application.Requests;
using ETrocas.Application.Responses;
using ETrocas.Domain.Entities;
using ETrocas.Domain.Enums;
using ETrocas.Domain.Interfaces;

namespace ETrocas.Application.Services.v1
{
    public class PropostaService : IPropostaService
    {
        private readonly IPropostaRepository _propostaRepository;
        private readonly IRepository<Produtos> _produtoRepository;
        private readonly IMapper _mapper;

        public PropostaService(
            IPropostaRepository propostaRepository,
            IRepository<Produtos> produtoRepository,
            IMapper mapper)
        {
            _propostaRepository = propostaRepository;
            _produtoRepository = produtoRepository;
            _mapper = mapper;
        }

        public async Task<PropostaResponse> FazerPropostaAsync(PropostaRequest propostaRequest)
        {
            if (propostaRequest.ProdutoDesejadoId == propostaRequest.ProdutoOfertadoId)
            {
                throw new ArgumentException("O produto ofertado não pode ser o mesmo produto desejado.");
            }

            var produtoDesejado = await _produtoRepository.GetByIdAsync(propostaRequest.ProdutoDesejadoId)
                                 ?? throw new InvalidOperationException("Produto desejado não encontrado.");

            var produtoOfertado = await _produtoRepository.GetByIdAsync(propostaRequest.ProdutoOfertadoId)
                                 ?? throw new InvalidOperationException("Produto ofertado não encontrado.");

            if (produtoDesejado.UsuarioId == produtoOfertado.UsuarioId)
            {
                throw new InvalidOperationException("Não é permitido trocar produtos do mesmo usuário.");
            }

            var proposta = _mapper.Map<Proposta>(propostaRequest);
            proposta.StatusProposta = EStatusProposta.Pendente;

            var propostaCriada = await _propostaRepository.FazerPropostaAsync(proposta);

            return _mapper.Map<PropostaResponse>(propostaCriada);
        }
    }
}
