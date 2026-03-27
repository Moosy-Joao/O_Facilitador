import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import styles from './Login.module.css';
import { Landmark } from 'lucide-react';

export default function Login() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const { login } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');

    if (!email || !password) {
      setError('Preencha todos os campos');
      return;
    }

    setLoading(true);
    try {
      await login(email, password);
      navigate('/');
    } catch (err) {
      setError(err.message || 'Erro ao fazer login');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className={styles['login-page']}>
      <div className={styles['login-card']}>
        <div className={styles['login-brand']}>
          <div className={styles['login-logo']}><Landmark size={32} strokeWidth={1.8} /></div>
          <h1>Facilitador</h1>
          <p>Sistema de crédito para comerciantes</p>
        </div>

        <form className={styles['login-form']} onSubmit={handleSubmit}>
          {error && <div className={styles['login-error']}>{error}</div>}

          <div className={styles['form-group']}>
            <label className={styles['form-label']} htmlFor="login-email">
              E-mail
            </label>
            <input
              id="login-email"
              className={styles['form-input']}
              type="email"
              placeholder="seu@email.com"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              autoComplete="email"
            />
          </div>

          <div className={styles['form-group']}>
            <label className={styles['form-label']} htmlFor="login-password">
              Senha
            </label>
            <input
              id="login-password"
              className={styles['form-input']}
              type="password"
              placeholder="••••••••"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              autoComplete="current-password"
            />
          </div>

          <button
            type="submit"
            className={styles['login-btn']}
            disabled={loading}
          >
            {loading ? 'Entrando...' : 'Entrar'}
          </button>
        </form>

        <div className={styles['login-footer']}>
          © 2026 O Facilitador — Todos os direitos reservados
        </div>
      </div>
    </div>
  );
}
