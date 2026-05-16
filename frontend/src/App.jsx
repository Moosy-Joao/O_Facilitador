import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import Login from './pages/Login';
import Dashboard from './pages/Dashboard';
import Clientes from './pages/Clientes';
import ClienteNovo from './pages/ClienteNovo';
import Vendas from './pages/Vendas';
import Pagamentos from './pages/Pagamentos';
import Historico from './pages/Historico';
import AppLayout from './components/Layout/AppLayout';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        {/* Public routes */}
        <Route path="/" element={<Login />} />

        {/* Authenticated routes (with sidebar layout) */}
        <Route element={<AppLayout />}>
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
