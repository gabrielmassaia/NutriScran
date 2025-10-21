'use client';

import { ResponsiveContainer, AreaChart, Area, XAxis, YAxis, Tooltip, CartesianGrid } from 'recharts';

interface ChartIntakeProps {
  data: Array<{ day: string; calories: number }>;
}

export default function ChartIntake({ data }: ChartIntakeProps) {
  return (
    <div style={{ width: '100%', height: 320 }}>
      <ResponsiveContainer>
        <AreaChart data={data} margin={{ top: 20, right: 30, left: 0, bottom: 0 }}>
          <defs>
            <linearGradient id="colorCalories" x1="0" y1="0" x2="0" y2="1">
              <stop offset="5%" stopColor="#38a169" stopOpacity={0.8} />
              <stop offset="95%" stopColor="#38a169" stopOpacity={0} />
            </linearGradient>
          </defs>
          <XAxis dataKey="day" stroke="#94a3b8" />
          <YAxis stroke="#94a3b8" />
          <CartesianGrid strokeDasharray="3 3" stroke="#e2e8f0" />
          <Tooltip formatter={(value: number) => [`${value.toFixed(0)} kcal`, 'Calorias']} />
          <Area type="monotone" dataKey="calories" stroke="#2f855a" fillOpacity={1} fill="url(#colorCalories)" />
        </AreaChart>
      </ResponsiveContainer>
    </div>
  );
}
