import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  ShoppingCart,
  Search,
  ArrowLeft,
  CheckCircle,
  AlertCircle,
  AlertTriangle,
  X,
  User,
} from 'lucide-react';
import { getClientes, registrarVenda, formatCurrency } from '../services/api';
import './Vendas.css';

const Vendas = () => {
  const navigate = useNavigate();
  const [clientes, setClientes] = useState([]);
  const [busca, setBusca] = useState('');
  const [clienteSelecionado, setClienteSelecionado] = useState(null);
  const [valor, setValor] = useState('');
  const [descricao, setDescricao] = useState('');
  const [loading, setLoading] = useState(false);
  const [success, setSuccess] = useState(false);
  const [error, setError] = useState('');
  const [showResults, setShowResults] = useState(false);

  useEffect(() => {
    if (busca.length >= 2) {
      getClientes({ busca, status: 'ativo' }).then((data) => {
        setClientes(data);
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
    if (!descricao.trim()) {
      setError('Informe a descrição da venda');
      return;
    }

    setLoading(true);
    try {
      await registrarVenda(clienteSelecionado.id, Number(valor), descricao);
      setSuccess(true);
      setTimeout(() => {
        setSuccess(false);
        setClienteSelecionado(null);
        setValor('');
        setDescricao('');
      }, 3000);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const creditoDisponivel = clienteSelecionado
    ? clienteSelecionado.limiteCredito - clienteSelecionado.saldo
    : 0;

  if (success) {
    return (
      <div className="success-screen">
        <div className="success-icon-wrap">
          <CheckCircle size={56} strokeWidth={1.5} />
        </div>
        <h2>Venda registrada!</h2>
        <p>Débito adicionado ao fiado do cliente com sucesso.</p>
      </div>
    );
  }

  return (
    <div className="vendas-page">
      <div className="page-header">
        <div className="header-with-back">
          <button className="btn-back" onClick={() => navigate('/dashboard')}>
            <ArrowLeft size={18} />
          </button>
          <div>
            <h1 className="page-title">Registrar Venda</h1>
            <p className="page-subtitle">Registre uma nova venda no fiado</p>
          </div>
        </div>
      </div>

      <div className="venda-layout">
        {/* Form */}
        <form onSubmit={handleSubmit} className="form-card venda-form">
          {error && (
            <div className="form-error-banner">
              <AlertCircle size={16} />
              <span>{error}</span>
            </div>
          )}

          <div className="form-fields">
            {/* Client Search */}
            <div className="form-group">
              <label><User size={16} /> Cliente</label>
              {clienteSelecionado ? (
                <div className="selected-cliente">
                  <div className="selected-avatar">
                    {clienteSelecionado.nome.charAt(0).toUpperCase()}
                  </div>
                  <div className="selected-info">
                    <span className="selected-name">{clienteSelecionado.nome}</span>
                    <span className="selected-doc">{clienteSelecionado.documento}</span>
                  </div>
                  <button
                    type="button"
                    className="selected-remove"
                    onClick={() => setClienteSelecionado(null)}
                  >
                    <X size={14} />
                  </button>
                </div>
              ) : (
                <div className="client-search-wrap">
                  <Search size={16} className="client-search-icon" />
                  <input
                    id="search-cliente-venda"
                    type="text"
                    placeholder="Buscar cliente por nome ou CPF..."
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
                          <div className="result-avatar">{c.nome.charAt(0)}</div>
                          <div className="result-info">
                            <span className="result-name">{c.nome}</span>
                            <span className="result-doc">{c.documento}</span>
                          </div>
                          <span className="result-saldo">
                            Saldo: {formatCurrency(c.saldo)}
                          </span>
                        </button>
                      ))}
                    </div>
                  )}
                  {showResults && clientes.length === 0 && busca.length >= 2 && (
                    <div className="client-results empty">
                      <p>Nenhum cliente encontrado</p>
                    </div>
                  )}
                </div>
              )}
            </div>

            {/* Valor */}
            <div className="form-group">
              <label><ShoppingCart size={16} /> Valor da Venda (R$)</label>
              <input
                id="valor-venda"
                type="number"
                placeholder="0.00"
                step="0.01"
                min="0.01"
                value={valor}
                onChange={(e) => setValor(e.target.value)}
              />
            </div>

            {/* Descrição */}
            <div className="form-group">
              <label>Descrição</label>
              <textarea
                id="descricao-venda"
                placeholder="Ex: Compras do mês — mantimentos"
                rows={3}
                value={descricao}
                onChange={(e) => setDescricao(e.target.value)}
              />
            </div>
          </div>

          <div className="form-actions">
            <button type="button" className="btn-form-secondary" onClick={() => navigate('/dashboard')}>
              Cancelar
            </button>
            <button
              type="submit"
              className={`btn-form-primary ${loading ? 'is-loading' : ''}`}
              disabled={loading}
            >
              {loading ? (
                <span className="btn-spinner" />
              ) : (
                <>
                  <ShoppingCart size={16} />
                  Registrar Venda
                </>
              )}
            </button>
          </div>
        </form>

        {/* Side Panel — Client Info */}
        {clienteSelecionado && (
          <div className="venda-side-panel">
            <div className="side-card">
              <h3 className="side-title">Resumo do Cliente</h3>
              <div className="side-stat">
                <span className="side-label">Saldo Devedor</span>
                <span className="side-value danger">{formatCurrency(clienteSelecionado.saldo)}</span>
              </div>
              <div className="side-stat">
                <span className="side-label">Limite de Crédito</span>
                <span className="side-value">{formatCurrency(clienteSelecionado.limiteCredito)}</span>
              </div>
              <div className="side-stat">
                <span className="side-label">Crédito Disponível</span>
                <span className={`side-value ${creditoDisponivel <= 0 ? 'danger' : 'safe'}`}>
                  {formatCurrency(creditoDisponivel)}
                </span>
              </div>

              {creditoDisponivel <= 0 && (
                <div className="side-alert">
                  <AlertTriangle size={16} />
                  <span>Cliente sem crédito disponível!</span>
                </div>
              )}

              {valor && Number(valor) > creditoDisponivel && creditoDisponivel > 0 && (
                <div className="side-alert warning">
                  <AlertTriangle size={16} />
                  <span>Valor excede o crédito disponível</span>
                </div>
              )}
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default Vendas;
