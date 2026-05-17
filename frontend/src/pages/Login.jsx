import { useState } from 'react';
import { Eye, EyeOff, Lock, Mail, ArrowRight, AlertCircle, CheckCircle, Leaf } from 'lucide-react';
import { useNavigate } from 'react-router-dom';
import Waves from '../components/Waves/Waves';
import { authLogin } from '../services/api';
import './Login.css';

const Login = () => {
  const [email, setEmail] = useState('');
  const [senha, setSenha] = useState('');
  const [showPassword, setShowPassword] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [fieldErrors, setFieldErrors] = useState({ email: '', senha: '' });
  const [showForm, setShowForm] = useState(false);

  const navigate = useNavigate();

  const handleLayoutClick = () => {
    if (!showForm) {
      setShowForm(true);
    }
  };

  const handleLogin = async (e) => {
    e.preventDefault();
    setError('');
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

      // O backend pode retornar { token, ... } ou { Token, ... }
      const token = response.token || response.Token;
      if (token) {
        localStorage.setItem('auth_token', token);
        localStorage.setItem('user', JSON.stringify(response.user || response.User || { email }));
        navigate('/dashboard');
      }
    } catch (err) {
      setError(err.message || 'Credenciais inválidas. Tente novamente.');
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
      <main className="card-container" onClick={(e) => e.stopPropagation()}>
        <div className="login-card">
          {/* Top accent line */}
          <div className="card-accent" />

          <div className="card-head">
            <h2>Bem-vindo de volta</h2>
            <p>Não tem uma conta? <a href="#">Fale com o suporte</a></p>
          </div>

          {error && (
            <div className="alert-error">
              <AlertCircle size={16} />
              <span>{error}</span>
            </div>
          )}

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
              <a href="#" className="forgot-link">Esqueci minha senha</a>
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
