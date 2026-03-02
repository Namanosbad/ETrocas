namespace ETrocas.Application.Requests;

/// <summary>
/// Credenciais para autenticação do usuário.
/// </summary>
public class LoginUsuarioRequest
{
    public string Email { get; set; }
    public string SenhaHash { get; set; }
}
