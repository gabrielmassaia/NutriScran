# Arquitetura NutriScan

O NutriScan é composto por um frontend Next.js (web/webview) que consome uma API .NET 8. A API coordena os fluxos entre MongoDB (produtos) e Supabase (auth e logs nutricionais). OpenFoodFacts é usado como fallback para enriquecer produtos inexistentes.

```mermaid
flowchart LR
  A[Next.js 16 \n (React 19)] -->|REST| B[.NET 8 Web API]
  B -->|Produtos| C[(MongoDB Atlas)]
  B -->|Usuários e Logs| D[(Supabase Postgres/Auth)]
  B -->|Fallback| E[(OpenFoodFacts API)]
```

## Componentes principais

- **Next.js:** App Router, React Query para cache, Supabase Auth client-side.
- **.NET API:** Controllers expõem endpoints REST, Services centralizam regras, Data contexts encapsulam conexões.
- **MongoDB:** Documentos de produtos com nutrientes flexíveis e `HealthScore` calculado.
- **Supabase:** Armazena usuários e logs (`user_logs`).
- **Seeds:** `MongoSeed` e `SupabaseSeed` preparam dados demo.

## Fluxo típico

1. Usuário autentica no Supabase via frontend.
2. Scanner lê código → frontend chama `GET /api/products/{barcode}`.
3. API consulta MongoDB; se não existir chama OpenFoodFacts, normaliza e persiste no MongoDB.
4. Usuário confirma ingestão → frontend envia `POST /api/users/{id}/logs`.
5. API persiste log no Supabase e recalcula estatísticas expostas em `GET /api/stats/{id}`.
