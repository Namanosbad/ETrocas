namespace ETrocas.Application.Requests;
//pergunta / o que o usuario vai colocar
public class LoginUsuarioRequest
{
    public string Email { get; set; }
    public string SenhaHash { get; set; }
}