using ETrocas.Domain.Enums;

namespace ETrocas.Application.Responses;

/// <summary>
/// Retorno de proposta.
/// </summary>
public class PropostaResponse
{
    public Guid Id { get; set; }
    public EStatusProposta Status { get; set; }
    public DateTime DataProposta { get; set; }
    public DateTime? DataResposta { get; set; }
    public Guid UsuarioPropostaId { get; set; }
    public Guid UsuarioRecebedorId { get; set; }
    public Guid ProdutoDesejadoId { get; set; }
    public Guid ProdutoOfertadoId { get; set; }
    public decimal ValorProposto { get; set; }
    public string? Mensagem { get; set; }
}
