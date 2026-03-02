namespace ETrocas.Application.Requests;

/// <summary>
/// Dados necessários para cadastro de um produto.
/// </summary>
public class CadastrarProdutoRequest
{
    /// <summary>Nome do produto.</summary>
    public string? Produto { get; set; }

    /// <summary>Descrição detalhada do produto.</summary>
    public string? Descricao { get; set; }

    /// <summary>Valor de referência do produto.</summary>
    public decimal Valor { get; set; }

    /// <summary>URL da imagem do produto.</summary>
    public string? Imagem { get; set; }

    /// <summary>Categoria ou tipo do produto.</summary>
    public string? Tipo { get; set; }
}
