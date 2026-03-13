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
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRepository<Usuario> _repository;

        //Injeção de dependencias.
        public UsuarioService(
            IUsuarioRepository usuarioRepository,
            ITokenService tokenService,
            IPasswordHasher passwordHasher,
            IRepository<Usuario> repository)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _repository = repository;
        }

        public async Task<RegistrarUsuarioResponse> RegistrarUsuarioAsync(RegistrarUsuarioRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(request.Nome) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.SenhaHash))
                throw new ArgumentException("Nome, email e senha são obrigatórios.");

            var usuarioExistente = await _usuarioRepository.ValidarEmailAsync(request.Email);
            if (usuarioExistente != null)
                throw new ArgumentException("Email já cadastrado.");

            //cria um novo objeto Usuario. Esse objeto representa um novo usuário que será registrado no sistema.
            var CadastroUsuario = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = request.Nome,
                Email = request.Email,
                SenhaHash = _passwordHasher.Hash(request.SenhaHash),
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
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.SenhaHash))
                throw new ArgumentException("Email e senha são obrigatórios.");

            //criando o novo objeto. Esse objeto representa o usuario que vai logar no sistema
            var usuario = await _usuarioRepository.ObterPorEmailAsync(request.Email);

            if (usuario == null || !_passwordHasher.Verify(request.SenhaHash, usuario.SenhaHash ?? string.Empty))
                throw new UnauthorizedAccessException("Credenciais inválidas.");

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

        public async Task<IEnumerable<UsuarioResponse>> ListarUsuariosAsync()
        {
            var usuarios = await _repository.GetAllAsync();

            return usuarios.Select(usuario => new UsuarioResponse
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
            });
        }

        public async Task<UsuarioResponse> ObterUsuarioPorIdAsync(Guid usuarioId)
        {
            var usuario = await _repository.GetByIdAsync(usuarioId);

            if (usuario == null)
                throw new InvalidOperationException("Usuário não encontrado.");

            return new UsuarioResponse
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
            };
        }

        public async Task<UsuarioResponse> AtualizarUsuarioAsync(Guid usuarioId, AtualizarUsuarioRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(request.Nome) || string.IsNullOrWhiteSpace(request.Email))
                throw new ArgumentException("Nome e email são obrigatórios.");

            var usuario = await _repository.GetByIdAsync(usuarioId);
            if (usuario == null)
                throw new InvalidOperationException("Usuário não encontrado.");

            var usuarioExistenteComMesmoEmail = await _usuarioRepository.ValidarEmailAsync(request.Email);
            if (usuarioExistenteComMesmoEmail != null && usuarioExistenteComMesmoEmail.Id != usuarioId)
                throw new ArgumentException("Email já cadastrado.");

            usuario.Nome = request.Nome;
            usuario.Email = request.Email;

            await _repository.UpdateAsync(usuario);

            return new UsuarioResponse
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
            };
        }
    }
}
