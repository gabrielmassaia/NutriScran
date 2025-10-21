import Link from 'next/link';
import ProductCard from '../components/ProductCard';

const heroCards = [
  {
    barcode: '7891000055123',
    name: 'Iogurte Natural Integral',
    brand: 'Nestlé',
    healthScore: 82
  },
  {
    barcode: '7894900011517',
    name: 'Biscoito Recheado',
    brand: 'Delícia',
    healthScore: 45
  }
];

export default function HomePage() {
  return (
    <div className="container py-5">
      <div className="row align-items-center mb-5">
        <div className="col-lg-6">
          <h1 className="display-4 fw-bold mb-3">NutriScan</h1>
          <p className="lead text-muted mb-4">
            Escaneie códigos de barras, descubra o perfil nutricional de cada produto e acompanhe
            suas metas diárias com insights inteligentes.
          </p>
          <div className="d-flex gap-3">
            <Link href="/product/demo" className="btn btn-nutriscan btn-lg px-4 py-2">
              Escanear agora
            </Link>
            <Link href="/dashboard" className="btn btn-outline-success btn-lg px-4 py-2">
              Ver dashboard
            </Link>
          </div>
        </div>
        <div className="col-lg-6 text-center">
          <img
            className="img-fluid rounded-4 shadow-lg"
            alt="Dashboard NutriScan"
            src="https://images.unsplash.com/photo-1546069901-ba9599a7e63c?auto=format&fit=crop&w=800&q=80"
          />
        </div>
      </div>

      <section>
        <h2 className="h4 mb-4">Produtos recentes</h2>
        <div className="row g-4">
          {heroCards.map((card) => (
            <div className="col-md-6" key={card.barcode}>
              <ProductCard product={card} />
            </div>
          ))}
        </div>
      </section>
    </div>
  );
}
