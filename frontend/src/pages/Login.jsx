import { useState } from 'react';
import { Eye, EyeOff, Lock, User, ArrowRight, AlertCircle, CheckCircle, Leaf } from 'lucide-react';
import { useNavigate } from 'react-router-dom';
import Waves from '../components/Waves/Waves';
import { authLogin } from '../services/api';
import './Login.css';

const Login = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [showPassword, setShowPassword] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
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

    if (!username || !password) {
      setError('Por favor, preencha todos os campos.');
      return;
    }

    setLoading(true);

    try {
      const response = await authLogin(username, password);

      if (response.token) {
        localStorage.setItem('auth_token', response.token);
        localStorage.setItem('user', JSON.stringify(response.user));
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
              <label htmlFor="username">Usuário</label>
              <div className="field-input">
                <User size={18} className="field-icon" />
                <input
                  id="username"
                  type="text"
                  placeholder="Nome de usuário"
                  value={username}
                  onChange={(e) => setUsername(e.target.value)}
                  autoComplete="username"
                />
              </div>
            </div>

            <div className="field">
              <label htmlFor="password">Senha</label>
              <div className="field-input">
                <Lock size={18} className="field-icon" />
                <input
                  id="password"
                  type={showPassword ? 'text' : 'password'}
                  placeholder="Sua senha secreta"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
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
