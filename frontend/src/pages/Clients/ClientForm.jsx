import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { mockClientService } from '../../services/mockData';
import styles from './ClientForm.module.css';
import { Pencil, UserRound } from 'lucide-react';

export default function ClientForm() {
  const { id } = useParams();
  const isEditing = Boolean(id);
  const navigate = useNavigate();

  const [form, setForm] = useState({
    name: '',
    cnpj: '',
    phone: '',
    address: '',
    creditLimit: '',
  });
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const [success, setSuccess] = useState('');
  const [apiError, setApiError] = useState('');

  useEffect(() => {
    if (isEditing) {
      loadClient();
    }
  }, [id]);

  const loadClient = async () => {
    try {
      const client = await mockClientService.getById(id);
      setForm({
        name: client.name,
        cnpj: client.cnpj,
        phone: client.phone,
        address: client.address,
        creditLimit: String(client.creditLimit),
      });
    } catch (err) {
      setApiError('Erro ao carregar cliente');
    }
  };

  const handleChange = (field, value) => {
    setForm((prev) => ({ ...prev, [field]: value }));
    if (errors[field]) {
      setErrors((prev) => ({ ...prev, [field]: '' }));
    }
  };

  const validate = () => {
    const newErrors = {};
    if (!form.name.trim()) newErrors.name = 'Nome é obrigatório';
    if (!form.cnpj.trim()) newErrors.cnpj = 'CNPJ é obrigatório';
    if (!form.phone.trim()) newErrors.phone = 'Telefone é obrigatório';
    if (!form.creditLimit || Number(form.creditLimit) <= 0) {
      newErrors.creditLimit = 'Limite deve ser maior que zero';
    }
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!validate()) return;

    setLoading(true);
    setApiError('');
    try {
      const data = {
        ...form,
        creditLimit: Number(form.creditLimit),
      };

      if (isEditing) {
        await mockClientService.update(id, data);
        setSuccess('Cliente atualizado com sucesso!');
      } else {
        await mockClientService.create(data);
        setSuccess('Cliente cadastrado com sucesso!');
      }
      setTimeout(() => navigate('/clientes'), 1200);
    } catch (err) {
      setApiError(err.message || 'Erro ao salvar cliente');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className={styles['client-form-page']}>
      <div className={styles['form-card']}>
        <div className={styles['form-card-header']}>
          <div className={styles['form-card-icon']}>
            {isEditing ? <Pencil size={22} /> : <UserRound size={22} />}
          </div>
          <div>
            <div className={styles['form-card-title']}>
              {isEditing ? 'Editar Cliente' : 'Novo Cliente'}
            </div>
            <div className={styles['form-card-subtitle']}>
              {isEditing
                ? 'Atualize as informações do cliente'
                : 'Preencha os dados para cadastrar um novo cliente'}
            </div>
          </div>
        </div>

        <form onSubmit={handleSubmit}>
          <div className={styles['form-card-body']}>
            {success && <div className={styles['form-success']}>{success}</div>}
            {apiError && <div className={styles['form-error-banner']}>{apiError}</div>}

            <div className={styles['form-grid']}>
              <div className={`${styles['form-group']} ${styles['full-width']}`}>
                <label className={styles['form-label']} htmlFor="client-name">
                  Nome completo <span className={styles.required}>*</span>
                </label>
                <input
                  id="client-name"
                  className={`${styles['form-input']} ${errors.name ? styles.error : ''}`}
                  type="text"
                  placeholder="Ex: Maria da Silva"
                  value={form.name}
                  onChange={(e) => handleChange('name', e.target.value)}
                />
                {errors.name && <span className={styles['form-error']}>{errors.name}</span>}
              </div>

              <div className={styles['form-group']}>
                <label className={styles['form-label']} htmlFor="client-cnpj">
                  CNPJ <span className={styles.required}>*</span>
                </label>
                <input
                  id="client-cnpj"
                  className={`${styles['form-input']} ${errors.cnpj ? styles.error : ''}`}
                  type="text"
                  placeholder="00.000.000/0001-00"
                  value={form.cnpj}
                  onChange={(e) => handleChange('cnpj', e.target.value)}
                />
                {errors.cnpj && <span className={styles['form-error']}>{errors.cnpj}</span>}
              </div>

              <div className={styles['form-group']}>
                <label className={styles['form-label']} htmlFor="client-phone">
                  Telefone <span className={styles.required}>*</span>
                </label>
                <input
                  id="client-phone"
                  className={`${styles['form-input']} ${errors.phone ? styles.error : ''}`}
                  type="text"
                  placeholder="(44) 99999-0000"
                  value={form.phone}
                  onChange={(e) => handleChange('phone', e.target.value)}
                />
                {errors.phone && <span className={styles['form-error']}>{errors.phone}</span>}
              </div>

              <div className={`${styles['form-group']} ${styles['full-width']}`}>
                <label className={styles['form-label']} htmlFor="client-address">
                  Endereço
                </label>
                <input
                  id="client-address"
                  className={styles['form-input']}
                  type="text"
                  placeholder="Rua, número - Bairro, Cidade/UF"
                  value={form.address}
                  onChange={(e) => handleChange('address', e.target.value)}
                />
              </div>

              <div className={styles['form-group']}>
                <label className={styles['form-label']} htmlFor="client-limit">
                  Limite de Crédito (R$) <span className={styles.required}>*</span>
                </label>
                <input
                  id="client-limit"
                  className={`${styles['form-input']} ${errors.creditLimit ? styles.error : ''}`}
                  type="number"
                  step="0.01"
                  min="0"
                  placeholder="500.00"
                  value={form.creditLimit}
                  onChange={(e) => handleChange('creditLimit', e.target.value)}
                />
                {errors.creditLimit && (
                  <span className={styles['form-error']}>{errors.creditLimit}</span>
                )}
                <span className={styles['form-hint']}>
                  Valor máximo que o cliente pode comprar no fiado
                </span>
              </div>
            </div>
          </div>

          <div className={styles['form-card-footer']}>
            <button
              type="button"
              className={`${styles.btn} ${styles['btn-cancel']}`}
              onClick={() => navigate('/clientes')}
            >
              Cancelar
            </button>
            <button
              type="submit"
              className={`${styles.btn} ${styles['btn-save']}`}
              disabled={loading}
            >
              {loading
                ? 'Salvando...'
                : isEditing
                ? 'Salvar Alterações'
                : 'Cadastrar Cliente'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
