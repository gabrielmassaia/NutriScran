export interface NutrientsDTO {
  calories?: number;
  fat?: number;
  protein?: number;
  sugar?: number;
  sodium?: number;
  carbohydrates?: number;
  fiber?: number;
}

export interface ProductDTO {
  barcode: string;
  name: string;
  brand?: string;
  imageUrl?: string | null;
  nutrients: NutrientsDTO;
  benefits?: string[];
  malefits?: string[];
  healthScore: number;
  source?: string;
  lastUpdated?: string;
}

export interface UserLogDTO {
  barcode: string;
  quantity: number;
  meal: 'breakfast' | 'lunch' | 'dinner' | 'snack';
  scannedAt?: string;
}

export interface UserStatsDTO {
  today: Record<string, number>;
  weeklyCalories: Array<{ day: string; calories: number }>;
}
