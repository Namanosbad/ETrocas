using ETrocas.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ETrocas.Database.EntitiesConfiguration
{
    public class UsuarioEntityConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("usuario");
            builder.HasKey(x => x.Id);

            builder.Property(i => i.Id)
                                    .IsRequired()
                                    .ValueGeneratedOnAdd();

            builder.Property(i => i.Nome)
                                    .IsRequired()
                                    .HasMaxLength(100);

            builder.Property(i => i.Email)
                                    .IsRequired()
                                    .HasMaxLength(100);

            builder.Property(i => i.SenhaHash)
                                    .IsRequired()
                                    .HasMaxLength(100);

            builder.HasMany(u => u.Produtos)
                                    .WithOne(p=> p.Usuario)
                                    .HasForeignKey(p=> p.UsuarioId)
                                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.PropostasFeitas)
                                    .WithOne(p=> p.UsuarioProposta)
                                    .HasForeignKey(p => p.UsuarioPropostaId)
                                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}