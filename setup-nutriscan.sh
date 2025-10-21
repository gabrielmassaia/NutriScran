#!/bin/bash
set -e

echo "🚀 Iniciando setup do projeto NutriScan..."
echo "------------------------------------------"

# --- FRONTEND ---
if [ ! -d "nutriscan-frontend" ]; then
  echo "📦 Criando projeto Next.js 16..."
  npx create-next-app@latest nutriscan-frontend --ts --use-npm --app
else
  echo "⚠️ Diretório 'nutriscan-frontend' já existe, pulando criação..."
fi

echo "📦 Instalando dependências do frontend..."
cd nutriscan-frontend
npm install @supabase/supabase-js axios @zxing/browser recharts @tanstack/react-query bootstrap


# Gera o .env.local.example apenas se não existir
if [ ! -f ".env.local.example" ]; then
  echo "🧩 Gerando .env.local.example..."
  cat > .env.local.example <<'EON'
NEXT_PUBLIC_SUPABASE_URL=https://YOUR-PROJECT.supabase.co
NEXT_PUBLIC_SUPABASE_ANON_KEY=YOUR-ANON-KEY
NEXT_PUBLIC_API_BASE_URL=http://localhost:5000
EON
else
  echo "ℹ️  .env.local.example já existe, mantendo o arquivo atual."
fi

cd ..

# --- BACKEND ---
if [ ! -d "NutriScan.Api" ]; then
  echo "⚙️ Criando API .NET 8..."
  dotnet new webapi -n NutriScan.Api
else
  echo "⚠️ Diretório 'NutriScan.Api' já existe, pulando criação..."
fi

cd NutriScan.Api

echo "⚙️ Instalando pacotes .NET..."
dotnet add package MongoDB.Driver
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Newtonsoft.Json
dotnet add package RestSharp
dotnet add package AutoMapper

mkdir -p Seed

# Gera o .env.example se não existir
if [ ! -f ".env.example" ]; then
  echo "🧩 Gerando .env.example do backend..."
  cat > .env.example <<'EON'
ASPNETCORE_URLS=http://0.0.0.0:5000
MONGODB_URI=mongodb+srv://user:password@cluster.mongodb.net
MONGODB_DB=nutriscan
MONGODB_COLLECTION=products
SUPABASE_PG_HOST=YOUR_PG_HOST.supabase.co
SUPABASE_PG_DB=postgres
SUPABASE_PG_USER=postgres
SUPABASE_PG_PASS=YOUR_POSTGRES_PASSWORD
SUPABASE_PG_PORT=6543
SUPABASE_URL=https://YOUR-PROJECT.supabase.co
SUPABASE_SERVICE_ROLE_KEY=YOUR_SERVICE_ROLE_KEY
OPENFOODFACTS_API_BASE=https://world.openfoodfacts.org/api/v2/product/
SEED=true
EON
else
  echo "ℹ️  .env.example do backend já existe, mantendo o arquivo atual."
fi

cd ..

# --- DOCS ---
mkdir -p docs
if [ ! -f "docs/api.http" ]; then
  echo "📜 Criando arquivo de documentação REST (api.http)..."
  cat > docs/api.http <<'EON'
### Buscar produto por barcode
GET http://localhost:5000/api/products/7891000055123

### Cadastrar manualmente um produto
POST http://localhost:5000/api/products
Content-Type: application/json

{
  "barcode": "0000000000000",
  "name": "Produto Manual",
  "brand": "Marca",
  "nutrients": { "calories": 100, "protein": 5, "fat": 2, "sugar": 8, "sodium": 120 }
}

### Inserir log de consumo
POST http://localhost:5000/api/users/11111111-1111-1111-1111-111111111111/logs
Content-Type: application/json

{
  "barcode": "7891000055123",
  "quantity": 1,
  "meal": "breakfast"
}

### Listar logs
GET http://localhost:5000/api/users/11111111-1111-1111-1111-111111111111/logs

### Stats do usuário
GET http://localhost:5000/api/stats/11111111-1111-1111-1111-111111111111
EON
else
  echo "ℹ️  docs/api.http já existe, mantendo o arquivo atual."
fi

# --- README E .env RAIZ ---
if [ ! -f "README.md" ]; then
  echo "📝 Criando README.md..."
  cat > README.md <<'EON'
# NutriScan

Aplicação web (Next.js 16) + API .NET 8 + Supabase (Postgres/Auth) + MongoDB (produtos).

## Pré-requisitos
- Node.js 20+
- .NET SDK 8
- Conta no Supabase (DB + Auth)
- Conta no MongoDB Atlas

## Setup
1) Copie `.env.local.example` para `nutriscan-frontend/.env.local` e preencha.
2) Copie `.env.example` para `NutriScan.Api/.env` e preencha.
3) Crie as tabelas no Supabase executando `NutriScan.Api/Seed/SupabaseSeed.sql` (ou habilite SEED=true).
4) Inicie os serviços:

### Frontend
cd nutriscan-frontend
npm run dev

### Backend
cd ../NutriScan.Api
dotnet run

Acesse http://localhost:3000 (Next) e http://localhost:5000 (API).

## Deploy
- Frontend: Vercel
- API: Render/Azure
- Banco SQL/Auth: Supabase
- MongoDB: Atlas

EON
else
  echo "ℹ️  README.md já existe, mantendo o arquivo atual."
fi

if [ ! -f ".env.example" ]; then
  echo "🧩 Gerando .env.example raiz..."
  cat > .env.example <<'EON'
# FRONTEND
NEXT_PUBLIC_SUPABASE_URL=
NEXT_PUBLIC_SUPABASE_ANON_KEY=
NEXT_PUBLIC_API_BASE_URL=
NEXT_PUBLIC_DEMO_USER_ID=11111111-1111-1111-1111-111111111111

# BACKEND
ASPNETCORE_URLS=
MONGODB_URI=
MONGODB_DB=
MONGODB_COLLECTION=
SUPABASE_PG_HOST=
SUPABASE_PG_DB=
SUPABASE_PG_USER=
SUPABASE_PG_PASS=
SUPABASE_PG_PORT=
SUPABASE_URL=
SUPABASE_SERVICE_ROLE_KEY=
OPENFOODFACTS_API_BASE=
SEED=true
EON
fi

echo "------------------------------------------"
echo "✅ Setup concluído com sucesso!"
echo "📁 Frontend: ./nutriscan-frontend"
echo "📁 Backend: ./NutriScan.Api"
echo "📁 Documentação: ./docs/api.http"
