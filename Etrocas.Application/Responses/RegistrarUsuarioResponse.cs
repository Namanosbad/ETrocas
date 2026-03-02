namespace ETrocas.Application.Responses;

/// <summary>
/// Retorno de cadastro de usuário.
/// </summary>
public class RegistrarUsuarioResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
}
