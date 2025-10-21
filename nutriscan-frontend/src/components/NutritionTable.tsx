'use client';

import { NutrientsDTO } from '../types';
import { formatCalories, formatNutrient } from '../utils/formatters';

interface NutritionTableProps {
  summary: NutrientsDTO | Record<string, number>;
}

export default function NutritionTable({ summary }: NutritionTableProps) {
  if (!summary) {
    return <p className="text-muted">Nenhum dado disponível.</p>;
  }

  const entries = Object.entries(summary);

  return (
    <table className="table table-borderless align-middle">
      <tbody>
        {entries.map(([key, value]) => {
          const normalizedKey = key.replace(/([A-Z])/g, ' $1').replace(/^./, (s) => s.toUpperCase());
          const formattedValue = key.toLowerCase().includes('calor')
            ? formatCalories(Number(value))
            : formatNutrient(Number(value));

          return (
            <tr key={key}>
              <th className="text-muted fw-semibold" scope="row">
                {normalizedKey}
              </th>
              <td className="text-end">{formattedValue}</td>
            </tr>
          );
        })}
      </tbody>
    </table>
  );
}
