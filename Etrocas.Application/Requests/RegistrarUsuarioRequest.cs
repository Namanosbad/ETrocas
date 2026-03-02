namespace ETrocas.Application.Requests;

/// <summary>
/// Dados necessários para registrar um usuário.
/// </summary>
public class RegistrarUsuarioRequest
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string SenhaHash { get; set; }
}
