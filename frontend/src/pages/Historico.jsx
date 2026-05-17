import { useState, useEffect } from 'react';
import {
  History,
  Search,
  Filter,
  ChevronDown,
  ShoppingCart,
  CreditCard,
  RotateCcw,
  X,
  AlertCircle,
} from 'lucide-react';
import { getTransacoes, estornarTransacao, formatCurrency, formatDateTime } from '../services/api';
import './Historico.css';

const Historico = () => {
  const [transacoes, setTransacoes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [busca, setBusca] = useState('');
  const [filtroTipo, setFiltroTipo] = useState('todos');
  const [showFilterMenu, setShowFilterMenu] = useState(false);
  const [estornando, setEstornando] = useState(null);
  const [confirmEstorno, setConfirmEstorno] = useState(null);
  const [error, setError] = useState('');

  const fetchTransacoes = async () => {
    try {
      const data = await getTransacoes({ busca, tipo: filtroTipo });
      setTransacoes(data);
    } catch (err) {
      console.error('Erro ao buscar transações:', err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    setLoading(true);
    const timer = setTimeout(() => fetchTransacoes(), 300);
    return () => clearTimeout(timer);
  }, [busca, filtroTipo]);

  const handleEstornar = async (id) => {
    setEstornando(id);
    setError('');
    try {
      await estornarTransacao(id);
      await fetchTransacoes();
      setConfirmEstorno(null);
    } catch (err) {
      setError(err.message);
    } finally {
      setEstornando(null);
    }
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

  const totalEstornos = transacoes.filter(t => t.status === 'estornado').length;

  if (loading) {
    return (
      <div className="page-loading">
        <div className="loading-spinner" />
        <p>Carregando histórico...</p>
      </div>
    );
  }

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
        <div className="summary-card">
          <div className="summary-icon estorno-icon">
            <RotateCcw size={18} />
          </div>
          <div className="summary-data">
            <span className="summary-label">Estornos</span>
            <span className="summary-value muted">{totalEstornos}</span>
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
            value={busca}
            onChange={(e) => setBusca(e.target.value)}
            className="search-input"
          />
          {busca && (
            <button className="search-clear" onClick={() => setBusca('')}>
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

      {error && (
        <div className="form-error-banner" style={{ marginBottom: '1rem' }}>
          <AlertCircle size={16} />
          <span>{error}</span>
        </div>
      )}

      {/* Transaction List */}
      {transacoes.length === 0 ? (
        <div className="empty-state">
          <History size={48} strokeWidth={1} />
          <h3>Nenhuma transação encontrada</h3>
          <p>Ajuste os filtros ou registre novas vendas/pagamentos.</p>
        </div>
      ) : (
        <div className="historico-list">
          {transacoes.map((t, i) => (
            <div
              key={t.id}
              className={`historico-item ${t.status === 'estornado' ? 'estornado' : ''}`}
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
                  {t.status === 'estornado' && (
                    <span className="historico-tipo-badge estorno-badge">Estornado</span>
                  )}
                </div>
                <span className="historico-desc">{t.descricao}</span>
              </div>

              <div className="historico-right">
                <span className={`historico-valor ${t.tipo === 'venda' ? 'val-out' : 'val-in'} ${t.status === 'estornado' ? 'estornado-val' : ''}`}>
                  {t.tipo === 'pagamento' ? '+' : ''}{formatCurrency(t.valor)}
                </span>
                <span className="historico-data">{formatDateTime(t.data)}</span>
              </div>

              <div className="historico-actions">
                {t.status !== 'estornado' && (
                  <button
                    className="btn-estornar"
                    title="Estornar transação"
                    onClick={() => setConfirmEstorno(t)}
                  >
                    <RotateCcw size={14} />
                  </button>
                )}
              </div>
            </div>
          ))}
        </div>
      )}

      {/* Estorno Confirmation Modal */}
      {confirmEstorno && (
        <div className="modal-overlay" onClick={() => setConfirmEstorno(null)}>
          <div className="modal-content estorno-modal" onClick={(e) => e.stopPropagation()}>
            <button className="modal-close" onClick={() => setConfirmEstorno(null)}>
              <X size={18} />
            </button>

            <div className="estorno-header">
              <div className="estorno-icon-wrap">
                <RotateCcw size={28} />
              </div>
              <h2>Confirmar Estorno</h2>
              <p>Esta ação não pode ser desfeita. A transação será marcada como estornada e o saldo do cliente será revertido.</p>
            </div>

            <div className="estorno-details">
              <div className="estorno-detail-row">
                <span>Cliente</span>
                <strong>{confirmEstorno.clienteNome}</strong>
              </div>
              <div className="estorno-detail-row">
                <span>Tipo</span>
                <strong>{confirmEstorno.tipo === 'venda' ? 'Venda' : 'Pagamento'}</strong>
              </div>
              <div className="estorno-detail-row">
                <span>Valor</span>
                <strong>{formatCurrency(confirmEstorno.valor)}</strong>
              </div>
              <div className="estorno-detail-row">
                <span>Descrição</span>
                <strong>{confirmEstorno.descricao}</strong>
              </div>
            </div>

            <div className="estorno-actions">
              <button className="btn-form-secondary" onClick={() => setConfirmEstorno(null)}>
                Cancelar
              </button>
              <button
                className={`btn-estorno-confirm ${estornando ? 'is-loading' : ''}`}
                onClick={() => handleEstornar(confirmEstorno.id)}
                disabled={estornando}
              >
                {estornando ? (
                  <span className="btn-spinner" />
                ) : (
                  <>
                    <RotateCcw size={16} />
                    Confirmar Estorno
                  </>
                )}
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default Historico;
