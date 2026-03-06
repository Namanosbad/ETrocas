using Asp.Versioning;
using ETrocas.Application.Interfaces;
using ETrocas.Application.Requests;
using ETrocas.Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETrocas.API.Internal.Controllers.v1
{
    /// <summary>
    /// Endpoints para criação de propostas de troca.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    public class PropostaController : Controller
    {
        private readonly IPropostaService _propostaService;

        public PropostaController(IPropostaService propostaService)
        {
            _propostaService = propostaService;
        }

        /// <summary>
        /// Cria uma proposta de troca para um produto desejado.
        /// </summary>
        /// <param name="id">Identificador do produto desejado (deve ser igual ao ProdutoDesejadoId do corpo).</param>
        /// <param name="propostaRequest">Dados da proposta de troca.</param>
        /// <returns>Proposta criada.</returns>
        /// <response code="200">Proposta criada com sucesso.</response>
        /// <response code="400">Dados da proposta inválidos.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="404">Produto não encontrado para proposta.</response>
        [Authorize]
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

            var proposta = await _propostaService.FazerPropostaAsync(propostaRequest);
            return Ok(proposta);
        }
    }
}
