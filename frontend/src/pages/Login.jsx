import { useState } from 'react';
import { Eye, EyeOff, Lock, User, ArrowRight, AlertCircle, CheckCircle } from 'lucide-react';
import { useNavigate } from 'react-router-dom';
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
      const response = await mockAuthApi(username, password);
      
      if (response.token) {
        localStorage.setItem('auth_token', response.token);
        localStorage.setItem('user', JSON.stringify(response.user));
        alert("Login efetuado com sucesso!");
      }
    } catch (err) {
      setError(err.message || 'Credenciais inválidas. Tente novamente.');
    } finally {
      setLoading(false);
    }
  };

  const mockAuthApi = (user, pass) => {
    return new Promise((resolve, reject) => {
      setTimeout(() => {
        if (user === 'admin' && pass === '123456') {
          resolve({
            token: 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.mockToken_backend',
            user: { id: 1, name: 'Seu João', role: 'admin' }
          });
        } else {
          reject(new Error('Usuário ou senha incorretos'));
        }
      }, 1500);
    });
  };

  return (
    <div className={`login-layout ${showForm ? 'show-form' : 'hide-form'}`} onClick={handleLayoutClick}>
      {/* Full-page background GIF and independent overlay */}
      <div className="login-bg" />
      <div className="login-overlay" />

      {/* Hero title */}
      <div className="hero-title-wrapper">
        <h1 className="hero-title">O Facilitador</h1>
        {!showForm ? (
          <p className="click-to-start">Clique em qualquer lugar para começar</p>
        ) : (
          <p className="hero-subtitle">Sistema de Gestão para o seu Comércio</p>
        )}
      </div>

      {/* Centered glass card */}
      <div className="login-center" onClick={(e) => e.stopPropagation()}>
        <div className="glass-card">
          <div className="card-header">
            <h1>Bem-vindo de volta</h1>
            <p>Não tem uma conta? <a href="#">Fale com o suporte</a></p>
          </div>

          {error && (
            <div className="error-message">
              <AlertCircle size={18} />
              <span>{error}</span>
            </div>
          )}

          <form onSubmit={handleLogin} className="login-form">
            <div className="underline-input-group">
              <label htmlFor="username">
                <User size={16} />
                <span>Usuário</span>
              </label>
              <input
                id="username"
                type="text"
                placeholder="Nome de usuário"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                autoComplete="username"
              />
            </div>

            <div className="underline-input-group">
              <label htmlFor="password">
                <Lock size={16} />
                <span>Senha</span>
              </label>
              <div className="password-wrapper">
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
                  className="eye-toggle"
                  onClick={() => setShowPassword(!showPassword)}
                  aria-label="Alternar exibição da senha"
                >
                  {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
                </button>
              </div>
            </div>

            <div className="forgot-row">
              <a href="#" className="forgot-password">Esqueci minha senha</a>
            </div>

            <button
              type="submit"
              id="btn-login"
              className={`btn-submit ${loading ? 'loading' : ''}`}
              disabled={loading}
            >
              {loading ? (
                <span className="spinner" />
              ) : (
                <>Entrar no Painel <ArrowRight size={20} className="btn-icon" /></>
              )}
            </button>
          </form>

          <div className="card-features">
            <div className="feature-item"><CheckCircle size={16} className="feature-icon" /><span>Controle de fiado</span></div>
            <div className="feature-item"><CheckCircle size={16} className="feature-icon" /><span>Extrato por cliente</span></div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Login;
