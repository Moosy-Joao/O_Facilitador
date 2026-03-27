import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import { mockProfileService } from '../../services/mockData';
import { User, Mail, Lock, Save, ArrowLeft, Eye, EyeOff, CheckCircle } from 'lucide-react';
import styles from './EditProfile.module.css';

export default function EditProfile() {
  const { user, updateUser } = useAuth();
  const navigate = useNavigate();

  const [formData, setFormData] = useState({
    name: user?.name || '',
    email: user?.email || '',
    currentPassword: '',
    newPassword: '',
    confirmPassword: '',
  });

  const [showCurrentPwd, setShowCurrentPwd] = useState(false);
  const [showNewPwd, setShowNewPwd]         = useState(false);
  const [showConfirmPwd, setShowConfirmPwd] = useState(false);

  const [loading, setLoading]   = useState(false);
  const [success, setSuccess]   = useState(false);
  const [errors, setErrors]     = useState({});
  const [serverError, setServerError] = useState('');

  const getInitials = (name) => {
    if (!name) return '?';
    return name.split(' ').map((n) => n[0]).join('').toUpperCase().slice(0, 2);
  };

  const validate = () => {
    const errs = {};
    if (!formData.name.trim()) errs.name = 'Nome é obrigatório';
    if (!formData.email.trim()) errs.email = 'E-mail é obrigatório';
    else if (!/\S+@\S+\.\S+/.test(formData.email)) errs.email = 'E-mail inválido';

    if (formData.newPassword || formData.currentPassword || formData.confirmPassword) {
      if (!formData.currentPassword) errs.currentPassword = 'Informe a senha atual';
      if (!formData.newPassword) errs.newPassword = 'Informe a nova senha';
      else if (formData.newPassword.length < 6) errs.newPassword = 'Mínimo de 6 caracteres';
      if (formData.newPassword !== formData.confirmPassword)
        errs.confirmPassword = 'As senhas não coincidem';
    }

    return errs;
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
    setErrors((prev) => ({ ...prev, [name]: '' }));
    setServerError('');
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const errs = validate();
    if (Object.keys(errs).length) { setErrors(errs); return; }

    setLoading(true);
    setServerError('');
    try {
      const payload = { name: formData.name.trim(), email: formData.email.trim() };
      if (formData.newPassword) {
        payload.currentPassword = formData.currentPassword;
        payload.newPassword     = formData.newPassword;
      }

      const updated = await mockProfileService.update(user.id, payload);
      updateUser(updated);
      setSuccess(true);
      setFormData((prev) => ({ ...prev, currentPassword: '', newPassword: '', confirmPassword: '' }));
      setTimeout(() => setSuccess(false), 3500);
    } catch (err) {
      setServerError(err.message || 'Erro ao salvar alterações');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className={styles.page}>
      <div className={styles.container}>

        {/* Header */}
        <div className={styles.header}>
          <button className={styles.backBtn} onClick={() => navigate(-1)}>
            <ArrowLeft size={16} />
            Voltar
          </button>
          <div>
            <h1 className={styles.title}>Editar Perfil</h1>
            <p className={styles.subtitle}>Atualize suas informações pessoais</p>
          </div>
        </div>

        <div className={styles.content}>

          {/* Avatar Card */}
          <div className={styles.avatarCard}>
            <div className={styles.avatarCircle}>{getInitials(formData.name)}</div>
            <div className={styles.avatarInfo}>
              <span className={styles.avatarName}>{formData.name || 'Usuário'}</span>
              <span className={styles.avatarRole}>{user?.role || 'gerente'}</span>
            </div>
          </div>

          {/* Success Banner */}
          {success && (
            <div className={styles.successBanner}>
              <CheckCircle size={18} />
              Perfil atualizado com sucesso!
            </div>
          )}

          {/* Server Error */}
          {serverError && (
            <div className={styles.errorBanner}>{serverError}</div>
          )}

          {/* Form */}
          <form className={styles.form} onSubmit={handleSubmit} noValidate>

            {/* Dados Pessoais */}
            <section className={styles.section}>
              <h2 className={styles.sectionTitle}>Dados Pessoais</h2>

              <div className={styles.field}>
                <label className={styles.label} htmlFor="name">
                  <User size={14} /> Nome completo
                </label>
                <input
                  id="name"
                  name="name"
                  type="text"
                  className={`${styles.input} ${errors.name ? styles.inputError : ''}`}
                  value={formData.name}
                  onChange={handleChange}
                  placeholder="Seu nome"
                />
                {errors.name && <span className={styles.fieldError}>{errors.name}</span>}
              </div>

              <div className={styles.field}>
                <label className={styles.label} htmlFor="email">
                  <Mail size={14} /> E-mail
                </label>
                <input
                  id="email"
                  name="email"
                  type="email"
                  className={`${styles.input} ${errors.email ? styles.inputError : ''}`}
                  value={formData.email}
                  onChange={handleChange}
                  placeholder="seu@email.com"
                />
                {errors.email && <span className={styles.fieldError}>{errors.email}</span>}
              </div>
            </section>

            {/* Alterar Senha */}
            <section className={styles.section}>
              <h2 className={styles.sectionTitle}>Alterar Senha</h2>
              <p className={styles.sectionHint}>Preencha apenas se quiser alterar sua senha.</p>

              <div className={styles.field}>
                <label className={styles.label} htmlFor="currentPassword">
                  <Lock size={14} /> Senha atual
                </label>
                <div className={styles.passwordWrapper}>
                  <input
                    id="currentPassword"
                    name="currentPassword"
                    type={showCurrentPwd ? 'text' : 'password'}
                    className={`${styles.input} ${errors.currentPassword ? styles.inputError : ''}`}
                    value={formData.currentPassword}
                    onChange={handleChange}
                    placeholder="••••••••"
                  />
                  <button type="button" className={styles.eyeBtn} onClick={() => setShowCurrentPwd((v) => !v)}>
                    {showCurrentPwd ? <EyeOff size={16} /> : <Eye size={16} />}
                  </button>
                </div>
                {errors.currentPassword && <span className={styles.fieldError}>{errors.currentPassword}</span>}
              </div>

              <div className={styles.row}>
                <div className={styles.field}>
                  <label className={styles.label} htmlFor="newPassword">
                    Nova senha
                  </label>
                  <div className={styles.passwordWrapper}>
                    <input
                      id="newPassword"
                      name="newPassword"
                      type={showNewPwd ? 'text' : 'password'}
                      className={`${styles.input} ${errors.newPassword ? styles.inputError : ''}`}
                      value={formData.newPassword}
                      onChange={handleChange}
                      placeholder="••••••••"
                    />
                    <button type="button" className={styles.eyeBtn} onClick={() => setShowNewPwd((v) => !v)}>
                      {showNewPwd ? <EyeOff size={16} /> : <Eye size={16} />}
                    </button>
                  </div>
                  {errors.newPassword && <span className={styles.fieldError}>{errors.newPassword}</span>}
                </div>

                <div className={styles.field}>
                  <label className={styles.label} htmlFor="confirmPassword">
                    Confirmar nova senha
                  </label>
                  <div className={styles.passwordWrapper}>
                    <input
                      id="confirmPassword"
                      name="confirmPassword"
                      type={showConfirmPwd ? 'text' : 'password'}
                      className={`${styles.input} ${errors.confirmPassword ? styles.inputError : ''}`}
                      value={formData.confirmPassword}
                      onChange={handleChange}
                      placeholder="••••••••"
                    />
                    <button type="button" className={styles.eyeBtn} onClick={() => setShowConfirmPwd((v) => !v)}>
                      {showConfirmPwd ? <EyeOff size={16} /> : <Eye size={16} />}
                    </button>
                  </div>
                  {errors.confirmPassword && <span className={styles.fieldError}>{errors.confirmPassword}</span>}
                </div>
              </div>
            </section>

            <div className={styles.actions}>
              <button type="button" className={styles.cancelBtn} onClick={() => navigate(-1)}>
                Cancelar
              </button>
              <button type="submit" className={styles.saveBtn} disabled={loading}>
                {loading ? (
                  <span className={styles.spinner} />
                ) : (
                  <Save size={16} />
                )}
                {loading ? 'Salvando...' : 'Salvar Alterações'}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}
