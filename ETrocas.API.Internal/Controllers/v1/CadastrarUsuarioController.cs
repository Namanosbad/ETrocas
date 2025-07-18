using ETrocas.Application.Interfaces;
using ETrocas.Application.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ETrocas.API.Internal.Controllers.v1
{
    [ApiController]
    [Route("api/v1/Cadastro")]
    [Produces("application/json")]

    public class CadastrarUsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public CadastrarUsuarioController(IUsuarioService usuarioService) => _usuarioService = usuarioService;

        [HttpPost("registrar")]
        public async Task<IActionResult> Cadastrar([FromBody]RegistrarUsuarioRequest request)
        {
            var response = await _usuarioService.RegistrarUsuarioAsync(request);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUsuarioRequest loginRequest)
        {
            var response = await _usuarioService.LoginUsuarioAsync(loginRequest);
            return Ok(response);
        }
    }
}
