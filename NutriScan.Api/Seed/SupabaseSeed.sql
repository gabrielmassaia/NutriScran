-- Tipo enumerado para refeição
DO $$
BEGIN
  IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'meal_type') THEN
    CREATE TYPE meal_type AS ENUM ('breakfast','lunch','dinner','snack');
  END IF;
END$$;

-- Tabela de usuários (espelho simplificado)
CREATE TABLE IF NOT EXISTS public.users (
  id UUID PRIMARY KEY,
  name TEXT,
  email TEXT UNIQUE,
  created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- Tabela de produtos cacheados opcionalmente para relatórios
CREATE TABLE IF NOT EXISTS public.products_cache (
  barcode TEXT PRIMARY KEY,
  name TEXT,
  nutrients JSONB DEFAULT '{}'::jsonb
);

-- Logs de consumo
CREATE TABLE IF NOT EXISTS public.user_logs (
  id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  user_id UUID NOT NULL REFERENCES public.users(id) ON DELETE CASCADE,
  barcode TEXT NOT NULL,
  scanned_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
  quantity NUMERIC(10,2) DEFAULT 1,
  meal meal_type DEFAULT 'snack'
);

CREATE INDEX IF NOT EXISTS idx_user_logs_user_id ON public.user_logs(user_id);
CREATE INDEX IF NOT EXISTS idx_user_logs_barcode ON public.user_logs(barcode);

-- Usuários seeds
INSERT INTO public.users (id, name, email)
VALUES
  ('11111111-1111-1111-1111-111111111111','Usuário Demo','demo@nutriscan.dev'),
  ('22222222-2222-2222-2222-222222222222','Usuário Saúde','saude@nutriscan.dev')
ON CONFLICT (id) DO NOTHING;

-- Produtos cache seeds para relatórios rápidos
INSERT INTO public.products_cache (barcode, name, nutrients) VALUES
  ('7891000055123','Iogurte Natural Integral','{"Calories":62,"Protein":3,"Fat":3.3,"Sugar":4.6}'::jsonb),
  ('7894900011517','Biscoito Recheado Chocolate','{"Calories":480,"Protein":5,"Fat":20,"Sugar":38}'::jsonb),
  ('7891000311304','Cereal Integral Aveia e Mel','{"Calories":210,"Protein":6,"Fat":4,"Sugar":12}'::jsonb),
  ('7891991010848','Refrigerante Cola','{"Calories":140,"Protein":0,"Fat":0,"Sugar":39}'::jsonb),
  ('7898080641234','Mix de Castanhas Premium','{"Calories":160,"Protein":6,"Fat":14,"Sugar":2}'::jsonb)
ON CONFLICT (barcode) DO UPDATE SET nutrients = EXCLUDED.nutrients;

-- Logs seeds
INSERT INTO public.user_logs (user_id, barcode, scanned_at, quantity, meal)
VALUES
  ('11111111-1111-1111-1111-111111111111','7891000055123', NOW() - INTERVAL '1 day', 1, 'breakfast'),
  ('11111111-1111-1111-1111-111111111111','7894900011517', NOW() - INTERVAL '12 hours', 1, 'snack'),
  ('11111111-1111-1111-1111-111111111111','7898080641234', NOW() - INTERVAL '2 days', 0.5, 'snack'),
  ('22222222-2222-2222-2222-222222222222','7891000311304', NOW() - INTERVAL '3 days', 1, 'breakfast'),
  ('22222222-2222-2222-2222-222222222222','7891991010848', NOW() - INTERVAL '6 hours', 1, 'dinner')
ON CONFLICT DO NOTHING;
