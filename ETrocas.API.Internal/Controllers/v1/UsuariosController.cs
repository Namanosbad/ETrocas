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
    /// Endpoints para gerenciamento de usuários.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    public class UsuariosController : ControllerBase
    {
        private const string LeituraUsuariosPolicy = "CanReadUsers";
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService) => _usuarioService = usuarioService;

        /// <summary>
        /// Registra um novo usuário na plataforma.
        /// </summary>
        /// <param name="request">Dados de cadastro do usuário.</param>
        /// <returns>Informações básicas do usuário criado e token de autenticação.</returns>
        /// <response code="200">Usuário cadastrado com sucesso.</response>
        /// <response code="400">Dados inválidos para cadastro.</response>
        [HttpPost]
        [ProducesResponseType(typeof(RegistrarUsuarioResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Cadastrar([FromBody] RegistrarUsuarioRequest request)
        {
            var response = await _usuarioService.RegistrarUsuarioAsync(request);
            return Ok(response);
        }

        /// <summary>
        /// Lista usuários com paginação e projeção pública mínima.
        /// </summary>
        /// <param name="pagina">Número da página (inicia em 1).</param>
        /// <param name="tamanhoPagina">Quantidade de registros por página.</param>
        /// <returns>Conjunto paginado de usuários públicos.</returns>
        /// <response code="200">Usuários retornados com sucesso.</response>
        /// <response code="401">Usuário não autenticado.</response>
        [Authorize(Policy = LeituraUsuariosPolicy)]
        [HttpGet]
        [ProducesResponseType(typeof(UsuariosPaginadosResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListarUsuarios([FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 20)
        {
            var response = await _usuarioService.ListarUsuariosAsync(pagina, tamanhoPagina);
            return Ok(response);
        }

        /// <summary>
        /// Obtém os dados públicos de um usuário pelo identificador.
        /// </summary>
        /// <param name="id">Identificador do usuário.</param>
        /// <returns>Dados públicos do usuário.</returns>
        /// <response code="200">Usuário encontrado.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="404">Usuário não encontrado.</response>
        [Authorize(Policy = LeituraUsuariosPolicy)]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(UsuarioPublicoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ObterUsuarioPorId(Guid id)
        {
            var response = await _usuarioService.ObterUsuarioPorIdAsync(id);
            return Ok(response);
        }

        /// <summary>
        /// Retorna os dados completos do usuário autenticado.
        /// </summary>
        /// <returns>Perfil do usuário autenticado.</returns>
        /// <response code="200">Perfil retornado com sucesso.</response>
        /// <response code="401">Usuário não autenticado.</response>
        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(typeof(UsuarioResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ObterMeuPerfil()
        {
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(usuarioId) || !Guid.TryParse(usuarioId, out var usuarioGuid))
                return Unauthorized();

            var response = await _usuarioService.ObterPerfilUsuarioAsync(usuarioGuid);
            return Ok(response);
        }

        /// <summary>
        /// Atualiza os dados de perfil do usuário autenticado.
        /// </summary>
        /// <param name="request">Dados atualizados de perfil.</param>
        /// <returns>Perfil atualizado do usuário autenticado.</returns>
        /// <response code="200">Perfil atualizado com sucesso.</response>
        /// <response code="400">Dados inválidos para atualização.</response>
        /// <response code="401">Usuário não autenticado.</response>
        /// <response code="404">Usuário não encontrado.</response>
        [Authorize]
        [HttpPut("me")]
        [ProducesResponseType(typeof(UsuarioResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AtualizarMeuPerfil([FromBody] AtualizarUsuarioRequest request)
        {
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(usuarioId) || !Guid.TryParse(usuarioId, out var usuarioGuid))
                return Unauthorized();

            var response = await _usuarioService.AtualizarUsuarioAsync(usuarioGuid, request);
            return Ok(response);
        }
    }
}
