namespace ETrocas.Application.Requests;

/// <summary>
/// Dados para criação de proposta de troca.
/// </summary>
public class PropostaRequest
{
    public Guid ProdutoDesejadoId { get; set; }
    public Guid ProdutoOfertadoid { get; set; }
    public decimal ValorProposto { get; set; }
    public string? Mensagem { get; set; }
}
