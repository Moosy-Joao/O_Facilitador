import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  CreditCard,
  Search,
  ArrowLeft,
  CheckCircle,
  AlertCircle,
  X,
  User,
  DollarSign,
} from 'lucide-react';
import { getClientes, registrarPagamento, formatCurrency } from '../services/api';
import './Vendas.css'; /* Reuses shared styles */
import './Pagamentos.css';

const Pagamentos = () => {
  const navigate = useNavigate();
  const [clientes, setClientes] = useState([]);
  const [busca, setBusca] = useState('');
  const [clienteSelecionado, setClienteSelecionado] = useState(null);
  const [valor, setValor] = useState('');
  const [observacao, setObservacao] = useState('');
  const [loading, setLoading] = useState(false);
  const [success, setSuccess] = useState(false);
  const [error, setError] = useState('');
  const [showResults, setShowResults] = useState(false);
  const [modoQuitacao, setModoQuitacao] = useState(false);

  useEffect(() => {
    if (busca.length >= 2) {
      getClientes({ busca }).then((data) => {
        // Only show clients with balance > 0
        setClientes(data.filter(c => c.saldo > 0));
        setShowResults(true);
      });
    } else {
      setClientes([]);
      setShowResults(false);
    }
  }, [busca]);

  const handleSelectCliente = (cliente) => {
    setClienteSelecionado(cliente);
    setBusca('');
    setShowResults(false);
    setError('');
    setModoQuitacao(false);
    setValor('');
  };

  const handleQuitarTotal = () => {
    if (clienteSelecionado) {
      setValor(clienteSelecionado.saldo.toFixed(2));
      setModoQuitacao(true);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');

    if (!clienteSelecionado) {
      setError('Selecione um cliente');
      return;
    }
    if (!valor || Number(valor) <= 0) {
      setError('Informe um valor válido');
      return;
    }
    if (!observacao.trim()) {
      setError('Informe uma observação');
      return;
    }

    setLoading(true);
    try {
      await registrarPagamento(clienteSelecionado.id, Number(valor), observacao);
      setSuccess(true);
      setTimeout(() => {
        setSuccess(false);
        setClienteSelecionado(null);
        setValor('');
        setObservacao('');
        setModoQuitacao(false);
      }, 3000);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  if (success) {
    return (
      <div className="success-screen">
        <div className="success-icon-wrap pag-success">
          <CheckCircle size={56} strokeWidth={1.5} />
        </div>
        <h2>Pagamento registrado!</h2>
        <p>O saldo devedor do cliente foi atualizado com sucesso.</p>
      </div>
    );
  }

  return (
    <div className="pagamentos-page">
      <div className="page-header">
        <div className="header-with-back">
          <button className="btn-back" onClick={() => navigate('/dashboard')}>
            <ArrowLeft size={18} />
          </button>
          <div>
            <h1 className="page-title">Registrar Pagamento</h1>
            <p className="page-subtitle">Registre um pagamento parcial ou total</p>
          </div>
        </div>
      </div>

      <div className="pagamento-layout">
        {/* Form */}
        <form onSubmit={handleSubmit} className="form-card pagamento-form">
          {error && (
            <div className="form-error-banner">
              <AlertCircle size={16} />
              <span>{error}</span>
            </div>
          )}

          <div className="form-fields">
            {/* Client Search */}
            <div className="form-group">
              <label><User size={16} /> Cliente Devedor</label>
              {clienteSelecionado ? (
                <div className="selected-cliente">
                  <div className="selected-avatar pag-avatar">
                    {clienteSelecionado.nome.charAt(0).toUpperCase()}
                  </div>
                  <div className="selected-info">
                    <span className="selected-name">{clienteSelecionado.nome}</span>
                    <span className="selected-doc">
                      Deve: {formatCurrency(clienteSelecionado.saldo)}
                    </span>
                  </div>
                  <button
                    type="button"
                    className="selected-remove"
                    onClick={() => { setClienteSelecionado(null); setModoQuitacao(false); setValor(''); }}
                  >
                    <X size={14} />
                  </button>
                </div>
              ) : (
                <div className="client-search-wrap">
                  <Search size={16} className="client-search-icon" />
                  <input
                    id="search-cliente-pagamento"
                    type="text"
                    placeholder="Buscar cliente devedor..."
                    value={busca}
                    onChange={(e) => setBusca(e.target.value)}
                    autoComplete="off"
                  />
                  {showResults && clientes.length > 0 && (
                    <div className="client-results">
                      {clientes.map((c) => (
                        <button
                          key={c.id}
                          type="button"
                          className="client-result-item"
                          onClick={() => handleSelectCliente(c)}
                        >
                          <div className="result-avatar pag-result-avatar">{c.nome.charAt(0)}</div>
                          <div className="result-info">
                            <span className="result-name">{c.nome}</span>
                            <span className="result-doc">{c.documento}</span>
                          </div>
                          <span className="result-saldo result-devendo">
                            {formatCurrency(c.saldo)}
                          </span>
                        </button>
                      ))}
                    </div>
                  )}
                  {showResults && clientes.length === 0 && busca.length >= 2 && (
                    <div className="client-results empty">
                      <p>Nenhum cliente devedor encontrado</p>
                    </div>
                  )}
                </div>
              )}
            </div>

            {/* Valor */}
            <div className="form-group">
              <div className="label-with-action">
                <label><DollarSign size={16} /> Valor do Pagamento (R$)</label>
                {clienteSelecionado && !modoQuitacao && (
                  <button type="button" className="btn-quitar" onClick={handleQuitarTotal}>
                    Quitar Total
                  </button>
                )}
              </div>
              <input
                id="valor-pagamento"
                type="number"
                placeholder="0.00"
                step="0.01"
                min="0.01"
                max={clienteSelecionado?.saldo || undefined}
                value={valor}
                onChange={(e) => { setValor(e.target.value); setModoQuitacao(false); }}
              />
              {modoQuitacao && (
                <span className="quitar-hint">
                  <CheckCircle size={13} /> Quitação total selecionada
                </span>
              )}
            </div>

            {/* Observação */}
            <div className="form-group">
              <label>Observação</label>
              <textarea
                id="observacao-pagamento"
                placeholder="Ex: Pagamento parcial da dívida"
                rows={3}
                value={observacao}
                onChange={(e) => setObservacao(e.target.value)}
              />
            </div>
          </div>

          <div className="form-actions">
            <button type="button" className="btn-form-secondary" onClick={() => navigate('/dashboard')}>
              Cancelar
            </button>
            <button
              type="submit"
              className={`btn-form-primary btn-pag ${loading ? 'is-loading' : ''}`}
              disabled={loading}
            >
              {loading ? (
                <span className="btn-spinner" />
              ) : (
                <>
                  <CreditCard size={16} />
                  Registrar Pagamento
                </>
              )}
            </button>
          </div>
        </form>

        {/* Side Panel */}
        {clienteSelecionado && (
          <div className="pagamento-side-panel">
            <div className="side-card">
              <h3 className="side-title">Resumo da Dívida</h3>
              <div className="side-stat">
                <span className="side-label">Saldo Devedor</span>
                <span className="side-value danger">{formatCurrency(clienteSelecionado.saldo)}</span>
              </div>
              <div className="side-stat">
                <span className="side-label">Pagando Agora</span>
                <span className="side-value safe">
                  {valor ? formatCurrency(Number(valor)) : 'R$ 0,00'}
                </span>
              </div>
              <div className="side-divider" />
              <div className="side-stat">
                <span className="side-label bold">Saldo Restante</span>
                <span className={`side-value ${(clienteSelecionado.saldo - Number(valor || 0)) <= 0 ? 'safe' : 'danger'}`}>
                  {formatCurrency(Math.max(0, clienteSelecionado.saldo - Number(valor || 0)))}
                </span>
              </div>

              {valor && Number(valor) >= clienteSelecionado.saldo && (
                <div className="side-alert side-success">
                  <CheckCircle size={16} />
                  <span>Cliente ficará em dia!</span>
                </div>
              )}
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default Pagamentos;
