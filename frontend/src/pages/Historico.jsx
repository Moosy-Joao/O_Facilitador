import { useState, useEffect } from 'react';
import { useQuery } from '@tanstack/react-query';
import {
  History,
  Search,
  Filter,
  ChevronDown,
  ShoppingCart,
  CreditCard,
  X,
} from 'lucide-react';
import { getTransacoes, formatCurrency, formatDateTime } from '../services/api';
import './Historico.css';

const Historico = () => {
  const [buscaInput, setBuscaInput] = useState('');
  const [buscaDebounced, setBuscaDebounced] = useState('');
  const [filtroTipo, setFiltroTipo] = useState('todos');
  const [showFilterMenu, setShowFilterMenu] = useState(false);

  // Debounce buscaInput para buscaDebounced
  useEffect(() => {
    const timer = setTimeout(() => {
      setBuscaDebounced(buscaInput);
    }, 300);
    return () => clearTimeout(timer);
  }, [buscaInput]);

  // React Query para buscar transações
  const { data: transacoes = [], isLoading } = useQuery({
    queryKey: ['transacoes', buscaDebounced, filtroTipo],
    queryFn: () => getTransacoes({ busca: buscaDebounced, tipo: filtroTipo }),
  });

  const handleClear = () => {
    setBuscaInput('');
    setBuscaDebounced('');
  };

  const filterLabels = {
    todos: 'Todas',
    venda: 'Vendas',
    pagamento: 'Pagamentos',
  };

  // Summary stats
  const totalVendas = transacoes
    .filter(t => t.tipo === 'venda' && t.status === 'concluido')
    .reduce((sum, t) => sum + t.valor, 0);

  const totalPagamentos = transacoes
    .filter(t => t.tipo === 'pagamento' && t.status === 'concluido')
    .reduce((sum, t) => sum + t.valor, 0);

  return (
    <div className="historico-page">
      {/* Header */}
      <div className="page-header">
        <div>
          <h1 className="page-title">Histórico</h1>
          <p className="page-subtitle">
            {transacoes.length} transaç{transacoes.length !== 1 ? 'ões' : 'ão'} encontrada{transacoes.length !== 1 ? 's' : ''}
          </p>
        </div>
      </div>

      {/* Summary Cards */}
      <div className="historico-summary">
        <div className="summary-card">
          <div className="summary-icon venda-icon">
            <ShoppingCart size={18} />
          </div>
          <div className="summary-data">
            <span className="summary-label">Total Vendas</span>
            <span className="summary-value">{formatCurrency(totalVendas)}</span>
          </div>
        </div>
        <div className="summary-card">
          <div className="summary-icon pag-icon">
            <CreditCard size={18} />
          </div>
          <div className="summary-data">
            <span className="summary-label">Total Recebido</span>
            <span className="summary-value safe">{formatCurrency(totalPagamentos)}</span>
          </div>
        </div>
      </div>

      {/* Filter Bar */}
      <div className="filter-bar">
        <div className="search-input-wrap">
          <Search size={18} className="search-icon" />
          <input
            id="search-historico"
            type="text"
            placeholder="Buscar por cliente ou descrição..."
            value={buscaInput}
            onChange={(e) => setBuscaInput(e.target.value)}
            className="search-input"
          />
          {buscaInput && (
            <button className="search-clear" onClick={handleClear}>
              <X size={14} />
            </button>
          )}
        </div>

        <div className="filter-dropdown-wrap">
          <button
            className="filter-dropdown-btn"
            onClick={() => setShowFilterMenu(!showFilterMenu)}
          >
            <Filter size={16} />
            {filterLabels[filtroTipo]}
            <ChevronDown size={14} />
          </button>
          {showFilterMenu && (
            <div className="filter-dropdown-menu">
              {Object.entries(filterLabels).map(([key, label]) => (
                <button
                  key={key}
                  className={`filter-option ${filtroTipo === key ? 'active' : ''}`}
                  onClick={() => { setFiltroTipo(key); setShowFilterMenu(false); }}
                >
                  {label}
                </button>
              ))}
            </div>
          )}
        </div>
      </div>

      {/* Transaction List */}
      {transacoes.length === 0 && !isLoading ? (
        <div className="empty-state">
          <History size={48} strokeWidth={1} />
          <h3>Nenhuma transação encontrada</h3>
          <p>Ajuste os filtros ou registre novas vendas/pagamentos.</p>
        </div>
      ) : (
        <div className="historico-list" style={{ opacity: isLoading ? 0.6 : 1, transition: 'opacity 0.2s' }}>
          {transacoes.map((t, i) => (
            <div
              key={t.id}
              className="historico-item"
              style={{ animationDelay: `${i * 0.04}s` }}
            >
              <div className={`historico-icon ${t.tipo === 'venda' ? 'tipo-venda' : 'tipo-pag'}`}>
                {t.tipo === 'venda' ? <ShoppingCart size={18} /> : <CreditCard size={18} />}
              </div>

              <div className="historico-main">
                <div className="historico-top">
                  <span className="historico-nome">{t.clienteNome}</span>
                  <span className={`historico-tipo-badge ${t.tipo}`}>
                    {t.tipo === 'venda' ? 'Venda' : 'Pagamento'}
                  </span>
                </div>
                <span className="historico-desc">{t.descricao}</span>
              </div>

              <div className="historico-right">
                <span className={`historico-valor ${t.tipo === 'venda' ? 'val-out' : 'val-in'}`}>
                  {t.tipo === 'pagamento' ? '+' : ''}{formatCurrency(t.valor)}
                </span>
                <span className="historico-data">{formatDateTime(t.data)}</span>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default Historico;
