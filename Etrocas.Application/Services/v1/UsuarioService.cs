using ETrocas.Application.Interfaces;
using ETrocas.Application.Requests;
using ETrocas.Application.Responses;
using ETrocas.Domain.Entities;
using ETrocas.Domain.Interfaces;
using ETrocas.Shared.Interfaces;

namespace ETrocas.Application.Services.v1
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenService _tokenService;

        public UsuarioService(IUsuarioRepository usuarioRepository, ITokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
        }

        public async Task<RegistrarUsuarioResponse> RegistrarUsuarioAsync(RegistrarUsuarioRequest request)
        {
            var CadastroUsuario = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = request.Nome,
                Email = request.Email,
                SenhaHash = request.SenhaHash,
            };

            var usuarioCriado = await _usuarioRepository.RegistrarUsuarioAsync(CadastroUsuario);

            var token = _tokenService.Gerar(usuarioCriado);

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
            var usuarioLogin = new Usuario
            {
                Email = request.Email,
                SenhaHash = request.SenhaHash,
            };

            var usuario = await _usuarioRepository.LoginUsuarioAsync(usuarioLogin);

            var token = _tokenService.Gerar(usuario);

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