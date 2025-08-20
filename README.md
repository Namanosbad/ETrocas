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
-   **Docker**
-   **Arquitetura em Camadas (Domain, Database, Application, API,
    Shared)**

------------------------------------------------------------------------

## 📂 Estrutura do Projeto

    ETrocas/
    │── ETrocas.Domain/        # Entidades e interfaces de domínio
    │── ETrocas.Database/      # Configurações do banco e migrations
    │── ETrocas.Application/   # Serviços, requests e responses
    │── ETrocas.Api/           # Endpoints (controllers)
    │── ETrocas.Shared/        # Enums, constantes e utilitários

------------------------------------------------------------------------

## 🔑 Funcionalidades Principais

-   Cadastro e autenticação de usuários
-   Cadastro de produtos
-   Criação e gerenciamento de propostas de troca
-   Aceitar ou rejeitar propostas
-   Histórico de trocas realizadas

------------------------------------------------------------------------

## ⚙️ Como Executar o Projeto

### 1️⃣ Clonar o repositório

``` bash
git clone https://github.com/Namanosbad/ETrocas.git
cd ETrocas
```

### 2️⃣ Configurar o Banco de Dados

No arquivo `appsettings.json`, configure a connection string para seu
**SQL Server**.

### 3️⃣ Executar as Migrations

``` bash
dotnet ef database update --project ETrocas.Database --startup-project ETrocas.Api
```

### 4️⃣ Rodar o Projeto

``` bash
dotnet run --project ETrocas.Api
```

A API estará disponível em: **https://localhost:7143** 🚀

------------------------------------------------------------------------

## 📌 Endpoints Principais

### Usuário

-   `POST /api/v1/usuario/registrar` → Registrar novo usuário
-   `POST /api/v1/usuario/login` → Login

### Produtos

-   `GET /api/v1/produto` → Listar produtos
-   `POST /api/v1/produto` → Criar produto

### Propostas

-   `POST /api/v1/proposta` → Criar proposta de troca
-   `GET /api/v1/proposta/minhas` → Minhas propostas feitas (em desenvolvimento)
-   `GET /api/v1/proposta/recebidas` → Propostas recebidas (em desenvolvimento)

------------------------------------------------------------------------

## 🐳 Executando com Docker

``` bash
docker-compose up --build -d
```

------------------------------------------------------------------------

## 👨‍💻 Autor

Desenvolvido por **Namanosbad** 👋\
Se gostou, deixe uma ⭐ no repositório!
