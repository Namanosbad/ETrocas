namespace ETrocas.Application.Requests;

public class RegistrarUsuarioRequest
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string SenhaHash { get; set; }
}