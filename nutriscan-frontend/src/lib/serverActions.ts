import 'server-only';

import { cache } from 'react';
import { ProductDTO, UserStatsDTO } from '../types';

const API_BASE = process.env.API_BASE_URL ?? process.env.NEXT_PUBLIC_API_BASE_URL ?? 'http://localhost:5000';
const DEMO_USER = process.env.DEMO_USER_ID ?? '11111111-1111-1111-1111-111111111111';

export const getProductByBarcode = cache(async (barcode: string): Promise<ProductDTO | null> => {
  try {
    const response = await fetch(`${API_BASE}/api/products/${barcode}`, {
      next: { revalidate: 60 }
    });

    if (!response.ok) {
      return null;
    }

    return (await response.json()) as ProductDTO;
  } catch (error) {
    console.error('Falha ao buscar produto', error);
    return null;
  }
});

export const fetchWeeklyStats = cache(async (): Promise<UserStatsDTO> => {
  try {
    const response = await fetch(`${API_BASE}/api/stats/${DEMO_USER}`, {
      next: { revalidate: 30 }
    });
    if (!response.ok) {
      throw new Error('Resposta inválida da API');
    }
    return (await response.json()) as UserStatsDTO;
  } catch (error) {
    console.error('Falha ao buscar estatísticas', error);
    return {
      today: { calories: 0, protein: 0, fat: 0, sugar: 0 },
      weeklyCalories: []
    };
  }
});
