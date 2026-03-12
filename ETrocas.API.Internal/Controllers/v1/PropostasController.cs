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
