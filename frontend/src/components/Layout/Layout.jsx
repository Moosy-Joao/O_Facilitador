import { useState } from 'react';
import { Outlet, useLocation } from 'react-router-dom';
import Sidebar from '../Sidebar/Sidebar';
import styles from './Layout.module.css';

const pageTitles = {
  '/': 'Painel Geral',
  '/clientes': 'Clientes',
  '/clientes/novo': 'Novo Cliente',
};

export default function Layout() {
  const [sidebarOpen, setSidebarOpen] = useState(false);
  const location = useLocation();

  const getPageTitle = () => {
    if (location.pathname.match(/^\/clientes\/\d+\/extrato$/)) return 'Extrato do Cliente';
    if (location.pathname.match(/^\/clientes\/\d+\/editar$/)) return 'Editar Cliente';
    if (location.pathname.match(/^\/clientes\/\d+$/)) return 'Detalhes do Cliente';
    return pageTitles[location.pathname] || 'Facilitador';
  };

  const formatDate = () => {
    return new Date().toLocaleDateString('pt-BR', {
      weekday: 'long',
      year: 'numeric',
      month: 'long',
      day: 'numeric',
    });
  };

  return (
    <div className={styles.layout}>
      <Sidebar isOpen={sidebarOpen} onClose={() => setSidebarOpen(false)} />
      <div className={styles['layout-content']}>
        <header className={styles['layout-header']}>
          <div className={styles['layout-header-left']}>
            <button
              className={styles['layout-menu-btn']}
              onClick={() => setSidebarOpen(true)}
            >
              ☰
            </button>
            <h1 className={styles['layout-page-title']}>{getPageTitle()}</h1>
          </div>
          <div className={styles['layout-header-right']}>
            <span className={styles['layout-date']}>{formatDate()}</span>
          </div>
        </header>
        <main className={styles['layout-main']}>
          <Outlet />
        </main>
      </div>
    </div>
  );
}
