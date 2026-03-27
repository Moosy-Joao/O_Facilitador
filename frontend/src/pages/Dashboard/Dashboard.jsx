import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { mockDashboardService } from '../../services/mockData';
import styles from './Dashboard.module.css';
import {
  Wallet,
  Users,
  ShieldOff,
  ShoppingCart,
  AlertTriangle,
  ClipboardList,
  CheckCircle2,
  Inbox,
} from 'lucide-react';

export default function Dashboard() {
  const [summary, setSummary] = useState(null);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    loadSummary();
  }, []);

  const loadSummary = async () => {
    try {
      const data = await mockDashboardService.getSummary();
      setSummary(data);
    } catch (err) {
      console.error('Erro ao carregar dashboard:', err);
    } finally {
      setLoading(false);
    }
  };

  const formatCurrency = (value) => {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL',
    }).format(value);
  };

  const formatDate = (dateStr) => {
    return new Date(dateStr).toLocaleDateString('pt-BR', {
      day: '2-digit',
      month: '2-digit',
      hour: '2-digit',
      minute: '2-digit',
    });
  };

  const getInitials = (name) => {
    return name
      .split(' ')
      .map((n) => n[0])
      .join('')
      .toUpperCase()
      .slice(0, 2);
  };

  if (loading) {
    return (
      <div className={styles.dashboard}>
        <div className={styles['stats-grid']}>
          {[1, 2, 3, 4].map((i) => (
            <div key={i} className={`${styles['stat-card']} ${styles.skeleton} ${styles['skeleton-stat']}`} />
          ))}
        </div>
      </div>
    );
  }

  return (
    <div className={styles.dashboard}>
      <div className={styles['stats-grid']}>
        <div className={`${styles['stat-card']} ${styles.primary}`}>
          <div className={styles['stat-header']}>
            <span className={styles['stat-label']}>Total a Receber</span>
            <div className={styles['stat-icon']}><Wallet size={22} /></div>
          </div>
          <div className={styles['stat-value']}>{formatCurrency(summary?.totalReceivable || 0)}</div>
          <div className={styles['stat-detail']}>Total em aberto de todos os clientes</div>
        </div>

        <div className={`${styles['stat-card']} ${styles.warning}`}>
          <div className={styles['stat-header']}>
            <span className={styles['stat-label']}>Clientes Ativos</span>
            <div className={styles['stat-icon']}><Users size={22} /></div>
          </div>
          <div className={styles['stat-value']}>{summary?.totalClients || 0}</div>
          <div className={styles['stat-detail']}>{summary?.totalDebtors || 0} com saldo devedor</div>
        </div>

        <div className={`${styles['stat-card']} ${styles.danger}`}>
          <div className={styles['stat-header']}>
            <span className={styles['stat-label']}>Bloqueados</span>
            <div className={styles['stat-icon']}><ShieldOff size={22} /></div>
          </div>
          <div className={styles['stat-value']}>{summary?.totalBlocked || 0}</div>
          <div className={styles['stat-detail']}>Clientes com crédito bloqueado</div>
        </div>

        <div className={`${styles['stat-card']} ${styles.success}`}>
          <div className={styles['stat-header']}>
            <span className={styles['stat-label']}>Vendas Hoje</span>
            <div className={styles['stat-icon']}><ShoppingCart size={22} /></div>
          </div>
          <div className={styles['stat-value']}>{formatCurrency(summary?.todayPurchases || 0)}</div>
          <div className={styles['stat-detail']}>Pgtos: {formatCurrency(summary?.todayPayments || 0)}</div>
        </div>
      </div>

      <div className={styles['dashboard-sections']}>
        <div className={styles['dashboard-section']}>
          <div className={styles['section-header']}>
            <h2 className={styles['section-title']}>
              <AlertTriangle size={18} style={{ marginRight: 6, verticalAlign: 'middle', color: 'var(--color-cotton-candy)' }} />
              Maiores Devedores
            </h2>
            <button className={styles['section-action']} onClick={() => navigate('/clientes')}>
              Ver todos →
            </button>
          </div>
          <div className={styles['section-body']}>
            {summary?.recentDebtors?.length > 0 ? (
              summary.recentDebtors.map((client) => (
                <div
                  key={client.id}
                  className={styles['debtor-item']}
                  onClick={() => navigate(`/clientes/${client.id}`)}
                >
                  <div className={styles['debtor-info']}>
                    <div className={styles['debtor-avatar']}>{getInitials(client.name)}</div>
                    <div>
                      <div className={styles['debtor-name']}>{client.name}</div>
                      <div className={styles['debtor-phone']}>{client.phone}</div>
                    </div>
                  </div>
                  <div className={styles['debtor-amount']}>{formatCurrency(client.balance)}</div>
                </div>
              ))
            ) : (
              <div className={styles['empty-state']}>
                <div className={styles['empty-state-icon']}><CheckCircle2 size={40} /></div>
                Nenhum devedor no momento
              </div>
            )}
          </div>
        </div>

        <div className={styles['dashboard-section']}>
          <div className={styles['section-header']}>
            <h2 className={styles['section-title']}>
              <ClipboardList size={18} style={{ marginRight: 6, verticalAlign: 'middle' }} />
              Últimas Movimentações
            </h2>
          </div>
          <div className={styles['section-body']}>
            {summary?.recentTransactions?.length > 0 ? (
              summary.recentTransactions.map((tx) => (
                <div key={tx.id} className={styles['transaction-item']}>
                  <div className={styles['transaction-info']}>
                    <div className={`${styles['transaction-icon']} ${styles[tx.type]}`}>
                      {tx.type === 'purchase' ? <ShoppingCart size={16} /> : <Wallet size={16} />}
                    </div>
                    <div>
                      <div className={styles['transaction-desc']}>
                        {tx.description || tx.observation || 'Transação'}
                      </div>
                      <div className={styles['transaction-date']}>{formatDate(tx.date)}</div>
                    </div>
                  </div>
                  <div className={`${styles['transaction-amount']} ${styles[tx.type]}`}>
                    {tx.type === 'purchase' ? '-' : '+'} {formatCurrency(tx.value)}
                  </div>
                </div>
              ))
            ) : (
              <div className={styles['empty-state']}>
                <div className={styles['empty-state-icon']}><Inbox size={40} /></div>
                Nenhuma movimentação recente
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}
