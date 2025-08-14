using Asp.Versioning;
using ETrocas.Application.Interfaces;
using ETrocas.Application.Requests;
using ETrocas.Application.Responses;
using ETrocas.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            var proposta = await _propostaService.FazerPropostaAsync(propostaRequest); 
            return Ok(proposta);
        }
    }
}