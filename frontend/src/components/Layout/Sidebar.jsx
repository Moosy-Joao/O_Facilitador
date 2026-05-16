import { useState, useRef, useEffect } from 'react';
import { Link, useNavigate, useLocation } from 'react-router-dom';
import {
  LayoutDashboard,
  Users,
  UserPlus,
  ShoppingCart,
  CreditCard,
  History,
  LogOut,
  Leaf,
  Menu,
  X,
} from 'lucide-react';
import './Sidebar.css';

const navItems = [
  { to: '/dashboard', icon: LayoutDashboard, label: 'Painel' },
  { to: '/clientes', icon: Users, label: 'Clientes' },
  { to: '/clientes/novo', icon: UserPlus, label: 'Cadastrar' },
  { to: '/vendas', icon: ShoppingCart, label: 'Venda' },
  { to: '/pagamentos', icon: CreditCard, label: 'Pagamento' },
  { to: '/historico', icon: History, label: 'Histórico' },
];

const Topbar = () => {
  const [mobileOpen, setMobileOpen] = useState(false);
  const [indicatorStyle, setIndicatorStyle] = useState({});
  const navRef = useRef(null);
  const location = useLocation();
  const navigate = useNavigate();

  const user = JSON.parse(localStorage.getItem('user') || '{}');

  // Exact match — /clientes should NOT match /clientes/novo
  const isItemActive = (itemPath) => location.pathname === itemPath;

  // Sliding pill indicator
  useEffect(() => {
    const rafId = requestAnimationFrame(() => {
      if (!navRef.current) return;
      const activeLink = navRef.current.querySelector('.topbar-link.is-active');
      if (activeLink) {
        const navRect = navRef.current.getBoundingClientRect();
        const linkRect = activeLink.getBoundingClientRect();
        setIndicatorStyle({
          left: linkRect.left - navRect.left,
          width: linkRect.width,
        });
      }
    });
    return () => cancelAnimationFrame(rafId);
  }, [location.pathname]);

  const handleLogout = () => {
    localStorage.removeItem('auth_token');
    localStorage.removeItem('user');
    navigate('/');
  };

  return (
    <>
      <header className="topbar">
        {/* Left — Brand */}
        <div className="topbar-brand">
          <div className="topbar-logo">
            <Leaf size={18} strokeWidth={2.4} />
          </div>
          <span className="topbar-brand-name">Facilitador</span>
        </div>

        {/* Center — Nav pill bar */}
        <nav className="topbar-nav" ref={navRef}>
          <div className="topbar-indicator" style={indicatorStyle} />
          {navItems.map((item) => (
            <Link
              key={item.to}
              to={item.to}
              className={`topbar-link ${isItemActive(item.to) ? 'is-active' : ''}`}
            >
              <item.icon size={16} strokeWidth={1.9} />
              <span className="topbar-link-label">{item.label}</span>
            </Link>
          ))}
        </nav>

        {/* Right — User + Logout */}
        <div className="topbar-right">
          <div className="topbar-user">
            <div className="topbar-avatar">
              {(user.name || 'U').charAt(0).toUpperCase()}
            </div>
            <span className="topbar-user-name">{user.name || 'Usuário'}</span>
          </div>
          <button className="topbar-logout" onClick={handleLogout} title="Sair">
            <LogOut size={16} strokeWidth={1.9} />
          </button>
        </div>

        {/* Mobile hamburger */}
        <button
          className="topbar-hamburger"
          onClick={() => setMobileOpen(!mobileOpen)}
        >
          {mobileOpen ? <X size={20} /> : <Menu size={20} />}
        </button>
      </header>

      {/* Mobile drawer */}
      {mobileOpen && (
        <div className="mobile-drawer-overlay" onClick={() => setMobileOpen(false)}>
          <nav className="mobile-drawer" onClick={(e) => e.stopPropagation()}>
            {navItems.map((item) => (
              <Link
                key={item.to}
                to={item.to}
                className={`mobile-link ${isItemActive(item.to) ? 'is-active' : ''}`}
                onClick={() => setMobileOpen(false)}
              >
                <item.icon size={18} strokeWidth={1.8} />
                <span>{item.label}</span>
              </Link>
            ))}
            <button className="mobile-link mobile-logout" onClick={handleLogout}>
              <LogOut size={18} strokeWidth={1.8} />
              <span>Sair</span>
            </button>
          </nav>
        </div>
      )}
    </>
  );
};

export default Topbar;

