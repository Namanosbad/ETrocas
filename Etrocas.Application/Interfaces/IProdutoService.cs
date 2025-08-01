﻿using ETrocas.Application.Requests;
using ETrocas.Application.Responses;

namespace ETrocas.Application.Interfaces;
//interface do ProdutoService que vai implementar o repository
public interface IProdutoService
{
    Task<CadastrarProdutoResponse> CadastrarProdutoAsync(CadastrarProdutoRequest cadastrarProdutoRequest, Guid usuarioGuid);
}