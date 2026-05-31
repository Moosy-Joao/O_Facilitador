import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
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
  X,
  RefreshCw,
} from 'lucide-react';
import { getClientes, toggleClienteStatus, formatCurrency, sincronizarSaldos } from '../services/api';
import './Clientes.css';

const Clientes = () => {
  const queryClient = useQueryClient();
  const [buscaInput, setBuscaInput] = useState('');
  const [buscaDebounced, setBuscaDebounced] = useState('');
  const [filtroStatus, setFiltroStatus] = useState('todos');
  const [showFilterMenu, setShowFilterMenu] = useState(false);
  const [actionMenuId, setActionMenuId] = useState(null);
  const navigate = useNavigate();

  // Debounce buscaInput para buscaDebounced
  useEffect(() => {
    const timer = setTimeout(() => {
      setBuscaDebounced(buscaInput);
    }, 300);
    return () => clearTimeout(timer);
  }, [buscaInput]);

  // Query para buscar clientes
  const { data: clientes = [], isLoading } = useQuery({
    queryKey: ['clientes', buscaDebounced, filtroStatus],
    queryFn: () => getClientes({
      busca: buscaDebounced,
      status: filtroStatus === 'todos' ? undefined : filtroStatus,
    }),
  });

  // Mutation para toggle status de ativo/inativo
  const toggleStatusMutation = useMutation({
    mutationFn: toggleClienteStatus,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['clientes'] });
      setActionMenuId(null);
    },
    onError: (err) => {
      alert(err.message);
    }
  });

  // Mutation para sincronizar saldos dos clientes
  const sincronizarSaldosMutation = useMutation({
    mutationFn: sincronizarSaldos,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['clientes'] });
    },
    onError: (err) => {
      alert(err.message);
    }
  });

  const handleToggleStatus = (id) => {
    toggleStatusMutation.mutate(id);
  };

  const handleSincronizarSaldos = () => {
    sincronizarSaldosMutation.mutate();
  };

  const handleClear = () => {
    setBuscaInput('');
    setBuscaDebounced('');
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
        <div className="header-actions" style={{ display: 'flex', gap: '12px' }}>
          <button 
            className="btn-secondary-action" 
            onClick={handleSincronizarSaldos}
            disabled={sincronizarSaldosMutation.isPending}
          >
            <RefreshCw size={16} className={sincronizarSaldosMutation.isPending ? 'spin' : ''} />
            {sincronizarSaldosMutation.isPending ? 'Sincronizando...' : 'Sincronizar Saldos'}
          </button>
          <button className="btn-primary-action" onClick={() => navigate('/clientes/novo')}>
            <UserPlus size={18} />
            Novo Cliente
          </button>
        </div>
      </div>

      {/* Filters */}
      <div className="filter-bar">
        <div className="search-input-wrap">
          <Search size={18} className="search-icon" />
          <input
            id="search-clientes"
            type="text"
            placeholder="Buscar por nome, CPF ou email..."
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
      {clientes.length === 0 && !isLoading ? (
        <div className="empty-state">
          <Search size={48} strokeWidth={1} />
          <h3>Nenhum cliente encontrado</h3>
          <p>Tente ajustar os filtros ou cadastre um novo cliente.</p>
        </div>
      ) : (
        <div className="clientes-table-wrap" style={{ opacity: isLoading ? 0.6 : 1, transition: 'opacity 0.2s' }}>
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
                          <div className={`action-menu ${i === clientes.length - 1 ? 'open-up' : ''}`}>
                            <button onClick={() => { navigate(`/clientes/editar/${cliente.id}`); }}>
                              <Edit3 size={14} /> Editar
                            </button>
                            <button onClick={() => handleToggleStatus(cliente.id)} disabled={toggleStatusMutation.isPending}>
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
    </div>
  );
};

export default Clientes;
