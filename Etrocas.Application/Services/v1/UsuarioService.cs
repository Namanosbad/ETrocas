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
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRepository<Usuario> _repository;

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

            var cadastroUsuario = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = request.Nome,
                Email = request.Email,
                SenhaHash = _passwordHasher.Hash(request.SenhaHash),
            };

            var usuarioCriado = await _usuarioRepository.RegistrarUsuarioAsync(cadastroUsuario);

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
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.SenhaHash))
                throw new ArgumentException("Email e senha são obrigatórios.");

            var usuario = await _usuarioRepository.ObterPorEmailAsync(request.Email);

            if (usuario == null || !_passwordHasher.Verify(request.SenhaHash, usuario.SenhaHash ?? string.Empty))
                throw new UnauthorizedAccessException("Credenciais inválidas.");

            var token = _tokenService.Gerar(usuario);
            return new LoginUsuarioResponse
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Token = token
            };
        }

        public async Task<UsuariosPaginadosResponse> ListarUsuariosAsync(int pagina, int tamanhoPagina)
        {
            if (pagina <= 0)
                throw new ArgumentException("Página deve ser maior que zero.");

            if (tamanhoPagina <= 0 || tamanhoPagina > 100)
                throw new ArgumentException("Tamanho da página deve estar entre 1 e 100.");

            var usuarios = (await _repository.GetAllAsync()).ToList();

            var itens = usuarios
                .OrderBy(usuario => usuario.Nome)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .Select(usuario => new UsuarioPublicoResponse
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                })
                .ToList();

            return new UsuariosPaginadosResponse
            {
                Pagina = pagina,
                TamanhoPagina = tamanhoPagina,
                TotalItens = usuarios.Count,
                Itens = itens,
            };
        }

        public async Task<UsuarioPublicoResponse> ObterUsuarioPorIdAsync(Guid usuarioId)
        {
            var usuario = await _repository.GetByIdAsync(usuarioId);

            if (usuario == null)
                throw new InvalidOperationException("Usuário não encontrado.");

            return new UsuarioPublicoResponse
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
            };
        }

        public async Task<UsuarioResponse> ObterPerfilUsuarioAsync(Guid usuarioId)
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
