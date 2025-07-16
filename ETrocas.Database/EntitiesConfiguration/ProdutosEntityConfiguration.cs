using ETrocas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ETrocas.Database.EntitiesConfiguration
{
    public class ProdutosEntityConfiguration : IEntityTypeConfiguration<Produtos>
    {
        public void Configure(EntityTypeBuilder<Produtos> builder)
        {
            builder.ToTable("produtos");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                        .IsRequired()
                        .ValueGeneratedOnAdd();

            builder.Property(p => p.Produto)
                        .IsRequired()
                        .HasMaxLength(100);

            builder.Property(p => p.Tipo)
                        .HasMaxLength(50);

            builder.Property(p => p.Valor)
                        .HasColumnType("decimal(10,2)");

            builder.Property(p => p.Descricao)
                        .IsRequired();

            builder.Property(p => p.Disponivel)
                        .IsRequired();

            builder.Property(p => p.DataCadastro)
                        .IsRequired();

            builder.Property(p=> p.ImageUrl)
                        .IsRequired ();

            builder.HasOne(p => p.Usuario)
                        .WithMany(u => u.Produtos)
                        .HasForeignKey(p => p.UsuarioId)
                        .OnDelete(DeleteBehavior.Restrict);
        }
    }
}