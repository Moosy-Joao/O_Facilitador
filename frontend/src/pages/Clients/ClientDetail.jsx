import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { mockClientService, mockPurchaseService, mockPaymentService } from '../../services/mockData';
import styles from './ClientDetail.module.css';
import formStyles from './ClientForm.module.css';

export default function ClientDetail() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [client, setClient] = useState(null);
  const [transactions, setTransactions] = useState([]);
  const [loading, setLoading] = useState(true);
  const [modal, setModal] = useState(null);
  const [modalForm, setModalForm] = useState({ value: '', description: '', observation: '' });
  const [modalLoading, setModalLoading] = useState(false);
  const [modalError, setModalError] = useState('');

  useEffect(() => {
    loadData();
  }, [id]);

  const loadData = async () => {
    try {
      const [clientData, purchases, payments] = await Promise.all([
        mockClientService.getById(id),
        mockPurchaseService.getByClient(id),
        mockPaymentService.getByClient(id),
      ]);
      setClient(clientData);
      const allTx = [...purchases, ...payments].sort(
        (a, b) => new Date(b.date) - new Date(a.date)
      );
      setTransactions(allTx);
    } catch (err) {
      console.error('Erro ao carregar detalhes:', err);
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
      year: 'numeric',
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

  const handleToggleActive = async () => {
    try {
      const updated = await mockClientService.toggleActive(id);
      setClient(updated);
    } catch (err) {
      console.error(err);
    }
  };

  const openModal = (type) => {
    setModal(type);
    setModalForm({ value: '', description: '', observation: '' });
    setModalError('');
  };

  const closeModal = () => {
    setModal(null);
    setModalForm({ value: '', description: '', observation: '' });
    setModalError('');
  };

  const handleModalSubmit = async (e) => {
    e.preventDefault();
    if (!modalForm.value || Number(modalForm.value) <= 0) {
      setModalError('Informe um valor válido');
      return;
    }

    setModalLoading(true);
    setModalError('');
    try {
      if (modal === 'purchase') {
        await mockPurchaseService.create(id, {
          value: Number(modalForm.value),
          description: modalForm.description,
        });
      } else {
        await mockPaymentService.create(id, {
          value: Number(modalForm.value),
          observation: modalForm.observation,
        });
      }
      closeModal();
      await loadData();
    } catch (err) {
      setModalError(err.message);
    } finally {
      setModalLoading(false);
    }
  };

  const handleReverse = async (tx) => {
    if (!confirm('Tem certeza que deseja estornar esta transação?')) return;
    try {
      if (tx.type === 'purchase') {
        await mockPurchaseService.reverse(id, tx.id);
      } else {
        await mockPaymentService.reverse(id, tx.id);
      }
      await loadData();
    } catch (err) {
      console.error(err);
    }
  };

  if (loading) {
    return <div className={styles['client-detail']}>Carregando...</div>;
  }

  if (!client) {
    return <div className={styles['client-detail']}>Cliente não encontrado</div>;
  }

  const available = Math.max(0, client.creditLimit - client.balance);

  return (
    <div className={styles['client-detail']}>
      <button className={styles['back-link']} onClick={() => navigate('/clientes')}>
        ← Voltar para clientes
      </button>

      <div className={styles['profile-card']}>
        <div className={styles['profile-header']}>
          <div className={styles['profile-info']}>
            <div className={`${styles['profile-avatar']} ${client.blocked ? styles.blocked : ''}`}>
              {getInitials(client.name)}
            </div>
            <div>
              <h2 className={styles['profile-name']}>{client.name}</h2>
              <div className={styles['profile-meta']}>
                <span className={styles['profile-meta-item']}>📄 {client.cnpj}</span>
                <span className={styles['profile-meta-item']}>📞 {client.phone}</span>
                {client.address && (
                  <span className={styles['profile-meta-item']}>📍 {client.address}</span>
                )}
              </div>
            </div>
          </div>
          <div className={styles['profile-actions']}>
            <button
              className={styles['btn-sm']}
              onClick={() => navigate(`/clientes/${id}/editar`)}
            >
              ✏️ Editar
            </button>
            <button
              className={`${styles['btn-sm']} ${client.active ? styles.danger : styles.success}`}
              onClick={handleToggleActive}
            >
              {client.active ? '🚫 Inativar' : '✅ Ativar'}
            </button>
          </div>
        </div>

        <div className={styles['finance-grid']}>
          <div className={styles['finance-item']}>
            <div className={styles['finance-label']}>Saldo Devedor</div>
            <div className={`${styles['finance-value']} ${client.balance > 0 ? styles.debt : ''}`}>
              {formatCurrency(client.balance)}
            </div>
          </div>
          <div className={styles['finance-item']}>
            <div className={styles['finance-label']}>Limite de Crédito</div>
            <div className={styles['finance-value']}>{formatCurrency(client.creditLimit)}</div>
          </div>
          <div className={styles['finance-item']}>
            <div className={styles['finance-label']}>Disponível</div>
            <div className={`${styles['finance-value']} ${styles.available}`}>
              {formatCurrency(available)}
            </div>
          </div>
          <div className={styles['finance-item']}>
            <div className={styles['finance-label']}>Status</div>
            <div className={styles['finance-value']}>
              {!client.active ? '⚫ Inativo' : client.blocked ? '🔴 Bloqueado' : '🟢 Ativo'}
            </div>
          </div>
        </div>
      </div>

      <div className={styles['transaction-section']}>
        <div className={styles['transaction-header']}>
          <h3 className={styles['transaction-title']}>📋 Extrato de Transações</h3>
          <div style={{ display: 'flex', gap: '8px' }}>
            <button
              className={`${styles['btn-action']} ${styles['btn-purchase']}`}
              onClick={() => openModal('purchase')}
              disabled={client.blocked || !client.active}
            >
              🛒 Nova Venda
            </button>
            <button
              className={`${styles['btn-action']} ${styles['btn-payment']}`}
              onClick={() => openModal('payment')}
              disabled={!client.active}
            >
              💵 Registrar Pagamento
            </button>
          </div>
        </div>

        {transactions.length > 0 ? (
          <table className={styles['transaction-table']}>
            <thead>
              <tr>
                <th>Tipo</th>
                <th>Descrição</th>
                <th>Data</th>
                <th>Valor</th>
                <th>Ação</th>
              </tr>
            </thead>
            <tbody>
              {transactions.map((tx) => (
                <tr key={tx.id} className={tx.reversed ? styles['tx-reversed'] : ''}>
                  <td>
                    <span className={`${styles['tx-type']} ${styles[tx.type]}`}>
                      {tx.type === 'purchase' ? '🛒 Compra' : '💵 Pagamento'}
                    </span>
                  </td>
                  <td>
                    {tx.description || tx.observation || '—'}
                    {tx.reversed && <span className={styles['tx-badge']}>Estornado</span>}
                  </td>
                  <td>{formatDate(tx.date)}</td>
                  <td style={{ fontWeight: 700, color: tx.type === 'purchase' ? 'var(--color-cotton-candy)' : 'var(--color-success)' }}>
                    {tx.type === 'purchase' ? '-' : '+'} {formatCurrency(tx.value)}
                  </td>
                  <td>
                    {!tx.reversed && (
                      <button
                        className={styles['btn-reverse']}
                        onClick={() => handleReverse(tx)}
                      >
                        Estornar
                      </button>
                    )}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        ) : (
          <div className={styles['empty-state']}>
            <div className={styles['empty-state-icon']}>📭</div>
            Nenhuma transação registrada para este cliente
          </div>
        )}
      </div>

      {/* Modal */}
      {modal && (
        <div className={styles['modal-overlay']} onClick={closeModal}>
          <div className={styles.modal} onClick={(e) => e.stopPropagation()}>
            <div className={styles['modal-header']}>
              <h3 className={styles['modal-title']}>
                {modal === 'purchase' ? '🛒 Registrar Venda' : '💵 Registrar Pagamento'}
              </h3>
              <button className={styles['modal-close']} onClick={closeModal}>
                ✕
              </button>
            </div>
            <form onSubmit={handleModalSubmit}>
              <div className={styles['modal-body']}>
                {modalError && (
                  <div className={formStyles['form-error-banner']}>{modalError}</div>
                )}
                <div className={formStyles['form-group']} style={{ marginBottom: '16px' }}>
                  <label className={formStyles['form-label']} htmlFor="modal-value">
                    Valor (R$) <span className={formStyles.required}>*</span>
                  </label>
                  <input
                    id="modal-value"
                    className={formStyles['form-input']}
                    type="number"
                    step="0.01"
                    min="0.01"
                    placeholder="0.00"
                    value={modalForm.value}
                    onChange={(e) => setModalForm((p) => ({ ...p, value: e.target.value }))}
                    autoFocus
                  />
                </div>
                <div className={formStyles['form-group']}>
                  <label className={formStyles['form-label']} htmlFor="modal-desc">
                    {modal === 'purchase' ? 'Descrição' : 'Observação'}
                  </label>
                  <input
                    id="modal-desc"
                    className={formStyles['form-input']}
                    type="text"
                    placeholder={modal === 'purchase' ? 'Ex: Compras do mês' : 'Ex: Pagamento em dinheiro'}
                    value={modal === 'purchase' ? modalForm.description : modalForm.observation}
                    onChange={(e) =>
                      setModalForm((p) => ({
                        ...p,
                        [modal === 'purchase' ? 'description' : 'observation']: e.target.value,
                      }))
                    }
                  />
                </div>
              </div>
              <div className={styles['modal-footer']}>
                <button
                  type="button"
                  className={`${formStyles.btn} ${formStyles['btn-cancel']}`}
                  onClick={closeModal}
                >
                  Cancelar
                </button>
                <button
                  type="submit"
                  className={`${formStyles.btn} ${formStyles['btn-save']}`}
                  disabled={modalLoading}
                >
                  {modalLoading ? 'Salvando...' : 'Confirmar'}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
