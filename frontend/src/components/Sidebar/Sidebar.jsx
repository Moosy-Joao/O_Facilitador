import { NavLink, useLocation } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import styles from './Sidebar.module.css';

export default function Sidebar({ isOpen, onClose }) {
  const { user, logout } = useAuth();
  const location = useLocation();

  const navItems = [
    { path: '/', icon: '📊', label: 'Painel', section: 'principal' },
    { path: '/clientes', icon: '👥', label: 'Clientes', section: 'principal' },
    { path: '/clientes/novo', icon: '➕', label: 'Novo Cliente', section: 'cadastro' },
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
            <div className={styles['sidebar-logo']}>💰</div>
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
          <div className={styles['sidebar-user']}>
            <div className={styles['sidebar-avatar']}>{getInitials(user?.name)}</div>
            <div className={styles['sidebar-user-info']}>
              <div className={styles['sidebar-user-name']}>{user?.name || 'Usuário'}</div>
              <div className={styles['sidebar-user-role']}>{user?.role || 'gerente'}</div>
            </div>
            <button className={styles['sidebar-logout']} onClick={logout} title="Sair">
              🚪
            </button>
          </div>
        </div>
      </aside>
    </>
  );
}
