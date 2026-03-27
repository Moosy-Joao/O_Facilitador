import { NavLink, useLocation, useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import styles from './Sidebar.module.css';
import {
  LayoutDashboard,
  Users,
  UserPlus,
  Landmark,
  LogOut,
  UserCog,
} from 'lucide-react';

export default function Sidebar({ isOpen, onClose }) {
  const { user, logout } = useAuth();
  const location = useLocation();
  const navigate = useNavigate();

  const navItems = [
    { path: '/', icon: <LayoutDashboard size={18} />, label: 'Painel', section: 'principal' },
    { path: '/clientes', icon: <Users size={18} />, label: 'Clientes', section: 'principal' },
    { path: '/clientes/novo', icon: <UserPlus size={18} />, label: 'Novo Cliente', section: 'cadastro' },
  ];

  const sections = {
    principal: 'Menu',
    cadastro: 'Cadastro',
  };

  const getInitials = (name) => {
    if (!name) return '?';
    return name
      .split(' ')
      .map((n) => n[0])
      .join('')
      .toUpperCase()
      .slice(0, 2);
  };

  const sectionOrder = ['principal', 'cadastro'];

  return (
    <>
      <div
        className={`${styles['sidebar-overlay']} ${isOpen ? styles.visible : ''}`}
        onClick={onClose}
      />
      <aside className={`${styles.sidebar} ${isOpen ? styles.open : ''}`}>
        <div className={styles['sidebar-header']}>
          <div className={styles['sidebar-brand']}>
            <div className={styles['sidebar-logo']}>
              <Landmark size={22} strokeWidth={2} />
            </div>
            <div>
              <div className={styles['sidebar-title']}>Facilitador</div>
              <div className={styles['sidebar-subtitle']}>Sistema de Crédito</div>
            </div>
          </div>
        </div>

        <nav className={styles['sidebar-nav']}>
          {sectionOrder.map((section) => (
            <div key={section}>
              <div className={styles['sidebar-section-label']}>{sections[section]}</div>
              {navItems
                .filter((item) => item.section === section)
                .map((item) => (
                  <NavLink
                    key={item.path}
                    to={item.path}
                    end={item.path === '/'}
                    className={({ isActive }) =>
                      `${styles['sidebar-link']} ${isActive ? styles.active : ''}`
                    }
                    onClick={onClose}
                  >
                    <span className={styles['sidebar-link-icon']}>{item.icon}</span>
                    <span className={styles['sidebar-link-text']}>{item.label}</span>
                  </NavLink>
                ))}
            </div>
          ))}
        </nav>

        <div className={styles['sidebar-footer']}>
          <div
            className={styles['sidebar-user']}
            onClick={() => { navigate('/perfil'); if (onClose) onClose(); }}
            role="button"
            tabIndex={0}
            title="Editar perfil"
            onKeyDown={(e) => e.key === 'Enter' && navigate('/perfil')}
          >
            <div className={styles['sidebar-avatar']}>{getInitials(user?.name)}</div>
            <div className={styles['sidebar-user-info']}>
              <div className={styles['sidebar-user-name']}>{user?.name || 'Usuário'}</div>
              <div className={styles['sidebar-user-role']}>{user?.role || 'gerente'}</div>
            </div>
            <button
              className={styles['sidebar-edit-profile']}
              onClick={(e) => { e.stopPropagation(); navigate('/perfil'); if (onClose) onClose(); }}
              title="Editar perfil"
            >
              <UserCog size={16} />
            </button>
            <button className={styles['sidebar-logout']} onClick={(e) => { e.stopPropagation(); logout(); }} title="Sair">
              <LogOut size={17} />
            </button>
          </div>
        </div>
      </aside>
    </>
  );
}
