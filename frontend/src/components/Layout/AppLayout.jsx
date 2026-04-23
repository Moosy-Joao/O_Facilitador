import { Outlet } from 'react-router-dom';
import Topbar from './Sidebar';
import './AppLayout.css';

const AppLayout = () => {
  return (
    <div className="app-shell">
      {/* Ambient mesh background */}
      <div className="mesh-bg" aria-hidden="true">
        <div className="mesh-orb mesh-orb-1" />
        <div className="mesh-orb mesh-orb-2" />
        <div className="mesh-orb mesh-orb-3" />
      </div>

      <Topbar />
      <main className="app-canvas">
        <Outlet />
      </main>
    </div>
  );
};

export default AppLayout;
