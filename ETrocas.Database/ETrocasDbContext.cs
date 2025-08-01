using ETrocas.Database.EntitiesConfiguration;
using ETrocas.Domain.Entities;
using Microsoft.EntityFrameworkCore;

//Banco de dados 
namespace ETrocas.Database
{
    //Essa classe representa o banco de dados e gerencia a conexão com ele.
    public class ETrocasDbContext : DbContext
    {
        //cada DbSet <T> representa uma tabela do banco de dados. 
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Produtos> Produtos { get; set; }
        public DbSet<Proposta> Propostas { get; set; }

        //construtor que traz as configurações da conexão do banco, que estão no appsettings.
        public ETrocasDbContext(DbContextOptions<ETrocasDbContext> options) : base(options) { }

        //metodo para configurar o modelo do banco usando classes separadas dessa.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsuarioEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ProdutosEntityConfiguration());
            modelBuilder.ApplyConfiguration(new PropostaEntityConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}