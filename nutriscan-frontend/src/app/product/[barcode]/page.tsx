import { notFound } from 'next/navigation';
import ProductCard from '../../../components/ProductCard';
import Scanner from '../../../components/Scanner';
import { getProductByBarcode } from '../../../lib/serverActions';
import NutritionTable from '../../../components/NutritionTable';

interface ProductPageProps {
  params: {
    barcode: string;
  };
}

export default async function ProductPage({ params }: ProductPageProps) {
  const { barcode } = params;
  const product = await getProductByBarcode(barcode);

  if (!product) {
    return notFound();
  }

  return (
    <div className="container py-5">
      <div className="row g-4">
        <div className="col-lg-6">
          <ProductCard product={product} detailed />
          <div className="card p-4 mt-4">
            <h2 className="h5 mb-3">Nutrientes</h2>
            <NutritionTable summary={product.nutrients} />
          </div>
        </div>
        <div className="col-lg-6">
          <div className="card p-4">
            <h2 className="h5 mb-3">Adicionar ao meu dia</h2>
            <Scanner initialBarcode={barcode} />
          </div>
        </div>
      </div>
    </div>
  );
}
