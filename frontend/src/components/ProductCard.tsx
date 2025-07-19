import { Card, CardContent, CardMedia, Typography, CardActions, Button } from '@mui/material';
import { useCart } from '../contexts/CartContext';
import type { CartItem } from '../contexts/CartContext';

export interface Product {
  id: string;
  name: string;
  price: number;
  imageUrl?: string;
}

export default function ProductCard({ product }: { product: Product }) {
  const { addItem } = useCart();
  const handleAdd = () => {
    const item: CartItem = { id: product.id, name: product.name, price: product.price, quantity: 1 };
    addItem(item);
  };

  return (
    <Card sx={{ maxWidth: 345 }}>
      {product.imageUrl && (
        <CardMedia component="img" height="140" image={product.imageUrl} alt={product.name} />
      )}
      <CardContent>
        <Typography gutterBottom variant="h6" component="div">
          {product.name}
        </Typography>
        <Typography variant="body2" color="text.secondary">
          ${product.price.toFixed(2)}
        </Typography>
      </CardContent>
      <CardActions>
        <Button size="small" variant="contained" onClick={handleAdd}>Add to Cart</Button>
      </CardActions>
    </Card>
  );
}
