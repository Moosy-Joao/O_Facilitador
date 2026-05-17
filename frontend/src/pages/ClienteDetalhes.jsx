import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { ArrowLeft, User, Phone, Mail, MapPin, AlertTriangle, Shield, TrendingUp, History, CreditCard } from 'lucide-react';
import { getClienteById, getTransacoes, formatCurrency, formatCPF, formatPhone, formatDateTime } from '../services/api';
import './ClienteDetalhes.css';

const ClienteDetalhes = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [cliente, setCliente] = useState(null);
  const [transacoes, setTransacoes] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);
      try {
        const clienteData = await getClienteById(id);
        if (clienteData) {
          setCliente(clienteData);
          const transacoesData = await getTransacoes({ clienteId: id });
          setTransacoes(transacoesData);
        } else {
          // Cliente não encontrado
          navigate('/clientes');
        }
      } catch (err) {
        console.error('Erro ao buscar detalhes do cliente:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [id, navigate]);

  if (loading) {
    return (
      <div className="page-loading">
        <div className="loading-spinner" />
        <p>Carregando detalhes do cliente...</p>
      </div>
    );
  }

  if (!cliente) return null;

  const getStatusInfo = (cliente) => {
    if (!cliente.ativo) return { label: 'Inativo', className: 'status-inativo' };
    if (cliente.saldo > cliente.limiteCredito) return { label: 'Inadimplente', className: 'status-inadimplente' };
    if (cliente.saldo > 0) return { label: 'Devendo', className: 'status-devendo' };
    return { label: 'Em dia', className: 'status-emdia' };
  };

  const status = getStatusInfo(cliente);
  const limiteDisponivel = Math.max(0, cliente.limiteCredito - cliente.saldo);
  const creditPct = cliente.limiteCredito > 0 ? Math.min((cliente.saldo / cliente.limiteCredito) * 100, 100) : 0;

  return (
    <div className="cliente-detalhes-page">
      <div className="page-header details-header">
        <button className="btn-back" onClick={() => navigate('/clientes')}>
          <ArrowLeft size={20} />
        </button>
        <div className="header-info">
          <h1 className="page-title">{cliente.nome}</h1>
          <span className={`status-badge ${status.className}`}>{status.label}</span>
        </div>
      </div>

      <div className="detalhes-grid">
        {/* Info Column */}
        <div className="detalhes-col-info">
          <div className="card-info">
            <h3>Informações Pessoais</h3>
            <div className="info-list">
              <div className="info-item">
                <User size={16} className="info-icon" />
                <span><strong>CPF/CNPJ:</strong> {formatCPF(cliente.documento)}</span>
              </div>
              <div className="info-item">
                <Mail size={16} className="info-icon" />
                <span><strong>Email:</strong> {cliente.email || 'Não informado'}</span>
              </div>
              <div className="info-item">
                <Phone size={16} className="info-icon" />
                <span><strong>Telefone:</strong> {formatPhone(cliente.telefone) || 'Não informado'}</span>
              </div>
            </div>
          </div>

          {cliente.endereco && (
            <div className="card-info mt-4">
              <h3>Endereço</h3>
              <div className="info-item address-item">
                <MapPin size={16} className="info-icon" />
                <span>
                  {cliente.endereco.rua}, {cliente.endereco.numero}
                  {cliente.endereco.complemento && ` - ${cliente.endereco.complemento}`}
                  <br />
                  {cliente.endereco.bairro}, {cliente.endereco.cidade}/{cliente.endereco.estado}
                  <br />
                  CEP: {cliente.endereco.cep}
                </span>
              </div>
            </div>
          )}

          <div className="quick-actions">
            <button className="btn-action primary" onClick={() => navigate(`/vendas?clienteId=${cliente.id}`)}>
              <TrendingUp size={16} /> Registrar Venda
            </button>
            <button className="btn-action secondary" onClick={() => navigate(`/pagamentos?clienteId=${cliente.id}`)}>
              <CreditCard size={16} /> Receber Pagamento
            </button>
          </div>
        </div>

        {/* Finance Column */}
        <div className="detalhes-col-finance">
          <div className="finance-cards">
            <div className="finance-card danger">
              <div className="card-icon"><AlertTriangle size={20} /></div>
              <div className="card-content">
                <span className="card-label">Saldo Devedor</span>
                <span className="card-value">{formatCurrency(cliente.saldo)}</span>
              </div>
            </div>
            
            <div className="finance-card safe">
              <div className="card-icon"><Shield size={20} /></div>
              <div className="card-content">
                <span className="card-label">Limite de Crédito</span>
                <span className="card-value">{formatCurrency(cliente.limiteCredito)}</span>
              </div>
            </div>

            <div className="finance-card info">
              <div className="card-icon"><CreditCard size={20} /></div>
              <div className="card-content">
                <span className="card-label">Limite Disponível</span>
                <span className="card-value">{formatCurrency(limiteDisponivel)}</span>
              </div>
            </div>
          </div>

          <div className="credit-usage-wrap">
            <div className="credit-usage-header">
              <span>Uso do Limite</span>
              <span>{creditPct.toFixed(1)}%</span>
            </div>
            <div className="credit-bar-bg">
              <div 
                className={`credit-bar-fill ${creditPct > 90 ? 'danger' : creditPct > 70 ? 'warning' : 'ok'}`} 
                style={{ width: `${creditPct}%` }}
              />
            </div>
          </div>

          <div className="card-info mt-4">
            <div className="history-header">
              <h3><History size={18} className="mr-2" /> Histórico Recente</h3>
            </div>
            
            {transacoes.length === 0 ? (
              <p className="no-history">Nenhuma transação encontrada para este cliente.</p>
            ) : (
              <div className="history-list">
                {transacoes.slice(0, 10).map((t, idx) => (
                  <div key={t.id || idx} className="history-item">
                    <div className="history-item-info">
                      <span className={`history-type ${t.tipo}`}>
                        {t.tipo === 'venda' ? 'Venda (Fiado)' : 'Pagamento'}
                      </span>
                      <span className="history-desc">{t.descricao || '-'}</span>
                      <span className="history-date">{formatDateTime(t.data)}</span>
                    </div>
                    <div className={`history-amount ${t.tipo}`}>
                      {t.tipo === 'pagamento' ? '+' : '-'}{formatCurrency(t.valor)}
                    </div>
                  </div>
                ))}
              </div>
            )}
            
            {transacoes.length > 10 && (
              <button 
                className="btn-view-all"
                onClick={() => navigate(`/historico?clienteId=${cliente.id}`)}
              >
                Ver histórico completo
              </button>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default ClienteDetalhes;
