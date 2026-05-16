import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  ArrowLeft,
  User,
  Mail,
  Phone,
  FileText,
  MapPin,
  CreditCard,
  CheckCircle,
  AlertCircle,
} from 'lucide-react';
import { criarCliente, formatCPF, formatPhone, formatCEP } from '../services/api';
import './ClienteNovo.css';

const ClienteNovo = () => {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [success, setSuccess] = useState(false);
  const [error, setError] = useState('');
  const [step, setStep] = useState(1);

  const [form, setForm] = useState({
    nome: '',
    email: '',
    documento: '',
    telefone: '',
    limiteCredito: '',
    // Endereço
    cep: '',
    rua: '',
    numero: '',
    bairro: '',
    cidade: '',
    estado: '',
    pais: 'Brasil',
  });

  const [errors, setErrors] = useState({});

  const handleChange = (e) => {
    const { name, value } = e.target;
    let formatted = value;

    if (name === 'documento') formatted = formatCPF(value);
    if (name === 'telefone') formatted = formatPhone(value);
    if (name === 'cep') formatted = formatCEP(value);

    setForm((prev) => ({ ...prev, [name]: formatted }));
    // Clear field error on change
    if (errors[name]) {
      setErrors((prev) => ({ ...prev, [name]: '' }));
    }
  };

  const validateStep1 = () => {
    const newErrors = {};
    if (!form.nome.trim()) newErrors.nome = 'Nome é obrigatório';
    if (!form.email.trim()) newErrors.email = 'Email é obrigatório';
    else if (!/\S+@\S+\.\S+/.test(form.email)) newErrors.email = 'Email inválido';
    if (!form.documento.trim()) newErrors.documento = 'CPF é obrigatório';
    else if (form.documento.replace(/\D/g, '').length < 11) newErrors.documento = 'CPF incompleto';
    if (!form.telefone.trim()) newErrors.telefone = 'Telefone é obrigatório';
    if (!form.limiteCredito) newErrors.limiteCredito = 'Limite de crédito é obrigatório';
    else if (Number(form.limiteCredito) <= 0) newErrors.limiteCredito = 'Deve ser maior que zero';
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const validateStep2 = () => {
    const newErrors = {};
    if (!form.cep.trim()) newErrors.cep = 'CEP é obrigatório';
    if (!form.rua.trim()) newErrors.rua = 'Rua é obrigatória';
    if (!form.numero.trim()) newErrors.numero = 'Número é obrigatório';
    if (!form.bairro.trim()) newErrors.bairro = 'Bairro é obrigatório';
    if (!form.cidade.trim()) newErrors.cidade = 'Cidade é obrigatória';
    if (!form.estado.trim()) newErrors.estado = 'Estado é obrigatório';
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleNextStep = () => {
    if (step === 1 && validateStep1()) {
      setStep(2);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!validateStep2()) return;

    setLoading(true);
    setError('');

    try {
      await criarCliente({
        ...form,
        limiteCredito: Number(form.limiteCredito),
        saldo: 0,
      });
      setSuccess(true);
      setTimeout(() => navigate('/clientes'), 2000);
    } catch (err) {
      setError(err.message || 'Erro ao cadastrar cliente');
    } finally {
      setLoading(false);
    }
  };

  if (success) {
    return (
      <div className="success-screen">
        <div className="success-icon-wrap">
          <CheckCircle size={56} strokeWidth={1.5} />
        </div>
        <h2>Cliente cadastrado!</h2>
        <p>Redirecionando para a lista de clientes...</p>
      </div>
    );
  }

  return (
    <div className="cliente-novo-page">
      {/* Header */}
      <div className="page-header">
        <div className="header-with-back">
          <button className="btn-back" onClick={() => step === 1 ? navigate('/clientes') : setStep(1)}>
            <ArrowLeft size={18} />
          </button>
          <div>
            <h1 className="page-title">Novo Cliente</h1>
            <p className="page-subtitle">
              {step === 1 ? 'Informações pessoais e financeiras' : 'Endereço do cliente'}
            </p>
          </div>
        </div>
      </div>

      {/* Step Indicator */}
      <div className="step-indicator">
        <div className={`step-dot ${step >= 1 ? 'active' : ''}`}>
          <span>1</span>
          <label>Dados Pessoais</label>
        </div>
        <div className="step-line-wrap">
          <div className={`step-line ${step >= 2 ? 'filled' : ''}`} />
        </div>
        <div className={`step-dot ${step >= 2 ? 'active' : ''}`}>
          <span>2</span>
          <label>Endereço</label>
        </div>
      </div>

      {error && (
        <div className="form-error-banner">
          <AlertCircle size={16} />
          <span>{error}</span>
        </div>
      )}

      <form onSubmit={handleSubmit} className="form-card" autoComplete="off">
        {step === 1 && (
          <div className="form-step" key="step1">
            <div className="form-group">
              <label htmlFor="nome">
                <User size={16} /> Nome Completo
              </label>
              <input
                id="nome"
                name="nome"
                type="text"
                placeholder="Ex: Maria da Silva"
                value={form.nome}
                onChange={handleChange}
                className={errors.nome ? 'has-error' : ''}
              />
              {errors.nome && <span className="field-error">{errors.nome}</span>}
            </div>

            <div className="form-row">
              <div className="form-group">
                <label htmlFor="email">
                  <Mail size={16} /> Email
                </label>
                <input
                  id="email"
                  name="email"
                  type="email"
                  placeholder="email@exemplo.com"
                  value={form.email}
                  onChange={handleChange}
                  className={errors.email ? 'has-error' : ''}
                />
                {errors.email && <span className="field-error">{errors.email}</span>}
              </div>
              <div className="form-group">
                <label htmlFor="telefone">
                  <Phone size={16} /> Telefone
                </label>
                <input
                  id="telefone"
                  name="telefone"
                  type="text"
                  placeholder="(44) 99999-9999"
                  value={form.telefone}
                  onChange={handleChange}
                  className={errors.telefone ? 'has-error' : ''}
                />
                {errors.telefone && <span className="field-error">{errors.telefone}</span>}
              </div>
            </div>

            <div className="form-row">
              <div className="form-group">
                <label htmlFor="documento">
                  <FileText size={16} /> CPF
                </label>
                <input
                  id="documento"
                  name="documento"
                  type="text"
                  placeholder="000.000.000-00"
                  value={form.documento}
                  onChange={handleChange}
                  className={errors.documento ? 'has-error' : ''}
                />
                {errors.documento && <span className="field-error">{errors.documento}</span>}
              </div>
              <div className="form-group">
                <label htmlFor="limiteCredito">
                  <CreditCard size={16} /> Limite de Crédito (R$)
                </label>
                <input
                  id="limiteCredito"
                  name="limiteCredito"
                  type="number"
                  placeholder="1000.00"
                  step="0.01"
                  min="0"
                  value={form.limiteCredito}
                  onChange={handleChange}
                  className={errors.limiteCredito ? 'has-error' : ''}
                />
                {errors.limiteCredito && <span className="field-error">{errors.limiteCredito}</span>}
              </div>
            </div>

            <div className="form-actions">
              <button type="button" className="btn-form-secondary" onClick={() => navigate('/clientes')}>
                Cancelar
              </button>
              <button type="button" className="btn-form-primary" onClick={handleNextStep}>
                Próximo
                <ArrowLeft size={16} style={{ transform: 'rotate(180deg)' }} />
              </button>
            </div>
          </div>
        )}

        {step === 2 && (
          <div className="form-step" key="step2">
            <div className="form-row">
              <div className="form-group" style={{ flex: '0 0 180px' }}>
                <label htmlFor="cep">
                  <MapPin size={16} /> CEP
                </label>
                <input
                  id="cep"
                  name="cep"
                  type="text"
                  placeholder="00000-000"
                  value={form.cep}
                  onChange={handleChange}
                  className={errors.cep ? 'has-error' : ''}
                />
                {errors.cep && <span className="field-error">{errors.cep}</span>}
              </div>
              <div className="form-group" style={{ flex: 1 }}>
                <label htmlFor="rua">Rua</label>
                <input
                  id="rua"
                  name="rua"
                  type="text"
                  placeholder="Av. Brasil"
                  value={form.rua}
                  onChange={handleChange}
                  className={errors.rua ? 'has-error' : ''}
                />
                {errors.rua && <span className="field-error">{errors.rua}</span>}
              </div>
            </div>

            <div className="form-row">
              <div className="form-group" style={{ flex: '0 0 120px' }}>
                <label htmlFor="numero">Número</label>
                <input
                  id="numero"
                  name="numero"
                  type="text"
                  placeholder="1234"
                  value={form.numero}
                  onChange={handleChange}
                  className={errors.numero ? 'has-error' : ''}
                />
                {errors.numero && <span className="field-error">{errors.numero}</span>}
              </div>
              <div className="form-group" style={{ flex: 1 }}>
                <label htmlFor="bairro">Bairro</label>
                <input
                  id="bairro"
                  name="bairro"
                  type="text"
                  placeholder="Centro"
                  value={form.bairro}
                  onChange={handleChange}
                  className={errors.bairro ? 'has-error' : ''}
                />
                {errors.bairro && <span className="field-error">{errors.bairro}</span>}
              </div>
            </div>

            <div className="form-row">
              <div className="form-group" style={{ flex: 1 }}>
                <label htmlFor="cidade">Cidade</label>
                <input
                  id="cidade"
                  name="cidade"
                  type="text"
                  placeholder="Maringá"
                  value={form.cidade}
                  onChange={handleChange}
                  className={errors.cidade ? 'has-error' : ''}
                />
                {errors.cidade && <span className="field-error">{errors.cidade}</span>}
              </div>
              <div className="form-group" style={{ flex: '0 0 120px' }}>
                <label htmlFor="estado">Estado</label>
                <input
                  id="estado"
                  name="estado"
                  type="text"
                  placeholder="PR"
                  maxLength={2}
                  value={form.estado}
                  onChange={handleChange}
                  className={errors.estado ? 'has-error' : ''}
                />
                {errors.estado && <span className="field-error">{errors.estado}</span>}
              </div>
            </div>

            <div className="form-actions">
              <button type="button" className="btn-form-secondary" onClick={() => setStep(1)}>
                <ArrowLeft size={16} />
                Voltar
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
                    <CheckCircle size={16} />
                    Cadastrar Cliente
                  </>
                )}
              </button>
            </div>
          </div>
        )}
      </form>
    </div>
  );
};

export default ClienteNovo;
