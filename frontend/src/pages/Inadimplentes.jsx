import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { AlertTriangle, Search, ChevronRight, MessageCircle, Eye, CreditCard, ShieldAlert } from 'lucide-react';
import { getClientes, formatCurrency, formatPhone } from '../services/api';
import './Inadimplentes.css';

const Inadimplentes = () => {
  const [clientes, setClientes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [busca, setBusca] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    const fetchInadimplentes = async () => {
      setLoading(true);
      try {
        const data = await getClientes({
          busca,
          status: 'inadimplente',
        });
        
        // Ordenar por valor excedido do limite (maior para menor)
        data.sort((a, b) => {
          const excedidoA = Math.max(0, a.saldo - a.limiteCredito);
          const excedidoB = Math.max(0, b.saldo - b.limiteCredito);
          return excedidoB - excedidoA;
        });

        setClientes(data);
      } catch (err) {
        console.error('Erro ao buscar inadimplentes:', err);
      } finally {
        setLoading(false);
      }
    };

    const timer = setTimeout(() => fetchInadimplentes(), 300);
    return () => clearTimeout(timer);
  }, [busca]);

  const totalDivida = clientes.reduce((acc, c) => acc + c.saldo, 0);
  const totalExcedido = clientes.reduce((acc, c) => acc + Math.max(0, c.saldo - c.limiteCredito), 0);

  const getWhatsappLink = (telefone, nome, valor) => {
    if (!telefone) return '#';
    const num = telefone.replace(/\D/g, '');
    const msg = encodeURIComponent(`Olá ${nome}, tudo bem? Consta em nosso sistema um valor em aberto de ${formatCurrency(valor)}. Gostaria de verificar a possibilidade de acerto?`);
    return `https://wa.me/55${num}?text=${msg}`;
  };

  if (loading) {
    return (
      <div className="page-loading">
        <div className="loading-spinner danger" />
        <p>Buscando inadimplentes...</p>
      </div>
    );
  }

  return (
    <div className="inadimplentes-page">
      <div className="page-header">
        <div>
          <h1 className="page-title danger-text">
            <ShieldAlert className="inline-icon mr-2" /> Inadimplentes
          </h1>
          <p className="page-subtitle">
            {clientes.length} cliente{clientes.length !== 1 ? 's' : ''} com saldo superior ao limite
          </p>
        </div>
      </div>

      <div className="inadimplentes-stats">
        <div className="stat-card">
          <span className="stat-label">Total Devido (Deste Grupo)</span>
          <span className="stat-val">{formatCurrency(totalDivida)}</span>
        </div>
        <div className="stat-card danger">
          <span className="stat-label">Total Excedido do Limite</span>
          <span className="stat-val danger-text">{formatCurrency(totalExcedido)}</span>
        </div>
      </div>

      <div className="filter-bar">
        <div className="search-input-wrap">
          <Search size={18} className="search-icon" />
          <input
            type="text"
            placeholder="Buscar inadimplente por nome..."
            value={busca}
            onChange={(e) => setBusca(e.target.value)}
            className="search-input"
          />
        </div>
      </div>

      {clientes.length === 0 ? (
        <div className="empty-state success-state">
          <div className="empty-icon-wrap">
            <ShieldAlert size={48} />
          </div>
          <h3>Nenhum Inadimplente</h3>
          <p>Ótima notícia! Nenhum cliente estourou o limite de crédito.</p>
        </div>
      ) : (
        <div className="inadimplentes-grid">
          {clientes.map((cliente, i) => {
            const excedido = cliente.saldo - cliente.limiteCredito;
            
            return (
              <div 
                key={cliente.id} 
                className="inadimplente-card"
                style={{ animationDelay: `${i * 0.05}s` }}
              >
                <div className="card-header">
                  <div className="cliente-id-info">
                    <div className="avatar-danger">
                      {cliente.nome.charAt(0).toUpperCase()}
                    </div>
                    <div>
                      <h3 className="cli-nome">{cliente.nome}</h3>
                      <span className="cli-tel">{formatPhone(cliente.telefone) || 'Sem telefone'}</span>
                    </div>
                  </div>
                  <button 
                    className="btn-icon-only" 
                    title="Ver Detalhes"
                    onClick={() => navigate(`/clientes/${cliente.id}`)}
                  >
                    <Eye size={18} />
                  </button>
                </div>

                <div className="card-finances">
                  <div className="fin-row">
                    <span className="fin-label">Limite Concedido:</span>
                    <span className="fin-val">{formatCurrency(cliente.limiteCredito)}</span>
                  </div>
                  <div className="fin-row">
                    <span className="fin-label">Saldo Atual:</span>
                    <span className="fin-val danger-text fw-bold">{formatCurrency(cliente.saldo)}</span>
                  </div>
                  <div className="fin-row alert-row">
                    <AlertTriangle size={14} className="danger-text mr-1" />
                    <span className="fin-label danger-text">Valor Excedido:</span>
                    <span className="fin-val danger-text fw-bold">{formatCurrency(excedido)}</span>
                  </div>
                </div>

                <div className="card-actions">
                  <a 
                    href={getWhatsappLink(cliente.telefone, cliente.nome, cliente.saldo)} 
                    target="_blank" 
                    rel="noopener noreferrer"
                    className={`btn-act-wpp ${!cliente.telefone ? 'disabled' : ''}`}
                    onClick={(e) => !cliente.telefone && e.preventDefault()}
                  >
                    <MessageCircle size={16} />
                    Cobrar
                  </a>
                  <button 
                    className="btn-act-pay"
                    onClick={() => navigate(`/pagamentos?clienteId=${cliente.id}`)}
                  >
                    <CreditCard size={16} />
                    Receber
                  </button>
                </div>
              </div>
            );
          })}
        </div>
      )}
    </div>
  );
};

export default Inadimplentes;
