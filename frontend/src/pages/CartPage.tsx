import { useCart } from '../contexts/CartContext';
import { Box, Typography, List, ListItem, IconButton } from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';

export default function CartPage() {
  const { items, removeItem, updateQuantity } = useCart();

  const handleQuantityChange = (id: string, e: React.ChangeEvent<HTMLInputElement>) => {
    const qty = parseInt(e.target.value, 10) || 0;
    updateQuantity(id, qty);
  };

  return (
    <Box>
      <Typography variant="h5" sx={{ mb: 2 }}>Cart</Typography>
      <List>
        {items.map(item => (
          <ListItem key={item.id} secondaryAction={
            <IconButton edge="end" aria-label="delete" onClick={() => removeItem(item.id)}>
              <DeleteIcon />
            </IconButton>
          }>
            <Box flexGrow={1}>{item.name}</Box>
            <input type="number" value={item.quantity} min={0} onChange={(e) => handleQuantityChange(item.id, e)} />
            <Box ml={2}>${(item.price * item.quantity).toFixed(2)}</Box>
          </ListItem>
        ))}
      </List>
      {items.length === 0 && <Typography>No items in cart.</Typography>}
    </Box>
  );
}
