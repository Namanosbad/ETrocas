namespace ETrocas.Application.Requests;
//o que o usuario precisa colocar
public class CadastrarProdutoRequest
{
    public string? Produto { get; set; }
    public string? Descricao { get; set; }
    public decimal Valor { get; set; }
    public string? Imagem { get; set; }
    public string? Tipo { get; set; }
}