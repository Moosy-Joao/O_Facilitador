import { useState, useEffect } from 'react';
import {
  ArrowUpRight,
  ArrowDownRight,
  AlertTriangle,
  Users,
  ShoppingCart,
  CreditCard,
  RefreshCw,
  Clock,
  TrendingUp,
} from 'lucide-react';
import './Dashboard.css';

/* ─────────── MINI CHART (SVG) ─────────── */

const BentoChart = ({ data }) => {
  if (!data || data.length === 0) return null;
  const width = 400;
  const height = 120;
  const padding = { top: 10, right: 0, bottom: 0, left: 0 };

  const chartW = width - padding.left - padding.right;
  const chartH = height - padding.top - padding.bottom;

  const values = data.map((d) => d.value);
  const minVal = Math.min(...values) * 0.95;
  const maxVal = Math.max(...values) * 1.05;

  const xScale = (i) => padding.left + (i / (data.length - 1)) * chartW;
  const yScale = (v) => padding.top + chartH - ((v - minVal) / (maxVal - minVal)) * chartH;

  const linePath = data
    .map((d, i) => `${i === 0 ? 'M' : 'L'}${xScale(i)},${yScale(d.value)}`)
    .join(' ');

  const areaPath = `${linePath} L${xScale(data.length - 1)},${height} L${xScale(0)},${height} Z`;

  return (
    <svg viewBox={`0 0 ${width} ${height}`} className="bento-chart-svg" preserveAspectRatio="none">
      <defs>
        <linearGradient id="bentoGrad" x1="0" y1="0" x2="0" y2="1">
          <stop offset="0%" stopColor="rgba(34, 197, 94, 0.3)" />
          <stop offset="100%" stopColor="rgba(34, 197, 94, 0)" />
        </linearGradient>
      </defs>

      <path d={areaPath} fill="url(#bentoGrad)" />
      
      <path
        d={linePath}
        fill="none"
        stroke="#16a34a"
        strokeWidth="3.5"
        strokeLinejoin="round"
        strokeLinecap="round"
        style={{ filter: 'drop-shadow(0px 6px 8px rgba(22, 163, 74, 0.25))' }}
      />
    </svg>
  );
};

/* ─────────── DASHBOARD PAGE ─────────── */

