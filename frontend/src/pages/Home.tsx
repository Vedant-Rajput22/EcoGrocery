import { useEffect, useState } from 'react';
import Grid from '@mui/material/GridLegacy';
import ProductCard from '../components/ProductCard';
import type { Product } from '../components/ProductCard';

export default function Home() {
  const [products, setProducts] = useState<Product[]>([]);

  useEffect(() => {
    fetch('/api/Products')
      .then(res => res.json())
      .then(data => setProducts(data));
  }, []);

  return (
    <Grid container spacing={2}>
      {products.map(p => (
        <Grid item xs={12} sm={6} md={4} lg={3} key={p.id}>
          <ProductCard product={p} />
        </Grid>
      ))}
    </Grid>
  );
}
