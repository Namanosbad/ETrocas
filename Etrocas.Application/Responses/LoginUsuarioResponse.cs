namespace ETrocas.Application.Responses;
//resposta/ O que o swagger vai devolver
public class LoginUsuarioResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
}