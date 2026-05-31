import React, { useState, useEffect } from 'react';
import { useSearchParams, useNavigate } from 'react-router-dom';
import { Eye, EyeOff, Lock, CheckCircle, AlertCircle, ArrowLeft, ArrowRight, Leaf } from 'lucide-react';
import Waves from '../components/Waves/Waves';
import { resetarSenha } from '../services/api';
import './Login.css';

const ResetarSenha = () => {
  const [searchParams] = useSearchParams();
  const token = searchParams.get('token');
  const navigate = useNavigate();

  const [senha, setSenha] = useState('');
  const [confirmarSenha, setConfirmarSenha] = useState('');
  const [showSenha, setShowSenha] = useState(false);
  const [showConfirmarSenha, setShowConfirmarSenha] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState(false);

  useEffect(() => {
    if (!token) {
      setError('Token de redefinição ausente. Por favor, utilize o link recebido por e-mail.');
    }
  }, [token]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');

    if (!token) {
      setError('Não é possível redefinir a senha sem um token válido.');
      return;
    }

    if (senha.length < 6) {
      setError('A nova senha deve ter pelo menos 6 caracteres.');
      return;
    }

    if (senha !== confirmarSenha) {
      setError('As senhas não coincidem.');
      return;
    }

    setLoading(true);

    try {
      await resetarSenha(token, senha);
      setSuccess(true);
      setTimeout(() => {
        navigate('/');
      }, 3000);
    } catch (err) {
      console.error(err);
      setError(err.message || 'Erro ao redefinir a senha. O link pode ter expirado.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="login-page is-active">
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
      <div className="ambient-glow" style={{ opacity: 1 }} />

      {/* ── Brand hero ── */}
      <header className="brand-hero">
        <div className="brand-badge" style={{ boxShadow: '0 0 30px rgba(74, 222, 128, 0.25)' }}>
          <Leaf size={22} strokeWidth={2.2} />
        </div>
        <h1 className="brand-title">O Facilitador</h1>
        <p className="brand-tagline">Sistema de Gestão para o seu Comércio</p>
      </header>

      {/* ── Reset password card ── */}
      <main className="card-container" style={{ display: 'block' }} onClick={(e) => e.stopPropagation()}>
        <div className="login-card">
          {/* Top accent line */}
          <div className="card-accent" />

          <div className="card-head">
            <h2>Redefinir Senha</h2>
            <p>Escolha uma nova senha de acesso</p>
          </div>

          {error && (
            <div className="alert-error">
              <AlertCircle size={16} />
              <span>{error}</span>
            </div>
          )}

          <form onSubmit={handleSubmit} className="login-form" autoComplete="off">
            <div className="field">
              <label htmlFor="senha">Nova Senha</label>
              <div className="field-input">
                <Lock size={18} className="field-icon" />
                <input
                  id="senha"
                  type={showSenha ? 'text' : 'password'}
                  placeholder="Mínimo 6 caracteres"
                  value={senha}
                  onChange={(e) => setSenha(e.target.value)}
                  required
                  disabled={!token || loading}
                />
                <button
                  type="button"
                  className="toggle-password"
                  onClick={() => setShowSenha(!showSenha)}
                  disabled={!token}
                  aria-label="Alternar exibição da senha"
                >
                  {showSenha ? <EyeOff size={18} /> : <Eye size={18} />}
                </button>
              </div>
            </div>

            <div className="field">
              <label htmlFor="confirmarSenha">Confirmar Nova Senha</label>
              <div className="field-input">
                <Lock size={18} className="field-icon" />
                <input
                  id="confirmarSenha"
                  type={showConfirmarSenha ? 'text' : 'password'}
                  placeholder="Confirme a nova senha"
                  value={confirmarSenha}
                  onChange={(e) => setConfirmarSenha(e.target.value)}
                  required
                  disabled={!token || loading}
                />
                <button
                  type="button"
                  className="toggle-password"
                  onClick={() => setShowConfirmarSenha(!showConfirmarSenha)}
                  disabled={!token}
                  aria-label="Alternar exibição da senha"
                >
                  {showConfirmarSenha ? <EyeOff size={18} /> : <Eye size={18} />}
                </button>
              </div>
            </div>

            <button
              type="submit"
              className="btn-primary"
              disabled={!token || loading}
              style={{ marginTop: '1rem' }}
            >
              {loading ? (
                <span className="spinner" />
              ) : (
                <>
                  Salvar Nova Senha
                  <ArrowRight size={18} className="btn-arrow" />
                </>
              )}
            </button>
          </form>

          <footer className="card-foot">
            <a
              href="#"
              className="forgot-link"
              onClick={(e) => {
                e.preventDefault();
                navigate('/');
              }}
              style={{ display: 'flex', alignItems: 'center', gap: '0.4rem' }}
            >
              <ArrowLeft size={14} />
              Voltar ao login
            </a>
          </footer>
        </div>
      </main>

      {/* Success Fullscreen Overlay */}
      {success && (
        <div className="success-fullscreen-overlay">
          <div className="overlay-glow" />
          <div className="overlay-content">
            <div className="overlay-badge">
              <CheckCircle size={48} strokeWidth={1.5} className="overlay-check" />
            </div>
            <h1 className="overlay-title">Senha Alterada!</h1>
            <p className="overlay-subtitle">Sua senha foi redefinida. Redirecionando para o login...</p>
            <div className="progress-bar-container">
              <div className="progress-bar-fill" />
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default ResetarSenha;
