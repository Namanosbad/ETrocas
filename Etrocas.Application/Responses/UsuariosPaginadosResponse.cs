namespace ETrocas.Application.Responses;

public class UsuariosPaginadosResponse
{
    public int Pagina { get; set; }
    public int TamanhoPagina { get; set; }
    public int TotalItens { get; set; }
    public IReadOnlyCollection<UsuarioPublicoResponse> Itens { get; set; } = [];
}
