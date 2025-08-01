using ETrocas.Domain.Enums;
using ETrocas.Domain.Interfaces;

namespace ETrocas.Domain.Entities;
  
public class Proposta : IEntity
{
    public Guid Id { get; set; }
    public DateTime DataProposta { get; set; }
    public DateTime DataResposta {  get; set; }
    public EStatusProposta StatusProposta { get; set; } = EStatusProposta.Pendente;

    public Guid UsuarioPropostaId { get; set; }
    public Usuario? UsuarioProposta { get; set; }

    public Guid ProdutoDesejadoId { get; set; }
    public Produtos? ProdutoDesejado { get; set; }
    public Guid ProdutoOfertadoId { get; set; }
    public Produtos? ProdutoOfertado { get; set; }
}