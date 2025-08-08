using ETrocas.Domain.Entities;
using ETrocas.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

        //Repository separa o acesso a dados da lógica de negócio.
namespace ETrocas.Database.Repository
{
        //Repositorio herda da interface I...Repository.
    public class UsuarioRepository : IUsuarioRepository
    {
        //armazenar a instancia do banco que o ctor vai receber.
        private readonly ETrocasDbContext _context;
        //injeção de dependencia do banco de dados.
        public UsuarioRepository(ETrocasDbContext context)
        {
            _context = context;
        }
        //metodo 
        public async Task<Usuario> RegistrarUsuarioAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario> LoginUsuarioAsync(Usuario usuario)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u =>
                    u.Email == usuario.Email &&
                    u.SenhaHash == usuario.SenhaHash);
        }
    }
}