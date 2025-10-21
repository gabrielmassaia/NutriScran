'use client';

import Link from 'next/link';
import { ProductDTO } from '../types';
import { formatCalories, formatHealthScore } from '../utils/formatters';

interface ProductCardProps {
  product: Partial<ProductDTO> & { barcode: string; name: string };
  detailed?: boolean;
}

export default function ProductCard({ product, detailed = false }: ProductCardProps) {
  const { barcode, name, brand, healthScore, imageUrl, nutrients, source } = product;

  return (
    <div className="card border-0">
      {imageUrl && (
        <img src={imageUrl} alt={name} className="card-img-top" style={{ borderRadius: '1rem 1rem 0 0' }} />
      )}
      <div className="card-body">
        <h3 className="h5 fw-bold">{name}</h3>
        {brand && <p className="text-muted mb-2">{brand}</p>}
        {healthScore != null && (
          <span className="badge bg-success-subtle text-success fw-semibold">
            {formatHealthScore(healthScore)}
          </span>
        )}
        {nutrients?.calories != null && (
          <p className="mt-3 mb-0 text-secondary">{formatCalories(nutrients.calories)} por porção</p>
        )}
        <div className="d-flex align-items-center gap-3 mt-4">
          <Link href={`/product/${barcode}`} className="btn btn-outline-success btn-sm">
            Ver detalhes
          </Link>
          {!detailed && source && <small className="text-muted">Fonte: {source}</small>}
        </div>
      </div>
    </div>
  );
}
