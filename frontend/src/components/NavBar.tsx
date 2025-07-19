import { AppBar, Toolbar, Typography, Button, Box } from '@mui/material';
import { Link as RouterLink } from 'react-router-dom';

export default function NavBar() {
  return (
    <AppBar position="static" sx={{ marginBottom: 2 }}>
      <Toolbar>
        <Typography variant="h6" component={RouterLink} to="/" sx={{ flexGrow: 1, textDecoration: 'none', color: 'inherit' }}>
          EcoGrocery
        </Typography>
        <Box>
          <Button color="inherit" component={RouterLink} to="/cart">Cart</Button>
        </Box>
      </Toolbar>
    </AppBar>
  );
}
