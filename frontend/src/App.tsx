import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { ThemeProvider, createTheme, Container } from '@mui/material';
import NavBar from './components/NavBar';
import Home from './pages/Home';
import CartPage from './pages/CartPage';
import { CartProvider } from './contexts/CartContext';

const theme = createTheme({
  palette: {
    primary: {
      main: '#2e7d32',
    },
    secondary: {
      main: '#81c784',
    },
  },
});

function App() {
  return (
    <ThemeProvider theme={theme}>
      <BrowserRouter>
        <CartProvider>
          <NavBar />
          <Container sx={{ mt: 2 }}>
            <Routes>
              <Route path="/" element={<Home />} />
              <Route path="/cart" element={<CartPage />} />
            </Routes>
          </Container>
        </CartProvider>
      </BrowserRouter>
    </ThemeProvider>
  );
}

export default App;
