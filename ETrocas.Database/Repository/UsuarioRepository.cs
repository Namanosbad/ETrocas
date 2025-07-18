using ETrocas.Domain.Entities;
using ETrocas.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ETrocas.Database.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ETrocasDbContext _context;

        public UsuarioRepository(ETrocasDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario> RegistrarUsuarioAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario?> LoginUsuarioAsync(Usuario usuario)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u =>
                    u.Email == usuario.Email &&
                    u.SenhaHash == usuario.SenhaHash);
        }
    }
}