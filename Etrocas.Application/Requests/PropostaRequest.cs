using ETrocas.Domain.Entities;

namespace ETrocas.Application.Requests;

public class PropostaRequest
{
    public Guid ProdutoDesejadoId { get; set; }
    public Guid ProdutoOfertadoid { get; set; }
    public decimal ValorProposto { get; set; }
    public string? Mensagem { get; set; }
}