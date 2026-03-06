using Asp.Versioning;
using ETrocas.Application.Interfaces;
using ETrocas.Application.Requests;
using ETrocas.Application.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETrocas.API.Internal.Controllers.v1
{
    /// <summary>
    /// Endpoints para autenticação de usuários.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public AuthController(IUsuarioService usuarioService) => _usuarioService = usuarioService;
        /// <summary>
        /// Realiza autenticação de usuário e retorna token JWT.
        /// </summary>
        /// <param name="loginRequest">Credenciais de acesso do usuário.</param>
        /// <returns>Token e informações básicas do usuário autenticado.</returns>
        /// <response code="200">Login efetuado com sucesso.</response>
        /// <response code="400">Dados de login inválidos.</response>
        /// <response code="401">Credenciais incorretas.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginUsuarioResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginUsuarioRequest loginRequest)
        {
            var response = await _usuarioService.LoginUsuarioAsync(loginRequest);
            return Ok(response);
        }
    }
}
