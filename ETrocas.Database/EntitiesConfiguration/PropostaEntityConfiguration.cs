using ETrocas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ETrocas.Database.EntitiesConfiguration
{
    public class PropostaEntityConfiguration : IEntityTypeConfiguration<Proposta>
    {
        public void Configure(EntityTypeBuilder<Proposta> builder)
        {
            builder.ToTable("proposta");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                         .IsRequired()
                         .ValueGeneratedOnAdd();

            builder.Property(p => p.DataProposta)
                        .IsRequired();

            builder.Property(p => p.DataResposta)
                        .IsRequired();

            builder.Property(p => p.StatusProposta)
                        .IsRequired();

            builder.HasOne(p => p.ProdutoDesejado)
                         .WithMany()
                         .HasForeignKey(p => p.ProdutoDesejadoId)
                         .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.ProdutoOfertado)
                         .WithMany()
                         .HasForeignKey(p => p.ProdutoOfertadoId)
                         .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.UsuarioProposta)
                         .WithMany(u => u.PropostasFeitas)
                         .HasForeignKey(p => p.UsuarioPropostaId)
                         .OnDelete(DeleteBehavior.Restrict);
        }
    }
}