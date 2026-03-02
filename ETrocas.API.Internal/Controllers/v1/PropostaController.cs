using Asp.Versioning;
using ETrocas.Application.Interfaces;
using ETrocas.Application.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETrocas.API.Internal.Controllers.v1
{
    [ApiController]
    //rota
    [Route("api/v{version:apiVersion}/[controller]")]
    //controlador herda de Controller
    [ApiVersion("1.0")]
    [Produces("application/json")]

    public class PropostaController : Controller
    {
        private readonly IPropostaService _propostaService;

        public PropostaController(IPropostaService propostaService)
        {
            _propostaService = propostaService;
        }

        [Authorize]
        [HttpPost("FazerProposta/{id:guid}")]
        public async Task<IActionResult> FazerProposta(Guid id, [FromBody] PropostaRequest propostaRequest)
        {
            if (id != propostaRequest.ProdutoDesejadoId)
            {
                return BadRequest("O id da rota deve ser o mesmo ProdutoDesejadoId informado no corpo da requisição.");
            }

            try
            {
                var proposta = await _propostaService.FazerPropostaAsync(propostaRequest);
                return Ok(proposta);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
