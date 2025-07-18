namespace ETrocas.Application.Requests;

public class LoginUsuarioRequest
{
    public string Email { get; set; }
    public string SenhaHash { get; set; }
}