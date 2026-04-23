import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import Login from './pages/Login';
import Dashboard from './pages/Dashboard';
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
          {/* Futuras rotas aqui */}
          <Route path="/clientes" element={<PlaceholderPage title="Gerenciamento de Clientes" />} />
          <Route path="/clientes/novo" element={<PlaceholderPage title="Novo Cliente" />} />
          <Route path="/vendas" element={<PlaceholderPage title="Registrar Venda" />} />
          <Route path="/pagamentos" element={<PlaceholderPage title="Registrar Pagamento" />} />
          <Route path="/historico" element={<PlaceholderPage title="Histórico de Transações" />} />
        </Route>

        {/* Catch all */}
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </BrowserRouter>
  );
}

/* Temporary placeholder for not-yet-implemented pages */
function PlaceholderPage({ title }) {
  return (
    <div style={{
      display: 'flex',
      flexDirection: 'column',
      alignItems: 'center',
      justifyContent: 'center',
      minHeight: '60vh',
      gap: '1rem',
      color: '#586856',
    }}>
      <h1 style={{
        fontFamily: "'Outfit', sans-serif",
        fontSize: '1.5rem',
        fontWeight: 700,
        color: '#f0f5ee',
        letterSpacing: '-0.5px',
      }}>
        {title}
      </h1>
      <p style={{ fontSize: '0.9rem' }}>Em desenvolvimento — disponível em breve.</p>
    </div>
  );
}

export default App;
