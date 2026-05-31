import { useState, useEffect } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { 
  Users, 
  UserPlus, 
  Mail, 
  Lock, 
  UserCheck, 
  Shield, 
  Eye, 
  EyeOff, 
  CheckCircle2, 
  AlertCircle, 
  ToggleLeft, 
  ToggleRight,
  ShieldAlert,
  UserX
} from 'lucide-react';
import { 
  getUsuarios, 
  criarFuncionario, 
  atualizarFuncionario, 
  toggleFuncionarioStatus,
  isGerente
} from '../services/api';
import './Funcionarios.css';

const Funcionarios = () => {
  const queryClient = useQueryClient();
  const [nome, setNome] = useState('');
  const [email, setEmail] = useState('');
  const [senha, setSenha] = useState('');
  const [cargo, setCargo] = useState(2); // 2 = Funcionario, 1 = Gerente
  const [showPassword, setShowPassword] = useState(false);
  const [editingId, setEditingId] = useState(null);
  
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [loading, setLoading] = useState(false);

  // Redireciona ou bloqueia se não for Gerente/Admin
  const gerenteAuthorized = isGerente();

  // React Query: Buscar funcionários
  const { data: funcionarios = [], isLoading } = useQuery({
    queryKey: ['funcionarios'],
    queryFn: getUsuarios,
    enabled: gerenteAuthorized
  });

  const clearForm = () => {
    setNome('');
    setEmail('');
    setSenha('');
    setCargo(2);
    setEditingId(null);
    setError('');
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setSuccess('');

    if (!nome.trim() || !email.trim()) {
      setError('Nome e E-mail são obrigatórios.');
      return;
    }

    if (!editingId && !senha.trim()) {
      setError('A senha é obrigatória para novos funcionários.');
      return;
    }

    setLoading(true);

    try {
      if (editingId) {
        // Atualizar funcionário existente
        await atualizarFuncionario(editingId, {
          nome,
          email,
          senha: senha ? senha : undefined,
          cargo
        });
        setSuccess('Funcionário atualizado com sucesso!');
      } else {
        // Criar novo funcionário
        await criarFuncionario({
          nome,
          email,
          senha,
          cargo
        });
        setSuccess('Funcionário cadastrado com sucesso!');
      }
      
      clearForm();
      queryClient.invalidateQueries(['funcionarios']);
      
      // Auto dismiss success message after 4s
      setTimeout(() => setSuccess(''), 4000);
    } catch (err) {
      setError(err.message || 'Erro ao salvar funcionário.');
    } finally {
      setLoading(false);
    }
  };

  const handleEdit = (func) => {
    setEditingId(func.id);
    setNome(func.nome);
    setEmail(func.email);
    setCargo(func.cargo);
    setSenha(''); // Deixa em branco caso não queira alterar a senha
    setError('');
    setSuccess('');
    
    // Rola suavemente até o formulário
    window.scrollTo({ top: 0, behavior: 'smooth' });
  };

  const handleToggleStatus = async (id, currentAtivo) => {
    try {
      await toggleFuncionarioStatus(id, !currentAtivo);
      queryClient.invalidateQueries(['funcionarios']);
      setSuccess(`Funcionário ${!currentAtivo ? 'ativado' : 'desativado'} com sucesso!`);
      setTimeout(() => setSuccess(''), 3000);
    } catch (err) {
      setError(err.message || 'Erro ao alterar status.');
    }
  };

  const getCargoBadge = (cargoVal) => {
    switch (cargoVal) {
      case 0:
        return { label: 'Administrador (Dono)', class: 'badge-admin', desc: 'Acesso total' };
      case 1:
        return { label: 'Gerente', class: 'badge-gerente', desc: 'Acesso total + Gerenciamento' };
      case 2:
      default:
        return { label: 'Funcionário', class: 'badge-func', desc: 'Vendas e recebimentos' };
    }
  };

  if (!gerenteAuthorized) {
    return (
      <div className="funcionarios-unauthorized">
        <ShieldAlert size={64} className="unauthorized-icon" />
        <h2>Acesso Negado</h2>
        <p>Apenas o Gerente ou Administrador da empresa pode acessar esta página e gerenciar a equipe.</p>
      </div>
    );
  }

  return (
    <div className="funcionarios-page">
      {/* Header */}
      <div className="page-header">
        <div>
          <h1 className="page-title">Equipe & Funcionários</h1>
          <p className="page-subtitle">
            Gerencie os usuários e defina quem pode vender, receber ou administrar o painel.
          </p>
        </div>
      </div>

      {success && (
        <div className="func-alert alert-success animate-slide-in">
          <CheckCircle2 size={18} />
          <span>{success}</span>
        </div>
      )}

      {error && (
        <div className="func-alert alert-error animate-slide-in">
          <AlertCircle size={18} />
          <span>{error}</span>
        </div>
      )}

      <div className="func-grid">
        {/* Formulário de Criação/Edição */}
        <div className="func-card form-card">
          <div className="card-header-accent" />
          <h3 className="form-title">
            {editingId ? 'Editar Colaborador' : 'Adicionar Novo Colaborador'}
          </h3>
          <p className="form-subtitle">
            {editingId ? 'Altere os dados ou permissões do funcionário.' : 'Crie uma nova conta com nível de acesso personalizado.'}
          </p>

          <form onSubmit={handleSubmit} className="func-form" autoComplete="off">
            <div className="func-field">
              <label>Nome Completo</label>
              <div className="func-input-wrap">
                <UserPlus size={18} className="input-icon" />
                <input
                  type="text"
                  placeholder="Nome do colaborador"
                  value={nome}
                  onChange={(e) => setNome(e.target.value)}
                  required
                />
              </div>
            </div>

            <div className="func-field">
              <label>E-mail</label>
              <div className="func-input-wrap">
                <Mail size={18} className="input-icon" />
                <input
                  type="email"
                  placeholder="email@empresa.com"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  required
                />
              </div>
            </div>

            <div className="func-field">
              <label>{editingId ? 'Alterar Senha (Opcional)' : 'Senha de Acesso'}</label>
              <div className="func-input-wrap">
                <Lock size={18} className="input-icon" />
                <input
                  type={showPassword ? 'text' : 'password'}
                  placeholder={editingId ? 'Deixe em branco para manter a atual' : 'Senha de login do colaborador'}
                  value={senha}
                  onChange={(e) => setSenha(e.target.value)}
                  required={!editingId}
                />
                <button
                  type="button"
                  className="btn-toggle-pass"
                  onClick={() => setShowPassword(!showPassword)}
                >
                  {showPassword ? <EyeOff size={16} /> : <Eye size={16} />}
                </button>
              </div>
            </div>

            {/* Seleção de Nível de Acesso (Cargo) */}
            <div className="func-field">
              <label>Nível de Acesso (Cargo)</label>
              <div className="access-options">
                <label className={`access-option ${cargo === 2 ? 'is-selected' : ''}`}>
                  <input
                    type="radio"
                    name="cargo"
                    value={2}
                    checked={cargo === 2}
                    onChange={() => setCargo(2)}
                  />
                  <div className="access-info">
                    <div className="access-name">
                      <Users size={16} /> Funcionário Comum
                    </div>
                    <p className="access-desc">
                      Pode apenas cadastrar clientes, registrar vendas (fiados) e receber pagamentos. Não tem acesso ao painel financeiro geral, configurações ou gerenciamento de equipe.
                    </p>
                  </div>
                </label>

                <label className={`access-option ${cargo === 1 ? 'is-selected' : ''}`}>
                  <input
                    type="radio"
                    name="cargo"
                    value={1}
                    checked={cargo === 1}
                    onChange={() => setCargo(1)}
                  />
                  <div className="access-info">
                    <div className="access-name">
                      <Shield size={16} /> Gerente
                    </div>
                    <p className="access-desc">
                      Acesso completo. Pode visualizar gráficos, saldos, inadimplências, reajustar saldos, ver histórico geral e criar outros funcionários/gerentes para a empresa.
                    </p>
                  </div>
                </label>
              </div>
            </div>

            <div className="form-actions">
              {editingId && (
                <button type="button" className="btn-cancel" onClick={clearForm}>
                  Cancelar
                </button>
              )}
              <button 
                type="submit" 
                className="btn-submit-func" 
                disabled={loading}
              >
                {loading ? 'Salvando...' : editingId ? 'Salvar Alterações' : 'Criar Conta de Acesso'}
              </button>
            </div>
          </form>
        </div>

        {/* Lista de Funcionários */}
        <div className="func-card list-card">
          <div className="list-header-wrap">
            <div>
              <h3>Colaboradores Ativos</h3>
              <p className="list-subtitle">Lista de contas ativas vinculadas a este estabelecimento.</p>
            </div>
            <span className="count-badge">{funcionarios.length} usuários</span>
          </div>

          {isLoading ? (
            <div className="list-loading">
              <div className="func-spinner" />
              <p>Carregando equipe...</p>
            </div>
          ) : funcionarios.length === 0 ? (
            <div className="list-empty">
              <UserX size={48} strokeWidth={1.2} />
              <p>Nenhum funcionário cadastrado ainda.</p>
            </div>
          ) : (
            <div className="func-list">
              {funcionarios.map((func) => {
                const badge = getCargoBadge(func.cargo);
                return (
                  <div key={func.id} className={`func-item ${!func.ativo ? 'is-disabled' : ''}`}>
                    <div className="func-avatar-wrap">
                      <div className={`func-avatar ${func.cargo === 2 ? 'avatar-blue' : 'avatar-green'}`}>
                        {func.nome.charAt(0).toUpperCase()}
                      </div>
                    </div>

                    <div className="func-info-main">
                      <div className="func-name-row">
                        <span className="func-name">{func.nome}</span>
                        <span className={`badge ${badge.class}`}>{badge.label}</span>
                      </div>
                      <span className="func-email">
                        <Mail size={12} /> {func.email}
                      </span>
                      <span className="func-desc">{badge.desc}</span>
                    </div>

                    <div className="func-actions-row">
                      {/* Evita desativar a própria conta de administrador/gerente */}
                      {func.cargo !== 0 && (
                        <>
                          <button
                            className="btn-action btn-edit"
                            onClick={() => handleEdit(func)}
                            title="Editar dados"
                          >
                            Editar
                          </button>
                          
                          <button
                            className={`btn-action btn-toggle-status ${func.ativo ? 'status-active' : 'status-inactive'}`}
                            onClick={() => handleToggleStatus(func.id, func.ativo)}
                            title={func.ativo ? 'Desativar usuário' : 'Ativar usuário'}
                          >
                            {func.ativo ? (
                              <>
                                <ToggleRight size={24} className="toggle-icon-active" />
                                <span>Ativo</span>
                              </>
                            ) : (
                              <>
                                <ToggleLeft size={24} className="toggle-icon-inactive" />
                                <span>Inativo</span>
                              </>
                            )}
                          </button>
                        </>
                      )}
                      
                      {func.cargo === 0 && (
                        <span className="dono-label">Dono</span>
                      )}
                    </div>
                  </div>
                );
              })}
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default Funcionarios;
