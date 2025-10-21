import { createClient } from '@supabase/supabase-js';

const supabaseUrl = process.env.NEXT_PUBLIC_SUPABASE_URL ?? '';
const supabaseAnonKey = process.env.NEXT_PUBLIC_SUPABASE_ANON_KEY ?? '';

if (!supabaseUrl || !supabaseAnonKey) {
  console.warn('Supabase URL ou ANON KEY não configurados. Verifique o arquivo .env.local.');
}

export const supabaseClient = createClient(supabaseUrl, supabaseAnonKey);
