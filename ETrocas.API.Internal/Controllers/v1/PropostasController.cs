using Asp.Versioning;
using ETrocas.Application.Interfaces;
using ETrocas.Application.Requests;
using ETrocas.Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ETrocas.API.Internal.Controllers.v1
{
    /// <summary>
    /// Endpoints de propostas de troca.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Authorize]
    public class PropostasController : ControllerBase
    {
        private readonly IPropostaService _propostaService;

        public PropostasController(IPropostaService propostaService)
        {
            _propostaService = propostaService;
        }

        /// <summary>
        /// Cria uma nova proposta de troca para um produto desejado.
        /// </summary>
        /// <param name="id">Identificador do produto desejado informado na rota.</param>
        /// <param name="propostaRequest">Dados da proposta (produto ofertado, valor e mensagem).</param>
        /// <returns>Proposta criada com status inicial pendente.</returns>
        /// <response code="200">Proposta criada com sucesso.</response>
        /// <response code="400">Dados inválidos para criação da proposta.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="404">Produto desejado ou ofertado não encontrado.</response>
        [HttpPost("{id:guid}")]
        [ProducesResponseType(typeof(PropostaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> FazerProposta(Guid id, [FromBody] PropostaRequest propostaRequest)
        {
            if (id != propostaRequest.ProdutoDesejadoId)
            {
                return BadRequest("O id da rota deve ser o mesmo ProdutoDesejadoId informado no corpo da requisição.");
            }

            if (!TryGetUsuarioId(out var usuarioId))
            {
                return Unauthorized();
            }

            var proposta = await _propostaService.FazerPropostaAsync(propostaRequest, usuarioId);
            return Ok(proposta);
        }

        /// <summary>
        /// Lista as propostas enviadas pelo usuário autenticado.
        /// </summary>
        /// <returns>Coleção de propostas enviadas.</returns>
        /// <response code="200">Propostas enviadas retornadas com sucesso.</response>
        /// <response code="401">Usuário não autenticado.</response>
        [HttpGet("enviadas")]
        [ProducesResponseType(typeof(IReadOnlyCollection<PropostaResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListarEnviadas()
        {
            if (!TryGetUsuarioId(out var usuarioId))
            {
                return Unauthorized();
            }

            var propostas = await _propostaService.ListarEnviadasAsync(usuarioId);
            return Ok(propostas);
        }

        /// <summary>
        /// Lista as propostas recebidas pelo usuário autenticado.
        /// </summary>
        /// <returns>Coleção de propostas recebidas.</returns>
        /// <response code="200">Propostas recebidas retornadas com sucesso.</response>
        /// <response code="401">Usuário não autenticado.</response>
        [HttpGet("recebidas")]
        [ProducesResponseType(typeof(IReadOnlyCollection<PropostaResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListarRecebidas()
        {
            if (!TryGetUsuarioId(out var usuarioId))
            {
                return Unauthorized();
            }

            var propostas = await _propostaService.ListarRecebidasAsync(usuarioId);
            return Ok(propostas);
        }

        /// <summary>
        /// Aceita uma proposta pendente recebida pelo usuário autenticado.
        /// </summary>
        /// <param name="id">Identificador da proposta.</param>
        /// <returns>Proposta atualizada com status aceito.</returns>
        /// <response code="200">Proposta aceita com sucesso.</response>
        /// <response code="401">Usuário não autenticado.</response>
        [HttpPut("{id:guid}/aceitar")]
        [ProducesResponseType(typeof(PropostaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Aceitar(Guid id)
        {
            if (!TryGetUsuarioId(out var usuarioId))
            {
                return Unauthorized();
            }

            return Ok(await _propostaService.AceitarAsync(id, usuarioId));
        }

        /// <summary>
        /// Recusa uma proposta pendente recebida pelo usuário autenticado.
        /// </summary>
        /// <param name="id">Identificador da proposta.</param>
        /// <returns>Proposta atualizada com status recusado.</returns>
        /// <response code="200">Proposta recusada com sucesso.</response>
        /// <response code="401">Usuário não autenticado.</response>
        [HttpPut("{id:guid}/recusar")]
        [ProducesResponseType(typeof(PropostaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Recusar(Guid id)
        {
            if (!TryGetUsuarioId(out var usuarioId))
            {
                return Unauthorized();
            }

            return Ok(await _propostaService.RecusarAsync(id, usuarioId));
        }

        /// <summary>
        /// Cancela uma proposta pendente enviada pelo usuário autenticado.
        /// </summary>
        /// <param name="id">Identificador da proposta.</param>
        /// <returns>Proposta atualizada com status cancelado.</returns>
        /// <response code="200">Proposta cancelada com sucesso.</response>
        /// <response code="401">Usuário não autenticado.</response>
        [HttpPut("{id:guid}/cancelar")]
        [ProducesResponseType(typeof(PropostaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Cancelar(Guid id)
        {
            if (!TryGetUsuarioId(out var usuarioId))
            {
                return Unauthorized();
            }

            return Ok(await _propostaService.CancelarAsync(id, usuarioId));
        }

        /// <summary>
        /// Conclui uma proposta aceita entre os participantes da troca.
        /// </summary>
        /// <param name="id">Identificador da proposta.</param>
        /// <returns>Proposta atualizada com status concluído.</returns>
        /// <response code="200">Proposta concluída com sucesso.</response>
        /// <response code="401">Usuário não autenticado.</response>
        [HttpPut("{id:guid}/concluir")]
        [ProducesResponseType(typeof(PropostaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Concluir(Guid id)
        {
            if (!TryGetUsuarioId(out var usuarioId))
            {
                return Unauthorized();
            }

            return Ok(await _propostaService.ConcluirAsync(id, usuarioId));
        }

        private bool TryGetUsuarioId(out Guid usuarioId)
        {
            usuarioId = Guid.Empty;
            var usuarioClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return !string.IsNullOrWhiteSpace(usuarioClaim) && Guid.TryParse(usuarioClaim, out usuarioId);
        }
    }
}
