namespace ETrocas.Application.Responses;

/// <summary>
/// Retorno do login com token de acesso.
/// </summary>
public class LoginUsuarioResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
}
