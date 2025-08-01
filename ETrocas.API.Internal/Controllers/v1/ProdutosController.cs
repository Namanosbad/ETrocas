using Asp.Versioning;
using ETrocas.Application.Interfaces;
using ETrocas.Application.Requests;
using ETrocas.Application.Services.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ETrocas.API.Internal.Controllers.v1
{
    //atributo [ApiController]
    [ApiController]
    //rota
    [Route("api/v{version:apiVersion}/[controller]")]
    //controlador herda de Controller
    [ApiVersion("1.0")]
    [Produces("application/json")]
    public class ProdutosController : Controller
    {
        //injetar o service na controller pois ele que tem a regra de negocio.
        private readonly IProdutoService _produtoService;
        public ProdutosController(IProdutoService produtoService)
        {
           _produtoService = produtoService;
        }

        [Authorize]
        [HttpPost("CadastrarProduto")]
   
        public async Task<IActionResult> CadastrarProduto([FromBody]CadastrarProdutoRequest cadastrarProdutoRequest)
        {
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(usuarioId) || !Guid.TryParse(usuarioId, out var usuarioGuid))
                return Unauthorized();

            var response = await _produtoService.CadastrarProdutoAsync(cadastrarProdutoRequest, usuarioGuid);

            return Ok(response);
        }

        [Authorize]
        [HttpPost("")]

        
    }
}