using ETrocas.Application.Interfaces;
using ETrocas.Application.Requests;
using ETrocas.Application.Responses;
using ETrocas.Domain.Entities;
using ETrocas.Domain.Interfaces;
using ETrocas.Shared.Interfaces;

        //Service contém as regras de negócio, Serve como ponte entre o controller e o repositório
namespace ETrocas.Application.Services.v1
{
    public class UsuarioService : IUsuarioService
    {
        //Injeção de dependencias.
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenService _tokenService;

        //Injeção de dependencias.
        public UsuarioService(IUsuarioRepository usuarioRepository, ITokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
        }

        public async Task<RegistrarUsuarioResponse> RegistrarUsuarioAsync(RegistrarUsuarioRequest request)
        {
            //cria um novo objeto Usuario. Esse objeto representa um novo usuário que será registrado no sistema.
            var CadastroUsuario = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = request.Nome,
                Email = request.Email,
                SenhaHash = request.SenhaHash,
            };

            //aqui grava no banco conforme o repository está feito.
            var usuarioCriado = await _usuarioRepository.RegistrarUsuarioAsync(CadastroUsuario);

            var token = _tokenService.Gerar(usuarioCriado);
            //o que vai retornar para o usuario.
            return new RegistrarUsuarioResponse
            {
                Id = usuarioCriado.Id,
                Nome = usuarioCriado.Nome,
                Email = usuarioCriado.Email,
                Token = token
            };
        }

        public async Task<LoginUsuarioResponse> LoginUsuarioAsync(LoginUsuarioRequest request)
        {
            //criando o novo objeto. Esse objeto representa o usuario que vai logar no sistema
            var usuarioLogin = new Usuario
            {
                Email = request.Email,
                SenhaHash = request.SenhaHash,
            };
            //salva o objeto no banco.
            var usuario = await _usuarioRepository.LoginUsuarioAsync(usuarioLogin);

            var token = _tokenService.Gerar(usuario);
            //o que aparece pro cliente/quem usar 
            return new LoginUsuarioResponse
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Token = token
            };
        }
    }
}