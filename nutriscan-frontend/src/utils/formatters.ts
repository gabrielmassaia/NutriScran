export const formatCalories = (value?: number) => {
  if (value == null) return '—';
  return `${value.toFixed(0)} kcal`;
};

export const formatNutrient = (value?: number, unit = 'g') => {
  if (value == null) return '—';
  return `${value.toFixed(1)} ${unit}`;
};

export const formatHealthScore = (score: number) => {
  if (Number.isNaN(score)) return 'N/A';
  if (score >= 80) return `${score} • Excelente`;
  if (score >= 60) return `${score} • Bom`;
  if (score >= 40) return `${score} • Moderado`;
  return `${score} • Cuidado`;
};
