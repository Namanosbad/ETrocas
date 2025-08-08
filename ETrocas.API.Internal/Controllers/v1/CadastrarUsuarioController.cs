using Asp.Versioning;
using ETrocas.Application.Interfaces;
using ETrocas.Application.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ETrocas.API.Internal.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Produces("application/json")]

    public class CadastrarUsuarioController : Controller
    {
        //injeção de dependencia do usuarioService responsável pela lógica de negócios de registro e login de usuários.
        private readonly IUsuarioService _usuarioService;
        //injeção de dependencia do usuarioService.
        public CadastrarUsuarioController(IUsuarioService usuarioService) => _usuarioService = usuarioService;

        //rota (registrar) e metodo http (Post)
        [HttpPost("registrar")]
        //metodo assincrono Cadastrar que retorna IAction result que permite retornar varios tipos de respostas http, como o OK/BadRequest/NotFound.
        public async Task<IActionResult> Cadastrar([FromBody] RegistrarUsuarioRequest request)
        {
            // crio a variavel response, ela acessa a interface IUsuarioService, que contem o contrato de RegistrarUsuarioAsync
            // nele tem a implementação do serviço que pega os dados, salva no banco com o repository e retorna o response e assim que o
            // usuario fizer isso no request passado no Body desse metodo, ele vai salvar com as informações do usuario e retornar o response.
            //conforme definido na classe response.
            //essa linha é equivalente a RegistrarUsuarioResponse response = await _usuarioService.RegistrarUsuarioAsync(request); 
            var response = await _usuarioService.RegistrarUsuarioAsync(request);
            return Ok(response);
        }
        
        //Rota(login) e metodo http (Post)
        [HttpPost("login")]

        //metodo async que usa IActionResult para retornar resposta http tipo OK,BadRequest,NotFound.
        public async Task<IActionResult> Login([FromBody] LoginUsuarioRequest loginRequest)
        {
            //cria a variavel response, que contem a logica do login no service quando recebe,
            //o loginRequest passado pelo usuario e retorna OK e resposta.
            //aqui é o mesmo que usar LoginUsuarioResponse response = await _usuarioService.LoginUsuarioAsync(loginRequest); mas é mais facil usar a variavel
            var response = await _usuarioService.LoginUsuarioAsync(loginRequest);
            return Ok(response);
        }
    }
}
