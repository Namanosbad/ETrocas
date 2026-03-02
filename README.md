# 📦 ETrocas - (em desenvolvimento)

Sistema de **trocas de produtos** desenvolvido em **.NET 8** com
arquitetura em camadas, pensado para oferecer uma forma prática, segura e organizada de negociar bens entre usuários.

A aplicação permite que pessoas cadastrem seus produtos e façam propostas de troca. A aplicação possibilita aceitar ou recusar ofertas, acompanhar o histórico de negociações e manter todo o processo de troca documentado e transparente.

O projeto foi construído com foco em boas práticas de desenvolvimento, separação de responsabilidades e escalabilidade.

O objetivo principal do ETrocas é demonstrar na prática como aplicar arquitetura limpa em um sistema real, promovendo código de fácil manutenção e com potencial de crescimento para novas funcionalidades.
------------------------------------------------------------------------

## 🚀 Tecnologias Utilizadas

-   **ASP.NET Core 8 (Web API)**
-   **Entity Framework Core 8**
-   **SQL Server**
-   **JWT (Bearer Token)**
-   **Swagger / OpenAPI**
-   **Arquitetura em Camadas (Domain, Database, Application, API, IoC, Shared)**

------------------------------------------------------------------------

## 📂 Estrutura do Projeto (pastas reais)

    ETrocas/
    │── ETrocas.API.Internal/   # API (controllers, Program, configs)
    │── Etrocas.Application/    # Serviços, requests e responses
    │── ETrocas.Database/       # DbContext, repositórios e migrations
    │── ETrocas.Domain/         # Entidades e interfaces de domínio
    │── ETrocas.Ioc/            # Injeção de dependências e configuração de serviços
    │── ETrocas.Shared/         # Configurações e serviços compartilhados

------------------------------------------------------------------------

## 🔑 Funcionalidades Principais

-   Cadastro e autenticação de usuários
-   Cadastro, atualização, consulta e remoção de produtos
-   Criação de propostas de troca
-   Versionamento de API (`v1`)
-   Autenticação com JWT em rotas protegidas

------------------------------------------------------------------------

## ⚙️ Como Executar o Projeto

### 1️⃣ Clonar o repositório

```bash
git clone https://github.com/Namanosbad/ETrocas.git
cd ETrocas
```

### 2️⃣ Configurar o Banco de Dados

No arquivo `ETrocas.API.Internal/appsettings.json`, configure:

- `DbConfig:ConnectionString`
- `TokenConfig:Key`

### 3️⃣ Executar as Migrations

```bash
dotnet ef database update --project ETrocas.Database --startup-project ETrocas.API.Internal
```

### 4️⃣ Rodar a API

```bash
dotnet run --project ETrocas.API.Internal
```

A API estará disponível conforme as URLs definidas em:
`ETrocas.API.Internal/Properties/launchSettings.json`.

------------------------------------------------------------------------

## 📌 Endpoints atuais (v1)

### Usuário (`CadastrarUsuarioController`)

-   `POST /api/v1/CadastrarUsuario/registrar`
-   `POST /api/v1/CadastrarUsuario/login`

### Produtos (`ProdutosController`)

-   `POST /api/v1/Produtos/CadastrarProduto` (autenticado)
-   `GET /api/v1/Produtos/BuscarTodosProdutos`
-   `GET /api/v1/Produtos/BuscarProduto/{id}` (autenticado)
-   `PUT /api/v1/Produtos/AtualizarProduto/{id}` (autenticado)
-   `DELETE /api/v1/Produtos/DeletarProduto?id={id}` (autenticado)

### Propostas (`PropostaController`)

-   `POST /api/v1/Proposta/FazerProposta/{id}` (autenticado)

------------------------------------------------------------------------

## 🐳 Docker

Atualmente o repositório contém apenas o `Dockerfile` da API em
`ETrocas.API.Internal/Dockerfile`.

> Não há arquivo `docker-compose.yml` versionado neste momento.

------------------------------------------------------------------------

## 👨‍💻 Autor

Desenvolvido por **Namanosbad** 👋  
Se gostou, deixe uma ⭐ no repositório!
