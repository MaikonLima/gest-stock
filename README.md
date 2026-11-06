# Gerenciamento de Estoque

Sistema de gestão de estoque com API em NestJS (`backend-estoque`) e app desktop em Avalonia/.NET (`frontend-estoque`).

## Pré-requisitos

- [Node.js](https://nodejs.org/) 20+ e [pnpm](https://pnpm.io/) (`npm install -g pnpm`)
- [Docker](https://www.docker.com/) (para o banco Postgres)
- [.NET SDK 10](https://dotnet.microsoft.com/download)

## 1. Backend (API)

```bash
cd backend-estoque
pnpm install          # também gera o Prisma Client (postinstall)
docker compose up -d  # sobe o Postgres em localhost:5432
pnpm exec prisma migrate deploy   # aplica as migrations no banco
pnpm start:dev        # API em http://localhost:3000 (docs em /docs)
```

A conexão com o banco é lida de `backend-estoque/.env` (`DATABASE_URL`), já configurado para o `docker-compose.yml` do projeto.

## 2. Frontend (Avalonia)

Com a API rodando:

```bash
cd frontend-estoque
dotnet run
```

A janela abre já conectada em `http://localhost:3000` e carrega Categorias, Fornecedores, Produtos e Movimentações.

> A URL da API está fixa em `Services/ApiClient.cs`. Se a API rodar em outro host/porta, ajuste o valor padrão do construtor de `ApiClient`.

## Estrutura

- `backend-estoque/` — API NestJS + Prisma (Postgres). Testes: `pnpm test`.
- `frontend-estoque/` — App desktop Avalonia (MVVM, CommunityToolkit.Mvvm). Build: `dotnet build`.
