namespace ETrocas.Application.Responses;

/// <summary>
/// Retorno com dados do produto atualizado.
/// </summary>
public class AtualizarProdutoResponse
{
    public Guid Id { get; set; }
    public string? Produto { get; set; }
    public string? Tipo { get; set; }
    public decimal Valor { get; set; }
    public string? Descricao { get; set; }
    public bool Disponivel { get; set; }
    public string? ImageUrl { get; set; }
    public Guid UsuarioId { get; set; }
}
