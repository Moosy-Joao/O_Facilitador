import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import Login from './pages/Login';
import Dashboard from './pages/Dashboard';
import Clientes from './pages/Clientes';
import ClienteNovo from './pages/ClienteNovo';
import Vendas from './pages/Vendas';
import Pagamentos from './pages/Pagamentos';
import Historico from './pages/Historico';
import AppLayout from './components/Layout/AppLayout';
import { isAuthenticated } from './services/api';

/**
 * Componente de rota protegida.
 * Redireciona para login se o usuário não tiver token JWT.
 */
const PrivateRoute = ({ children }) => {
  return isAuthenticated() ? children : <Navigate to="/" replace />;
};

function App() {
  return (
    <BrowserRouter>
      <Routes>
        {/* Public routes */}
        <Route path="/" element={<Login />} />

        {/* Authenticated routes (with sidebar layout) */}
        <Route element={<PrivateRoute><AppLayout /></PrivateRoute>}>
          <Route path="/dashboard" element={<Dashboard />} />
          <Route path="/clientes" element={<Clientes />} />
          <Route path="/clientes/novo" element={<ClienteNovo />} />
          <Route path="/vendas" element={<Vendas />} />
          <Route path="/pagamentos" element={<Pagamentos />} />
          <Route path="/historico" element={<Historico />} />
        </Route>

        {/* Catch all */}
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;

