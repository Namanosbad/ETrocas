namespace ETrocas.Application.Requests;

public class AtualizarProdutoRequest
{
    public string? Produto { get; set; }
    public string? Tipo { get; set; }
    public decimal Valor { get; set; }
    public string? Descricao { get; set; }
    public bool Disponivel { get; set; }
    public string? ImageUrl { get; set; }
}