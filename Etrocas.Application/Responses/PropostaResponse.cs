using ETrocas.Domain.Enums;

namespace ETrocas.Application.Responses;

/// <summary>
/// Retorno de proposta criada.
/// </summary>
public class PropostaResponse
{
    public Guid Id { get; set; }
    public EStatusProposta Status { get; set; }
    public DateTime DataProposta { get; set; }
    public string? Mensagem { get; set; }
}
