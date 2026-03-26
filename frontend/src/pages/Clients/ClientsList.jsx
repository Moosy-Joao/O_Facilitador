import { useState, useEffect, useMemo } from 'react';
import { useNavigate } from 'react-router-dom';
import { mockClientService } from '../../services/mockData';
import styles from './ClientsList.module.css';

export default function ClientsList() {
  const [clients, setClients] = useState([]);
  const [search, setSearch] = useState('');
  const [filter, setFilter] = useState('all');
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    loadClients();
  }, []);

  const loadClients = async () => {
    try {
      const data = await mockClientService.getAll();
      setClients(data);
    } catch (err) {
      console.error('Erro ao carregar clientes:', err);
    } finally {
      setLoading(false);
    }
  };

  const filteredClients = useMemo(() => {
    let result = clients;

    if (search) {
      const s = search.toLowerCase();
      result = result.filter(
        (c) =>
          c.name.toLowerCase().includes(s) ||
          c.cnpj.includes(s) ||
          c.phone.includes(s)
      );
    }

    switch (filter) {
      case 'debtors':
        result = result.filter((c) => c.balance > 0 && c.active);
        break;
      case 'blocked':
        result = result.filter((c) => c.blocked);
        break;
      case 'inactive':
        result = result.filter((c) => !c.active);
        break;
      case 'clear':
        result = result.filter((c) => c.balance === 0 && c.active);
        break;
      default:
        break;
    }

    return result;
  }, [clients, search, filter]);

  const formatCurrency = (value) => {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL',
    }).format(value);
  };

  const getInitials = (name) => {
    return name
      .split(' ')
      .map((n) => n[0])
      .join('')
      .toUpperCase()
      .slice(0, 2);
  };

  const getCreditPercent = (balance, limit) => {
    if (!limit) return 0;
    return Math.min(100, Math.round((balance / limit) * 100));
  };

  const getCreditBarClass = (percent) => {
    if (percent >= 100) return styles.full;
    if (percent >= 75) return styles.high;
    if (percent >= 50) return styles.medium;
    return styles.low;
  };

  const getStatusInfo = (client) => {
    if (!client.active) return { label: 'Inativo', className: styles.inactive };
    if (client.blocked) return { label: 'Bloqueado', className: styles.blocked };
    return { label: 'Ativo', className: styles.active };
  };

  return (
    <div className={styles['clients-page']}>
      <div className={styles['clients-toolbar']}>
        <div className={styles['search-box']}>
          <span className={styles['search-icon']}>🔍</span>
          <input
            id="client-search"
            className={styles['search-input']}
            type="text"
            placeholder="Buscar por nome, CPF ou telefone..."
            value={search}
            onChange={(e) => setSearch(e.target.value)}
          />
        </div>
        <div className={styles['toolbar-actions']}>
          {[
            { key: 'all', label: 'Todos' },
            { key: 'debtors', label: 'Devedores' },
            { key: 'blocked', label: 'Bloqueados' },
            { key: 'clear', label: 'Em dia' },
          ].map((f) => (
            <button
              key={f.key}
              className={`${styles['filter-btn']} ${filter === f.key ? styles.active : ''}`}
              onClick={() => setFilter(f.key)}
            >
              {f.label}
            </button>
          ))}
          <button
            className={styles['btn-primary']}
            onClick={() => navigate('/clientes/novo')}
          >
            ➕ Novo Cliente
          </button>
        </div>
      </div>

      <div className={styles['clients-count']}>
        {filteredClients.length} cliente{filteredClients.length !== 1 ? 's' : ''} encontrado{filteredClients.length !== 1 ? 's' : ''}
      </div>

      {filteredClients.length > 0 ? (
        <div className={styles['clients-grid']}>
          {filteredClients.map((client) => {
            const status = getStatusInfo(client);
            const creditPercent = getCreditPercent(client.balance, client.creditLimit);

            return (
              <div
                key={client.id}
                className={`${styles['client-card']} ${!client.active ? styles.inactive : ''}`}
                onClick={() => navigate(`/clientes/${client.id}`)}
              >
                <div className={styles['client-card-header']}>
                  <div className={`${styles['client-avatar']} ${client.blocked ? styles.blocked : ''}`}>
                    {getInitials(client.name)}
                  </div>
                  <div className={styles['client-header-info']}>
                    <div className={styles['client-name']}>{client.name}</div>
                    <div className={styles['client-cpf']}>{client.cnpj}</div>
                  </div>
                  <span className={`${styles['client-status']} ${status.className}`}>
                    {status.label}
                  </span>
                </div>

                <div className={styles['client-card-body']}>
                  <div className={styles['client-details']}>
                    <div className={styles['client-detail-item']}>
                      <span className={styles['client-detail-label']}>Saldo Devedor</span>
                      <span
                        className={`${styles['client-detail-value']} ${
                          client.balance > 0 ? styles.debt : styles.clear
                        }`}
                      >
                        {formatCurrency(client.balance)}
                      </span>
                    </div>
                    <div className={styles['client-detail-item']}>
                      <span className={styles['client-detail-label']}>Limite</span>
                      <span className={styles['client-detail-value']}>
                        {formatCurrency(client.creditLimit)}
                      </span>
                    </div>
                    <div className={styles['client-detail-item']}>
                      <span className={styles['client-detail-label']}>Telefone</span>
                      <span className={styles['client-detail-value']}>{client.phone}</span>
                    </div>
                    <div className={styles['client-detail-item']}>
                      <span className={styles['client-detail-label']}>Disponível</span>
                      <span className={`${styles['client-detail-value']} ${styles.clear}`}>
                        {formatCurrency(Math.max(0, client.creditLimit - client.balance))}
                      </span>
                    </div>
                  </div>

                  <div className={styles['credit-bar-container']}>
                    <div className={styles['credit-bar-header']}>
                      <span className={styles['credit-bar-label']}>Uso do crédito</span>
                      <span className={styles['credit-bar-percent']}>{creditPercent}%</span>
                    </div>
                    <div className={styles['credit-bar']}>
                      <div
                        className={`${styles['credit-bar-fill']} ${getCreditBarClass(creditPercent)}`}
                        style={{ width: `${creditPercent}%` }}
                      />
                    </div>
                  </div>
                </div>
              </div>
            );
          })}
        </div>
      ) : (
        <div className={styles['empty-state']}>
          <div className={styles['empty-state-icon']}>👥</div>
          <div className={styles['empty-state-title']}>
            {search || filter !== 'all' ? 'Nenhum cliente encontrado' : 'Nenhum cliente cadastrado'}
          </div>
          <div className={styles['empty-state-text']}>
            {search || filter !== 'all'
              ? 'Tente alterar os filtros ou termo de busca'
              : 'Comece cadastrando seu primeiro cliente para controlar o fiado'}
          </div>
        </div>
      )}
    </div>
  );
}
