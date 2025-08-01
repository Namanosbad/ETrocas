namespace ETrocas.Application.Responses;
//o que o swagger vai devolver.
public class CadastrarProdutoResponse
{
    public Guid UsuarioId { get; set; }
    public Guid Id { get; set; }
    public string? Produto { get; set; }
    public string? Tipo { get; set; }
    public decimal Valor { get; set; }
    public string? Descricao { get; set; }
    public bool Disponivel { get; set; }
    public DateTime DataCadastro { get; set; } = DateTime.Now;
    public string? ImageUrl { get; set; }

}