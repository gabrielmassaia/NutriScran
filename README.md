# NutriScan

Monorepo do NutriScan contendo frontend (Next.js), backend (.NET 8), seeds e documentação.

## Visão geral

- **Frontend:** Next.js (App Router, React Server Components), TypeScript, Bootstrap, React Query.
- **Backend:** .NET 8 Web API com arquitetura em camadas, MongoDB para produtos, Supabase/Postgres para usuários e logs.
- **Integrações:** OpenFoodFacts como fallback para consulta de produtos.
- **Infra alvo:** Vercel (web), Render/Azure (API), Supabase Cloud e MongoDB Atlas.

## Estrutura

```
/nutriscan
├─ setup-nutriscan.sh
├─ .env.example
├─ README.md
├─ nutriscan-frontend/
│  ├─ .env.local.example
│  └─ src/...
├─ NutriScan.Api/
│  ├─ .env.example
│  ├─ Seed/
│  │  ├─ MongoSeed.cs
│  │  └─ SupabaseSeed.sql
│  └─ ...
└─ docs/
   ├─ api.http
   └─ architecture.md
```

## Pré-requisitos

- Node.js 20+
- .NET SDK 8
- Conta Supabase (Auth + Postgres)
- Conta MongoDB Atlas

## Configuração rápida

1. Copie `.env.local.example` para `nutriscan-frontend/.env.local` e preencha.
2. Copie `NutriScan.Api/.env.example` para `NutriScan.Api/.env` e defina as credenciais.
3. Opcional: execute `setup-nutriscan.sh` para recriar estrutura do zero (apenas ambientes novos).
4. Execute os seeds (Mongo e Supabase) manualmente ou deixe `SEED=true` nas variáveis para rodar automaticamente ao iniciar a API.

### Rodando o frontend

```
cd nutriscan-frontend
npm install
npm run dev
```

O frontend ficará disponível em `http://localhost:3000`.

### Rodando a API

```
cd NutriScan.Api
dotnet restore
dotnet run
```

A API ficará disponível em `http://localhost:5000`.

## Deploy

- **Frontend:** Vercel (importar diretório `nutriscan-frontend`).
- **API:** Render, Azure App Service ou outra plataforma compatível com .NET 8.
- **Banco relacional/Auth:** Supabase (Postgres gerenciado + autenticação).
- **Banco de documentos:** MongoDB Atlas.

## Checklist de aceite

- [ ] `GET /api/products/{barcode}` busca no Mongo, usa fallback OpenFoodFacts e salva.
- [ ] `POST /api/products` cadastra produtos manualmente.
- [ ] `POST /api/users/{id}/logs` insere log no Postgres (Supabase).
- [ ] `GET /api/users/{id}/logs` retorna histórico ordenado.
- [ ] `GET /api/stats/{id}` retorna totais/calorias por dia/semana.
- [ ] Seeds com 3–5 produtos (Mongo) e 1 usuário + 4–6 logs (Supabase).
- [ ] Frontend integra com Supabase Auth, scanner e dashboard.
- [ ] Documentação e exemplos REST disponíveis em `docs/`.

## Referências úteis

- [Next.js Documentation](https://nextjs.org/docs)
- [Supabase Docs](https://supabase.com/docs)
- [MongoDB Atlas](https://www.mongodb.com/atlas)
- [OpenFoodFacts API](https://world.openfoodfacts.org/data)
