import { useState } from 'react';
import { Eye, EyeOff, Lock, Mail, ArrowRight, AlertCircle, CheckCircle, Leaf, User, Briefcase, FileText, Phone, UserPlus } from 'lucide-react';
import { useNavigate } from 'react-router-dom';
import Waves from '../components/Waves/Waves';
import { authLogin, registrarEmpresa, getEmpresaByCNPJ, registrarUsuario, formatCPF, formatPhone, validateCNPJ } from '../services/api';
import './Login.css';

const Login = () => {
  const [email, setEmail] = useState('');
  const [senha, setSenha] = useState('');
  const [showPassword, setShowPassword] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [successMsg, setSuccessMsg] = useState('');
  const [fieldErrors, setFieldErrors] = useState({});
  const [showForm, setShowForm] = useState(false);

  // Estados de Cadastro
  const [isRegisterMode, setIsRegisterMode] = useState(false);
  const [nomeUsuario, setNomeUsuario] = useState('');
  const [emailUsuario, setEmailUsuario] = useState('');
  const [senhaUsuario, setSenhaUsuario] = useState('');
  const [nomeEmpresa, setNomeEmpresa] = useState('');
  const [cnpj, setCnpj] = useState('');
  const [telefoneEmpresa, setTelefoneEmpresa] = useState('');

  const navigate = useNavigate();

  const handleLayoutClick = () => {
    if (!showForm) {
      setShowForm(true);
    }
  };

  const handleLogin = async (e) => {
    e.preventDefault();
    setError('');
    setSuccessMsg('');
    setFieldErrors({ email: '', senha: '' });

    let hasErrors = false;
    const newErrors = { email: '', senha: '' };

    if (!email.trim()) {
      newErrors.email = 'E-mail é obrigatório';
      hasErrors = true;
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
      newErrors.email = 'Formato de e-mail inválido';
      hasErrors = true;
    }

    if (!senha.trim()) {
      newErrors.senha = 'Senha é obrigatória';
      hasErrors = true;
    }

    if (hasErrors) {
      setFieldErrors(newErrors);
      return;
    }

    setLoading(true);

    try {
      const response = await authLogin(email, senha);

      const token = response.token || response.Token;
      if (token) {
        localStorage.setItem('auth_token', token);
        
        const userEmail = response.email || response.Email || email;
        const userData = {
          id: response.usuarioId || response.UsuarioId,
          email: userEmail,
          name: userEmail.split('@')[0]
        };
        
        localStorage.setItem('user', JSON.stringify(userData));
        navigate('/dashboard');
      }
    } catch (err) {
      setError(err.message || 'Credenciais inválidas. Tente novamente.');
    } finally {
      setLoading(false);
    }
  };

  const handleRegister = async (e) => {
    e.preventDefault();
    setError('');
    setSuccessMsg('');
    setFieldErrors({});

    let hasErrors = false;
    const newErrors = {};

    if (!nomeUsuario.trim()) {
      newErrors.nomeUsuario = 'Nome do usuário é obrigatório';
      hasErrors = true;
    }
    if (!emailUsuario.trim()) {
      newErrors.emailUsuario = 'E-mail é obrigatório';
      hasErrors = true;
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(emailUsuario)) {
      newErrors.emailUsuario = 'E-mail inválido';
      hasErrors = true;
    }
    if (!senhaUsuario.trim()) {
      newErrors.senhaUsuario = 'Senha é obrigatória';
      hasErrors = true;
    } else if (senhaUsuario.length < 6) {
      newErrors.senhaUsuario = 'A senha deve ter no mínimo 6 caracteres';
      hasErrors = true;
    }
    if (!nomeEmpresa.trim()) {
      newErrors.nomeEmpresa = 'Nome da empresa é obrigatório';
      hasErrors = true;
    }
    if (!cnpj.trim()) {
      newErrors.cnpj = 'CNPJ é obrigatório';
      hasErrors = true;
    } else if (!validateCNPJ(cnpj)) {
      newErrors.cnpj = 'CNPJ inválido';
      hasErrors = true;
    }

    if (hasErrors) {
      setFieldErrors(newErrors);
      return;
    }

    setLoading(true);
    try {
      // 1. Criar a Empresa
      await registrarEmpresa({
        nomeEmpresa,
        cnpj: cnpj.replace(/\D/g, ''),
        emailEmpresa: emailUsuario,
        telefoneEmpresa: telefoneEmpresa.replace(/\D/g, '')
      });

      // 2. Buscar o ID da Empresa criada pelo CNPJ
      const empresa = await getEmpresaByCNPJ(cnpj.replace(/\D/g, ''));
      if (!empresa) {
        throw new Error('Empresa cadastrada, mas não encontrada no banco.');
      }

      // 3. Criar o Usuário com a Empresa vinculada
      const response = await registrarUsuario({
        nomeUsuario,
        emailUsuario,
        senhaUsuario,
        empresaId: empresa.id
      });

      // 4. Auto-login com o Token retornado
      const token = response.token || response.Token;
      if (token) {
        localStorage.setItem('auth_token', token);
        const userData = {
          id: response.usuarioId || response.UsuarioId,
          email: response.email || response.Email || emailUsuario,
          name: nomeUsuario
        };
        localStorage.setItem('user', JSON.stringify(userData));
        setSuccessMsg('Cadastro realizado com sucesso! Redirecionando...');
        setTimeout(() => navigate('/dashboard'), 1500);
      }
    } catch (err) {
      setError(err.message || 'Erro ao realizar cadastro. Tente novamente.');
    } finally {
      setLoading(false);
    }
  };

  const handleEsqueciSenha = async (e) => {
    e.preventDefault();
    setSuccessMsg('');
    setError('');

    if (!email.trim() || !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
      setError('Por favor, informe um e-mail válido no campo acima para recuperar a senha.');
      return;
    }

    setLoading(true);

    try {
      // Simula a requisição para a API
      await new Promise(resolve => setTimeout(resolve, 1200));
      setSuccessMsg(`Um link de recuperação foi enviado para ${email}`);
    } catch (err) {
      setError('Ocorreu um erro ao processar sua solicitação.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div
      className={`login-page ${showForm ? 'is-active' : ''}`}
      onClick={handleLayoutClick}
    >
      {/* Living background */}
      <Waves
        lineColor="rgba(110, 170, 100, 0.22)"
        backgroundColor="#080c07"
        waveSpeedX={0.02}
        waveSpeedY={0.01}
        waveAmpX={40}
        waveAmpY={20}
        friction={0.9}
        tension={0.01}
        maxCursorMove={120}
        xGap={12}
        yGap={36}
      />

      {/* Ambient light bloom behind the card */}
      <div className="ambient-glow" />

      {/* ── Brand hero ── */}
      <header className="brand-hero">
        <div className="brand-badge">
          <Leaf size={22} strokeWidth={2.2} />
        </div>
        <h1 className="brand-title">O Facilitador</h1>
        <p className="brand-tagline">Sistema de Gestão para o seu Comércio</p>
        {!showForm && (
          <span className="start-cta">
            Clique em qualquer lugar para começar
            <ArrowRight size={15} />
          </span>
        )}
      </header>

      {/* ── Login card ── */}
      <main className={`card-container ${isRegisterMode ? 'register-mode-active' : ''}`} onClick={(e) => e.stopPropagation()}>
        <div className={`login-card ${isRegisterMode ? 'register-card-layout' : ''}`}>
          {/* Top accent line */}
          <div className="card-accent" />

          <div className="card-head">
            <h2>{isRegisterMode ? 'Criar sua Conta' : 'Bem-vindo de volta'}</h2>
            <p>
              {isRegisterMode ? (
                <>Já tem uma conta? <a href="#" onClick={(e) => { e.preventDefault(); setIsRegisterMode(false); setError(''); setSuccessMsg(''); setFieldErrors({}); }}>Fazer Login</a></>
              ) : (
                <>Não tem uma conta? <a href="#" onClick={(e) => { e.preventDefault(); setIsRegisterMode(true); setError(''); setSuccessMsg(''); setFieldErrors({}); }}>Cadastre sua Empresa</a></>
              )}
            </p>
          </div>

          {error && (
            <div className="alert-error">
              <AlertCircle size={16} />
              <span>{error}</span>
            </div>
          )}

          {successMsg && (
            <div className="alert-error" style={{ backgroundColor: 'rgba(34, 197, 94, 0.1)', color: '#16a34a', borderColor: 'rgba(34, 197, 94, 0.2)' }}>
              <CheckCircle size={16} />
              <span>{successMsg}</span>
            </div>
          )}

          {!isRegisterMode ? (
            <form onSubmit={handleLogin} className="login-form" autoComplete="off">
              <div className="field">
                <label htmlFor="email">Email</label>
                <div className={`field-input ${fieldErrors.email ? 'has-error' : ''}`}>
                  <Mail size={18} className="field-icon" />
                  <input
                    id="email"
                    type="email"
                    placeholder="seu@email.com"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    autoComplete="email"
                  />
                </div>
                {fieldErrors.email && <span className="field-error-text">{fieldErrors.email}</span>}
              </div>

              <div className="field">
                <label htmlFor="senha">Senha</label>
                <div className={`field-input ${fieldErrors.senha ? 'has-error' : ''}`}>
                  <Lock size={18} className="field-icon" />
                  <input
                    id="senha"
                    type={showPassword ? 'text' : 'password'}
                    placeholder="Sua senha secreta"
                    value={senha}
                    onChange={(e) => setSenha(e.target.value)}
                    autoComplete="current-password"
                  />
                  <button
                    type="button"
                    className="toggle-password"
                    onClick={() => setShowPassword(!showPassword)}
                    aria-label="Alternar exibição da senha"
                  >
                    {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
                  </button>
                </div>
                {fieldErrors.senha && <span className="field-error-text">{fieldErrors.senha}</span>}
              </div>

              <div className="form-meta">
                <a 
                  href="#" 
                  className="forgot-link" 
                  onClick={handleEsqueciSenha}
                >
                  Esqueci minha senha
                </a>
              </div>

              <button
                type="submit"
                id="btn-login"
                className={`btn-primary ${loading ? 'is-loading' : ''}`}
                disabled={loading}
              >
                {loading ? (
                  <span className="spinner" />
                ) : (
                  <>
                    Entrar no Painel
                    <ArrowRight size={18} className="btn-arrow" />
                  </>
                )}
              </button>
            </form>
          ) : (
            <form onSubmit={handleRegister} className="login-form" autoComplete="off">
              {/* Seção 1: Dados do Usuário */}
              <div className="form-section-title">Dados do Gestor (Usuário)</div>
              
              <div className="field">
                <label htmlFor="nomeUsuario">Nome Completo</label>
                <div className={`field-input ${fieldErrors.nomeUsuario ? 'has-error' : ''}`}>
                  <User size={18} className="field-icon" />
                  <input
                    id="nomeUsuario"
                    type="text"
                    placeholder="Ex: João da Silva"
                    value={nomeUsuario}
                    onChange={(e) => setNomeUsuario(e.target.value)}
                  />
                </div>
                {fieldErrors.nomeUsuario && <span className="field-error-text">{fieldErrors.nomeUsuario}</span>}
              </div>

              <div className="field">
                <label htmlFor="emailUsuario">E-mail do Gestor</label>
                <div className={`field-input ${fieldErrors.emailUsuario ? 'has-error' : ''}`}>
                  <Mail size={18} className="field-icon" />
                  <input
                    id="emailUsuario"
                    type="email"
                    placeholder="gestor@empresa.com"
                    value={emailUsuario}
                    onChange={(e) => setEmailUsuario(e.target.value)}
                  />
                </div>
                {fieldErrors.emailUsuario && <span className="field-error-text">{fieldErrors.emailUsuario}</span>}
              </div>

              <div className="field">
                <label htmlFor="senhaUsuario">Senha Secreta</label>
                <div className={`field-input ${fieldErrors.senhaUsuario ? 'has-error' : ''}`}>
                  <Lock size={18} className="field-icon" />
                  <input
                    id="senhaUsuario"
                    type={showPassword ? 'text' : 'password'}
                    placeholder="Mínimo 6 caracteres"
                    value={senhaUsuario}
                    onChange={(e) => setSenhaUsuario(e.target.value)}
                  />
                  <button
                    type="button"
                    className="toggle-password"
                    onClick={() => setShowPassword(!showPassword)}
                  >
                    {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
                  </button>
                </div>
                {fieldErrors.senhaUsuario && <span className="field-error-text">{fieldErrors.senhaUsuario}</span>}
              </div>

              {/* Seção 2: Dados da Empresa */}
              <div className="form-section-title" style={{ marginTop: '1rem' }}>Dados da Empresa</div>

              <div className="field">
                <label htmlFor="nomeEmpresa">Nome do Estabelecimento / Razão Social</label>
                <div className={`field-input ${fieldErrors.nomeEmpresa ? 'has-error' : ''}`}>
                  <Briefcase size={18} className="field-icon" />
                  <input
                    id="nomeEmpresa"
                    type="text"
                    placeholder="Ex: Mercadinho Central"
                    value={nomeEmpresa}
                    onChange={(e) => setNomeEmpresa(e.target.value)}
                  />
                </div>
                {fieldErrors.nomeEmpresa && <span className="field-error-text">{fieldErrors.nomeEmpresa}</span>}
              </div>

              <div className="form-row" style={{ display: 'flex', gap: '1rem' }}>
                <div className="field" style={{ flex: 1 }}>
                  <label htmlFor="cnpj">CNPJ</label>
                  <div className={`field-input ${fieldErrors.cnpj ? 'has-error' : ''}`}>
                    <FileText size={18} className="field-icon" />
                    <input
                      id="cnpj"
                      type="text"
                      placeholder="00.000.000/0000-00"
                      value={cnpj}
                      onChange={(e) => {
                        const clean = e.target.value.replace(/\D/g, '').slice(0, 14);
                        let formatted = clean;
                        if (clean.length > 2) formatted = clean.replace(/^(\d{2})(\d)/, '$1.$2');
                        if (clean.length > 5) formatted = formatted.replace(/^(\d{2})\.(\d{3})(\d)/, '$1.$2.$3');
                        if (clean.length > 8) formatted = formatted.replace(/^(\d{2})\.(\d{3})\.(\d{3})(\d)/, '$1.$2.$3/$4');
                        if (clean.length > 12) formatted = formatted.replace(/^(\d{2})\.(\d{3})\.(\d{3})\/(\d{4})(\d)/, '$1.$2.$3/$4-$5');
                        setCnpj(formatted);
                      }}
                    />
                  </div>
                  {fieldErrors.cnpj && <span className="field-error-text">{fieldErrors.cnpj}</span>}
                </div>

                <div className="field" style={{ flex: 1 }}>
                  <label htmlFor="telefoneEmpresa">Telefone (Opcional)</label>
                  <div className={`field-input ${fieldErrors.telefoneEmpresa ? 'has-error' : ''}`}>
                    <Phone size={18} className="field-icon" />
                    <input
                      id="telefoneEmpresa"
                      type="text"
                      placeholder="(00) 00000-0000"
                      value={telefoneEmpresa}
                      onChange={(e) => setTelefoneEmpresa(formatPhone(e.target.value))}
                    />
                  </div>
                  {fieldErrors.telefoneEmpresa && <span className="field-error-text">{fieldErrors.telefoneEmpresa}</span>}
                </div>
              </div>

              <button
                type="submit"
                id="btn-register"
                className={`btn-primary ${loading ? 'is-loading' : ''}`}
                style={{ marginTop: '1.5rem' }}
                disabled={loading}
              >
                {loading ? (
                  <span className="spinner" />
                ) : (
                  <>
                    Cadastrar e Acessar
                    <UserPlus size={18} className="btn-arrow" />
                  </>
                )}
              </button>
            </form>
          )}

          <footer className="card-foot">
            <div className="feature">
              <CheckCircle size={14} className="feature-check" />
              <span>Controle de fiado</span>
            </div>
            <span className="foot-dot">·</span>
            <div className="feature">
              <CheckCircle size={14} className="feature-check" />
              <span>Extrato por cliente</span>
            </div>
          </footer>
        </div>
      </main>
    </div>
  );
};

export default Login;
