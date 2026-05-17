import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Search,
  UserPlus,
  Filter,
  MoreHorizontal,
  ChevronDown,
  Eye,
  Edit3,
  UserX,
  UserCheck,
  AlertTriangle,
  Shield,
  X,
} from 'lucide-react';
import { getClientes, toggleClienteStatus, formatCurrency } from '../services/api';
import './Clientes.css';

const Clientes = () => {
  const [clientes, setClientes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [busca, setBusca] = useState('');
  const [filtroStatus, setFiltroStatus] = useState('todos');
  const [showFilterMenu, setShowFilterMenu] = useState(false);
  const [actionMenuId, setActionMenuId] = useState(null);
  const navigate = useNavigate();

  const fetchClientes = async () => {
    try {
      const data = await getClientes({
        busca,
        status: filtroStatus === 'todos' ? undefined : filtroStatus,
      });
      setClientes(data);
    } catch (err) {
      console.error('Erro ao buscar clientes:', err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    setLoading(true);
    const timer = setTimeout(() => fetchClientes(), 300);
    return () => clearTimeout(timer);
  }, [busca, filtroStatus]);

  const handleToggleStatus = async (id) => {
    try {
      await toggleClienteStatus(id);
      await fetchClientes();
      setActionMenuId(null);
    } catch (err) {
      alert(err.message);
    }
  };

  const getStatusInfo = (cliente) => {
    if (!cliente.ativo) return { label: 'Inativo', className: 'status-inativo' };
    if (cliente.saldo > cliente.limiteCredito) return { label: 'Inadimplente', className: 'status-inadimplente' };
    if (cliente.saldo > 0) return { label: 'Devendo', className: 'status-devendo' };
    return { label: 'Em dia', className: 'status-emdia' };
  };

  const getCreditPercent = (cliente) => {
    if (cliente.limiteCredito === 0) return 0;
    return Math.min((cliente.saldo / cliente.limiteCredito) * 100, 100);
  };

  const filterLabels = {
    todos: 'Todos',
    ativo: 'Ativos',
    inativo: 'Inativos',
    inadimplente: 'Inadimplentes',
  };

  if (loading) {
    return (
      <div className="page-loading">
        <div className="loading-spinner" />
        <p>Carregando clientes...</p>
      </div>
    );
  }

  return (
    <div className="clientes-page">
      {/* Header */}
      <div className="page-header">
        <div>
          <h1 className="page-title">Clientes</h1>
          <p className="page-subtitle">
            {clientes.length} cliente{clientes.length !== 1 ? 's' : ''} encontrado{clientes.length !== 1 ? 's' : ''}
          </p>
        </div>
        <button className="btn-primary-action" onClick={() => navigate('/clientes/novo')}>
          <UserPlus size={18} />
          Novo Cliente
        </button>
      </div>

      {/* Filters */}
      <div className="filter-bar">
        <div className="search-input-wrap">
          <Search size={18} className="search-icon" />
          <input
            id="search-clientes"
            type="text"
            placeholder="Buscar por nome, CPF ou email..."
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
            {filterLabels[filtroStatus]}
            <ChevronDown size={14} />
          </button>
          {showFilterMenu && (
            <div className="filter-dropdown-menu">
              {Object.entries(filterLabels).map(([key, label]) => (
                <button
                  key={key}
                  className={`filter-option ${filtroStatus === key ? 'active' : ''}`}
                  onClick={() => { setFiltroStatus(key); setShowFilterMenu(false); }}
                >
                  {label}
                </button>
              ))}
            </div>
          )}
        </div>
      </div>

      {/* Table */}
      {clientes.length === 0 ? (
        <div className="empty-state">
          <Search size={48} strokeWidth={1} />
          <h3>Nenhum cliente encontrado</h3>
          <p>Tente ajustar os filtros ou cadastre um novo cliente.</p>
        </div>
      ) : (
        <div className="clientes-table-wrap">
          <table className="clientes-table" id="clientes-table">
            <thead>
              <tr>
                <th>Cliente</th>
                <th>Documento</th>
                <th>Saldo Devedor</th>
                <th>Crédito Utilizado</th>
                <th>Status</th>
                <th className="th-actions">Ações</th>
              </tr>
            </thead>
            <tbody>
              {clientes.map((cliente, i) => {
                const status = getStatusInfo(cliente);
                const creditPct = getCreditPercent(cliente);
                return (
                  <tr
                    key={cliente.id}
                    className="table-row"
                    style={{ animationDelay: `${i * 0.04}s` }}
                  >
                    <td>
                      <div className="cliente-cell">
                        <div className="cliente-avatar">
                          {cliente.nome.charAt(0).toUpperCase()}
                        </div>
                        <div className="cliente-info">
                          <span className="cliente-nome">{cliente.nome}</span>
                          <span className="cliente-email">{cliente.email}</span>
                        </div>
                      </div>
                    </td>
                    <td>
                      <span className="doc-text">{cliente.documento}</span>
                    </td>
                    <td>
                      <span className={`saldo-text ${cliente.saldo > 0 ? 'has-saldo' : 'no-saldo'}`}>
                        {formatCurrency(cliente.saldo)}
                      </span>
                    </td>
                    <td>
                      <div className="credit-cell">
                        <div className="credit-bar-bg">
                          <div
                            className={`credit-bar-fill ${creditPct > 90 ? 'danger' : creditPct > 60 ? 'warning' : 'ok'}`}
                            style={{ width: `${creditPct}%` }}
                          />
                        </div>
                        <span className="credit-label">
                          {formatCurrency(cliente.saldo)} / {formatCurrency(cliente.limiteCredito)}
                        </span>
                      </div>
                    </td>
                    <td>
                      <span className={`status-badge ${status.className}`}>
                        {status.label}
                      </span>
                    </td>
                    <td className="td-actions">
                      <div className="actions-wrap">
                        <button
                          className="action-btn"
                          title="Ver detalhes"
                          onClick={() => navigate(`/clientes/${cliente.id}`)}
                        >
                          <Eye size={16} />
                        </button>
                        <button
                          className="action-btn"
                          title="Mais ações"
                          onClick={() => setActionMenuId(actionMenuId === cliente.id ? null : cliente.id)}
                        >
                          <MoreHorizontal size={16} />
                        </button>
                        {actionMenuId === cliente.id && (
                          <div className="action-menu">
                            <button onClick={() => { navigate(`/clientes/novo?edit=${cliente.id}`); }}>
                              <Edit3 size={14} /> Editar
                            </button>
                            <button onClick={() => handleToggleStatus(cliente.id)}>
                              {cliente.ativo ? (
                                <><UserX size={14} /> Desativar</>
                              ) : (
                                <><UserCheck size={14} /> Ativar</>
                              )}
                            </button>
                          </div>
                        )}
                      </div>
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>
      )}

      )}
    </div>
  );
};

export default Clientes;