const Dashboard = () => {
  const [loading, setLoading] = useState(true);
  const [stats, setStats] = useState({
    totalReceber: 0,
    totalReceberVar: 0,
    inadimplentes: 0,
    inadimplentesValor: 0,
    totalClientes: 0,
    novosClientesSemana: 0,
    vendasHoje: 0,
    pagamentosHoje: 0,
  });
  const [movimentacoes, setMovimentacoes] = useState([]);
  const [chartData, setChartData] = useState([]);

  const user = JSON.parse(localStorage.getItem('user') || '{}');

  useEffect(() => {
    const fetchDashboardData = async () => {
      try {
        const apiUrl = import.meta.env.VITE_API_URL || 'http://localhost:5238/api';
        
        const [statsRes, movRes, chartRes] = await Promise.all([
          fetch(`${apiUrl}/Dashboard/stats`).catch(() => null),
          fetch(`${apiUrl}/Dashboard/transactions`).catch(() => null),
          fetch(`${apiUrl}/Dashboard/chart`).catch(() => null)
        ]);

        if (statsRes && statsRes.ok) setStats(await statsRes.json());
        if (movRes && movRes.ok) setMovimentacoes(await movRes.json());
        if (chartRes && chartRes.ok) setChartData(await chartRes.json());
      } catch (error) {
        console.error("Erro ao buscar dados do dashboard:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchDashboardData();
  }, []);

  const formatCurrency = (val) =>
    val.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL', minimumFractionDigits: 2 });

  if (loading) {
    return (
      <div className="dashboard-loading">
        <div className="loading-spinner" />
        <p>Sincronizando dados...</p>
      </div>
    );
  }

  const [, decimalPart] = formatCurrency(stats.totalReceber).split(',');
  const integerPart = formatCurrency(stats.totalReceber).split(',')[0];

  return (
    <div className="bento-dashboard">
      {/* Top action row */}
      <div className="bento-header-actions">
        <div>
          <h1 className="greeting">Olá, {user.name || user.Name || 'Gestor'}</h1>
          <p className="greeting-sub">Aqui está o resumo do seu negócio hoje.</p>
        </div>
        <button className="btn-refresh" onClick={() => window.location.reload()}>
          <RefreshCw size={16} />
          Atualizar
        </button>
      </div>

      <div className="bento-grid">
        
        {/* HERO METRIC: Total a Receber */}
        <div className="bento-box hero-box">
          <div className="hero-box-content">
            <span className="bento-label">
              <TrendingUp size={16} /> Total a Receber
            </span>
            <div className="hero-value-wrap">
              <span className="hero-value">{integerPart}</span>
              <span className="hero-decimal">,{decimalPart}</span>
            </div>
            <div className="hero-trend">
              <span className={`trend-badge ${stats.totalReceberVar >= 0 ? 'up' : 'down'}`}>
                {stats.totalReceberVar >= 0 ? <ArrowUpRight size={14} /> : <ArrowDownRight size={14} />}
                {Math.abs(stats.totalReceberVar)}% 
              </span>
              <span className="trend-text">vs. mês passado</span>
            </div>
          </div>
          <div className="hero-box-chart">
             <BentoChart data={chartData} />
          </div>
        </div>

        {/* ALERTS: Inadimplentes */}
        <div className="bento-box alert-box">
          <div className="bento-box-header">
            <span className="bento-label danger-label">
              <AlertTriangle size={16} /> Inadimplência
            </span>
          </div>
          <div className="alert-content">
            <div className="alert-value">{stats.inadimplentes}</div>
            <div className="alert-desc">clientes em atraso</div>
            <div className="alert-sub-value">
              Risco: {formatCurrency(stats.inadimplentesValor)}
            </div>
          </div>
        </div>

        {/* STATS: Clientes */}
        <div className="bento-box stat-box">
           <div className="bento-box-header">
            <span className="bento-label info-label">
              <Users size={16} /> Base de Clientes
            </span>
          </div>
          <div className="stat-flex">
            <div className="stat-main">
              <span className="stat-number">{stats.totalClientes}</span>
              <span className="stat-text">ativos</span>
            </div>
            <div className="stat-divider" />
            <div className="stat-secondary">
              <span className="stat-number-sm">+{stats.novosClientesSemana}</span>
              <span className="stat-text">esta semana</span>
            </div>
          </div>
        </div>

        {/* TODAY: Vendas & Pagamentos */}
        <div className="bento-box today-box">
          <div className="bento-box-header">
            <span className="bento-label">
              <Clock size={16} /> Fluxo do Dia
            </span>
          </div>
          <div className="today-flow">
            <div className="flow-item">
              <div className="flow-icon flow-out">
                <ShoppingCart size={18} />
              </div>
              <div className="flow-data">
                <span className="flow-title">Vendas (Fiado)</span>
                <span className="flow-val">{formatCurrency(stats.vendasHoje)}</span>
              </div>
            </div>
            <div className="flow-item">
              <div className="flow-icon flow-in">
                <CreditCard size={18} />
              </div>
              <div className="flow-data">
                <span className="flow-title">Recebimentos</span>
                <span className="flow-val">{formatCurrency(stats.pagamentosHoje)}</span>
              </div>
            </div>
          </div>
        </div>

        {/* RECENT TRANSACTIONS */}
        <div className="bento-box list-box">
          <div className="bento-box-header list-header">
            <span className="bento-label">Últimas Movimentações</span>
            <button className="btn-text">Ver todas</button>
          </div>
          <div className="bento-list">
            {movimentacoes.map((m) => (
              <div key={m.id} className="bento-list-item">
                <div className={`bento-avatar ${m.type === 'venda' ? 'venda-bg' : 'pag-bg'}`}>
                   {m.cliente.charAt(0)}
                </div>
                <div className="bento-list-info">
                  <span className="bento-list-name">{m.cliente}</span>
                  <span className="bento-list-type">
                    {m.type === 'venda' ? 'Venda registrada' : 'Pagamento recebido'}
                  </span>
                </div>
                <div className="bento-list-right">
                  <span className={`bento-list-val ${m.type === 'venda' ? 'val-out' : 'val-in'}`}>
                    {m.type === 'venda' ? '' : '+'}{formatCurrency(m.valor)}
                  </span>
                  <span className="bento-list-time">{m.hora}</span>
                </div>
              </div>
            ))}
          </div>
        </div>

      </div>
    </div>
  );
};

export default Dashboard;
