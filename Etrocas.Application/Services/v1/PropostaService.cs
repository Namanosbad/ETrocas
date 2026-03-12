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

        public async Task<PropostaResponse> FazerPropostaAsync(PropostaRequest propostaRequest, Guid usuarioId)
        {
            if (propostaRequest.ProdutoDesejadoId == propostaRequest.ProdutoOfertadoId)
            {
                throw new ArgumentException("O produto ofertado não pode ser o mesmo produto desejado.");
            }

            var produtoDesejado = await _produtoRepository.GetByIdAsync(propostaRequest.ProdutoDesejadoId)
                                 ?? throw new InvalidOperationException("Produto desejado não encontrado.");

            var produtoOfertado = await _produtoRepository.GetByIdAsync(propostaRequest.ProdutoOfertadoId)
                                 ?? throw new InvalidOperationException("Produto ofertado não encontrado.");

            if (produtoOfertado.UsuarioId != usuarioId)
            {
                throw new UnauthorizedAccessException("Você só pode ofertar um produto do seu próprio usuário.");
            }

            if (produtoDesejado.UsuarioId == produtoOfertado.UsuarioId)
            {
                throw new InvalidOperationException("Não é permitido trocar produtos do mesmo usuário.");
            }

            var proposta = _mapper.Map<Proposta>(propostaRequest);
            proposta.StatusProposta = EStatusProposta.Pendente;
            proposta.UsuarioPropostaId = usuarioId;
            proposta.UsuarioRecebedorId = produtoDesejado.UsuarioId;
            proposta.DataResposta = null;

            var propostaCriada = await _propostaRepository.FazerPropostaAsync(proposta);
            return _mapper.Map<PropostaResponse>(propostaCriada);
        }

        public async Task<IReadOnlyCollection<PropostaResponse>> ListarEnviadasAsync(Guid usuarioId)
        {
            var propostas = await _propostaRepository.ListarEnviadasAsync(usuarioId);
            return _mapper.Map<IReadOnlyCollection<PropostaResponse>>(propostas);
        }

        public async Task<IReadOnlyCollection<PropostaResponse>> ListarRecebidasAsync(Guid usuarioId)
        {
            var propostas = await _propostaRepository.ListarRecebidasAsync(usuarioId);
            return _mapper.Map<IReadOnlyCollection<PropostaResponse>>(propostas);
        }

        public async Task<PropostaResponse> AtualizarStatusAsync(Guid propostaId, Guid usuarioId, EStatusProposta status)
        {
            var proposta = await _propostaRepository.GetByIdAsync(propostaId)
                           ?? throw new KeyNotFoundException("Proposta não encontrada.");

            switch (status)
            {
                case EStatusProposta.Aceita:
                case EStatusProposta.Recusada:
                    ValidarRespostaRecebedor(proposta, usuarioId);
                    break;
                case EStatusProposta.Cancelada:
                    ValidarCancelamento(proposta, usuarioId);
                    break;
                case EStatusProposta.Concluida:
                    ValidarConclusao(proposta, usuarioId);
                    break;
                default:
                    throw new InvalidOperationException("Transição de status inválida para o fluxo da proposta.");
            }

            var atualizada = await _propostaRepository.AtualizarStatusAsync(proposta, status);
            return _mapper.Map<PropostaResponse>(atualizada);
        }

        private static void ValidarRespostaRecebedor(Proposta proposta, Guid usuarioId)
        {
            if (proposta.UsuarioRecebedorId != usuarioId)
            {
                throw new UnauthorizedAccessException("Somente o usuário que recebeu a proposta pode atualizar esse status.");
            }

            if (proposta.StatusProposta != EStatusProposta.Pendente)
            {
                throw new InvalidOperationException("Apenas propostas pendentes podem ser respondidas.");
            }
        }

        private static void ValidarCancelamento(Proposta proposta, Guid usuarioId)
        {
            if (proposta.UsuarioPropostaId != usuarioId)
            {
                throw new UnauthorizedAccessException("Somente o usuário que fez a proposta pode cancelá-la.");
            }

            if (proposta.StatusProposta != EStatusProposta.Pendente)
            {
                throw new InvalidOperationException("Apenas propostas pendentes podem ser canceladas.");
            }
        }

        private static void ValidarConclusao(Proposta proposta, Guid usuarioId)
        {
            if (proposta.UsuarioPropostaId != usuarioId && proposta.UsuarioRecebedorId != usuarioId)
            {
                throw new UnauthorizedAccessException("Somente participantes da proposta podem concluí-la.");
            }

            if (proposta.StatusProposta != EStatusProposta.Aceita)
            {
                throw new InvalidOperationException("Somente propostas aceitas podem ser concluídas.");
            }
        }
    }
}
