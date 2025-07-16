using ETrocas.Database.EntitiesConfiguration;
using ETrocas.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ETrocas.Database
{
    public class ETrocasDbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Produtos> Produtos { get; set; }
        public DbSet<Proposta> Propostas { get; set; }

        public ETrocasDbContext(DbContextOptions<ETrocasDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsuarioEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ProdutosEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PropostaEntityConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}