import { Suspense } from 'react';
import ChartIntake from '../../components/ChartIntake';
import NutritionTable from '../../components/NutritionTable';
import { fetchWeeklyStats } from '../../lib/serverActions';

export const dynamic = 'force-dynamic';

export default async function DashboardPage() {
  const stats = await fetchWeeklyStats();

  return (
    <div className="container py-5">
      <h1 className="display-5 fw-bold mb-4">Seu painel nutricional</h1>
      <p className="text-muted mb-5">
        Acompanhe as calorias, macros e descubra padrões de consumo ao longo da semana.
      </p>
      <div className="row g-4">
        <div className="col-lg-7">
          <div className="card p-4">
            <h2 className="h5 mb-3">Consumo calórico semanal</h2>
            <Suspense fallback={<p>Carregando gráfico...</p>}>
              <ChartIntake data={stats.weeklyCalories} />
            </Suspense>
          </div>
        </div>
        <div className="col-lg-5">
          <div className="card p-4 h-100">
            <h2 className="h5 mb-3">Resumo diário</h2>
            <NutritionTable summary={stats.today} />
          </div>
        </div>
      </div>
    </div>
  );
}
