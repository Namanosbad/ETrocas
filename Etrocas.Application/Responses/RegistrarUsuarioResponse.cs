﻿namespace ETrocas.Application.Responses;

public class RegistrarUsuarioResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
}