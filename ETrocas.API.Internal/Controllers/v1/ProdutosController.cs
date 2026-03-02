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
    /// Endpoints para operações de produtos.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    public class ProdutosController : Controller
    {
        private readonly IProdutoService _produtoService;

        public ProdutosController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        /// <summary>
        /// Cadastra um novo produto para o usuário autenticado.
        /// </summary>
        /// <param name="cadastrarProdutoRequest">Dados do produto a ser cadastrado.</param>
        /// <returns>Produto cadastrado.</returns>
        /// <response code="200">Produto cadastrado com sucesso.</response>
        /// <response code="400">Dados inválidos para cadastro.</response>
        /// <response code="401">Usuário não autenticado.</response>
        [Authorize]
        [HttpPost("CadastrarProduto")]
        [ProducesResponseType(typeof(CadastrarProdutoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CadastrarProduto([FromBody] CadastrarProdutoRequest cadastrarProdutoRequest)
        {
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(usuarioId) || !Guid.TryParse(usuarioId, out var usuarioGuid))
                return Unauthorized();

            var response = await _produtoService.CadastrarProdutoAsync(cadastrarProdutoRequest, usuarioGuid);
            return Ok(response);
        }

        /// <summary>
        /// Lista todos os produtos cadastrados.
        /// </summary>
        /// <returns>Lista de produtos.</returns>
        /// <response code="200">Produtos retornados com sucesso.</response>
        [HttpGet("BuscarTodosProdutos")]
        [ProducesResponseType(typeof(IEnumerable<CadastrarProdutoResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> TodosProdutosAsync()
        {
            var produtos = await _produtoService.GetAllAsync();
            return Ok(produtos);
        }

        /// <summary>
        /// Busca um produto específico pelo identificador.
        /// </summary>
        /// <param name="id">Identificador do produto.</param>
        /// <returns>Produto localizado.</returns>
        /// <response code="200">Produto encontrado.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="404">Produto não encontrado.</response>
        [Authorize]
        [HttpGet("BuscarProduto/{id:guid}")]
        [ProducesResponseType(typeof(CadastrarProdutoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PuxarIdAsync(Guid id)
        {
            var produto = await _produtoService.GetByIdAsync(id);
            return Ok(produto);
        }

        /// <summary>
        /// Atualiza os dados de um produto existente.
        /// </summary>
        /// <param name="id">Identificador do produto.</param>
        /// <param name="updateRequest">Novos dados do produto.</param>
        /// <returns>Produto atualizado.</returns>
        /// <response code="200">Produto atualizado com sucesso.</response>
        /// <response code="400">Dados inválidos para atualização.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="404">Produto não encontrado.</response>
        [Authorize]
        [HttpPut("AtualizarProduto/{id:guid}")]
        [ProducesResponseType(typeof(AtualizarProdutoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AtualizarProdutoAsync(Guid id, [FromBody] AtualizarProdutoRequest updateRequest)
        {
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(usuarioId) || !Guid.TryParse(usuarioId, out var usuarioGuid))
                return Unauthorized();

            var produto = await _produtoService.UpdateAsync(id, updateRequest, usuarioGuid);
            return Ok(produto);
        }

        /// <summary>
        /// Remove um produto pelo identificador.
        /// </summary>
        /// <param name="id">Identificador do produto.</param>
        /// <response code="204">Produto removido com sucesso.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="404">Produto não encontrado.</response>
        [Authorize]
        [HttpDelete("DeletarProduto/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeletarProdutoAsync(Guid id)
        {
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(usuarioId) || !Guid.TryParse(usuarioId, out var usuarioGuid))
                return Unauthorized();

            await _produtoService.DeletarProdutoAsync(id, usuarioGuid);
            return NoContent();
        }
    }
}
