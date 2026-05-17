/**
 * O Facilitador — API Service Layer
 * 
 * Camada de integração com o backend real.
 * Autenticação via JWT (api/v1/auth/login).
 */

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5238/api';

/* ─────────── Helpers ─────────── */

const headers = () => ({
  'Content-Type': 'application/json',
  'Authorization': `Bearer ${localStorage.getItem('auth_token') || ''}`
});

/**
 * Wrapper de fetch que intercepta 401 (token expirado/inválido)
 * e redireciona automaticamente para a tela de login.
 */
const fetchWithAuth = async (url, options = {}) => {
  const res = await fetch(url, { ...options, headers: { ...headers(), ...(options.headers || {}) } });
  if (res.status === 401) {
    logout();
    throw new Error('Sessão expirada. Faça login novamente.');
  }
  return res;
};

/**
 * Remove tokens e dados do usuário e redireciona para login.
 */
export const logout = () => {
  localStorage.removeItem('auth_token');
  localStorage.removeItem('user');
  // Redireciona apenas se não estiver já na página de login
  if (window.location.pathname !== '/') {
    window.location.href = '/';
  }
};

/**
 * Verifica se o usuário está autenticado (tem token salvo).
 */
export const isAuthenticated = () => {
  return !!localStorage.getItem('auth_token');
};

// Helper para pegar um ID default de Endereço/Empresa para não quebrar a FK
const getDefaultIds = async () => {
  let empresaId = '00000000-0000-0000-0000-000000000000';
  let enderecoId = '00000000-0000-0000-0000-000000000000';
  
  try {
    const resEmpresa = await fetchWithAuth(`${API_URL}/v1/empresa`);
    if (resEmpresa.ok) {
      const empresas = await resEmpresa.json();
      if (empresas && empresas.length > 0) empresaId = empresas[0].id;
    }
  } catch(e) {}
  
  try {
    const resEnd = await fetchWithAuth(`${API_URL}/v1/endereco`);
    if (resEnd.ok) {
      const ends = await resEnd.json();
      if (ends && ends.length > 0) enderecoId = ends[0].id;
    }
  } catch(e) {}

  return { empresaId, enderecoId };
};

/* ─────────── Auth ─────────── */

export const authLogin = async (email, senha) => {
  const res = await fetch(`${API_URL}/v1/auth/login`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ email, senha })
  });
  if (!res.ok) {
    const errText = await res.text().catch(() => '');
    throw new Error(errText || 'Email ou senha inválidos');
  }
  return await res.json();
};

/* ─────────── Clientes ─────────── */

export const getClientes = async (filtros = {}) => {
  const res = await fetchWithAuth(`${API_URL}/v1/cliente`);
  if (!res.ok) throw new Error('Erro ao buscar clientes');
  let result = await res.json();

  if (filtros.busca) {
    const q = filtros.busca.toLowerCase();
    result = result.filter(c =>
      (c.nome && c.nome.toLowerCase().includes(q)) ||
      (c.documento && c.documento.includes(q)) ||
      (c.email && c.email.toLowerCase().includes(q))
    );
  }

  if (filtros.status === 'ativo') result = result.filter(c => c.ativo);
  if (filtros.status === 'inativo') result = result.filter(c => c.ativo === false);
  if (filtros.status === 'inadimplente') result = result.filter(c => c.saldo > c.limiteCredito);

  return result;
};

export const getClienteById = async (id) => {
  const clientes = await getClientes();
  return clientes.find(c => c.id === id) || null;
};

export const criarCliente = async (data) => {
  const { empresaId, enderecoId } = await getDefaultIds();

  const dto = {
    nome: data.nome,
    email: data.email,
    documento: data.documento,
    telefone: data.telefone || '',
    saldo: data.saldo || 0,
    limiteCredito: data.limiteCredito || 0,
    enderecoId: enderecoId,
    empresaId: empresaId
  };

  const res = await fetchWithAuth(`${API_URL}/v1/cliente`, {
    method: 'POST',
    body: JSON.stringify(dto)
  });
  if (!res.ok) {
     const txt = await res.text();
     throw new Error(txt || 'Erro ao criar cliente');
  }
  return await res.json(); 
};

export const atualizarCliente = async (id, data) => {
  const dto = { ...data };
  const res = await fetchWithAuth(`${API_URL}/v1/cliente?id=${id}`, {
    method: 'PATCH',
    body: JSON.stringify(dto)
  });
  if (!res.ok) {
    const txt = await res.text();
    throw new Error(txt || 'Erro ao atualizar cliente');
  }
  return { id, ...data };
};

export const toggleClienteStatus = async (id) => {
  const cliente = await getClienteById(id);
  if (!cliente) throw new Error('Cliente não encontrado');

  const endpoint = cliente.ativo ? 'desativar' : 'ativar';
  const method = cliente.ativo ? 'DELETE' : 'POST';

  const res = await fetchWithAuth(`${API_URL}/v1/cliente/${endpoint}?id=${id}`, {
    method: method
  });
  if (!res.ok) throw new Error('Erro ao alterar status do cliente');
  
  cliente.ativo = !cliente.ativo;
  return cliente;
};

/* ─────────── Vendas / Compras ─────────── */

export const registrarVenda = async (clienteId, valor, descricao) => {
  const { empresaId } = await getDefaultIds();
  const dto = { 
    valor: Number(valor), 
    descricao, 
    numeroNota: 'N/A', 
    clienteId, 
    empresaId 
  };

  const res = await fetchWithAuth(`${API_URL}/v1/compras`, {
    method: 'POST',
    body: JSON.stringify(dto)
  });
  
  if (!res.ok) {
    const txt = await res.text();
    throw new Error(txt || 'Erro ao registrar venda');
  }
  
  return await res.json();
};

/* ─────────── Pagamentos ─────────── */

export const registrarPagamento = async (clienteId, valor, observacao) => {
  const { empresaId } = await getDefaultIds();
  const dto = { 
    clienteId, 
    empresaId, 
    valorPagamento: Number(valor), 
    observacao: observacao || '', 
    dataPagamento: new Date().toISOString() 
  };

  const res = await fetchWithAuth(`${API_URL}/v1/pagamentos`, {
    method: 'POST',
    body: JSON.stringify(dto)
  });

  if (!res.ok) {
    const txt = await res.text();
    throw new Error(txt || 'Erro ao registrar pagamento');
  }

  return await res.json();
};

/* ─────────── Histórico / Transações ─────────── */

export const getTransacoes = async (filtros = {}) => {
  let transacoes = [];

  try {
    const resCompras = await fetchWithAuth(`${API_URL}/v1/compras`);
    if (resCompras.ok) {
      const compras = await resCompras.json();
      const formatadas = compras.map(c => ({
        id: c.id,
        tipo: 'venda',
        clienteId: c.clienteId,
        clienteNome: 'Cliente',
        valor: c.valor,
        descricao: c.descricao,
        data: c.criadoEm || new Date().toISOString(),
        status: c.ativo ? 'concluido' : 'estornado'
      }));
      transacoes = [...transacoes, ...formatadas];
    }

    const resPagamentos = await fetchWithAuth(`${API_URL}/v1/pagamentos`);
    if (resPagamentos.ok) {
      const pagamentos = await resPagamentos.json();
      const formPagamentos = pagamentos.map(p => ({
        id: p.id,
        tipo: 'pagamento',
        clienteId: p.clienteId,
        clienteNome: 'Cliente',
        valor: p.valorPagamento,
        descricao: p.observacao || 'Pagamento',
        data: p.dataPagamento || p.criadoEm || new Date().toISOString(),
        status: p.ativo ? 'concluido' : 'estornado'
      }));
      transacoes = [...transacoes, ...formPagamentos];
    }
  } catch (err) {
    console.error('Erro ao buscar transacoes:', err);
  }

  if (filtros.clienteId) {
    transacoes = transacoes.filter(t => t.clienteId === filtros.clienteId);
  }
  if (filtros.tipo && filtros.tipo !== 'todos') {
    transacoes = transacoes.filter(t => t.tipo === filtros.tipo);
  }
  if (filtros.busca) {
    const q = filtros.busca.toLowerCase();
    transacoes = transacoes.filter(t =>
      (t.clienteNome && t.clienteNome.toLowerCase().includes(q)) ||
      (t.descricao && t.descricao.toLowerCase().includes(q))
    );
  }

  if (transacoes.length > 0) {
     try {
       const clientes = await getClientes();
       const mapClientes = {};
       clientes.forEach(c => { mapClientes[c.id] = c.nome; });
       transacoes.forEach(t => {
         t.clienteNome = mapClientes[t.clienteId] || 'Cliente Desconhecido';
       });
     } catch(e){}
  }

  transacoes.sort((a, b) => new Date(b.data) - new Date(a.data));
  return transacoes;
};

export const estornarTransacao = async (id) => {
  try {
    const resCompra = await fetchWithAuth(`${API_URL}/v1/compras/${id}`, { method: 'DELETE' });
    if (resCompra.ok) return { id, status: 'estornado' };
  } catch(e) {}

  try {
    const resPag = await fetchWithAuth(`${API_URL}/v1/pagamentos/${id}`, { method: 'DELETE' });
    if (resPag.ok) return { id, status: 'estornado' };
  } catch(e) {}

  throw new Error('Não foi possível estornar a transação');
};

/* ─────────── Dashboard ─────────── */

export const getDashboardStats = async () => {
  const res = await fetchWithAuth(`${API_URL}/Dashboard/stats`);
  if (!res.ok) throw new Error('Erro ao buscar stats do dashboard');
  return await res.json();
};

export const getDashboardTransactions = async () => {
  const res = await fetchWithAuth(`${API_URL}/Dashboard/transactions`);
  if (!res.ok) throw new Error('Erro ao buscar transacoes do dashboard');
  return await res.json();
};

export const getDashboardChart = async () => {
  const res = await fetchWithAuth(`${API_URL}/Dashboard/chart`);
  if (!res.ok) throw new Error('Erro ao buscar grafico do dashboard');
  return await res.json();
};

/* ─────────── Utilitários ─────────── */

export const formatCurrency = (val) => {
  if (val == null) return 'R$ 0,00';
  return Number(val).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL', minimumFractionDigits: 2 });
}

export const formatDate = (dateStr) => {
  if (!dateStr) return '';
  const d = new Date(dateStr);
  return d.toLocaleDateString('pt-BR', { day: '2-digit', month: '2-digit', year: 'numeric' });
};

export const formatDateTime = (dateStr) => {
  if (!dateStr) return '';
  const d = new Date(dateStr);
  return d.toLocaleDateString('pt-BR', { day: '2-digit', month: '2-digit', year: 'numeric' }) +
    ' ' + d.toLocaleTimeString('pt-BR', { hour: '2-digit', minute: '2-digit' });
};

export const formatCPF = (val) => {
  if (!val) return '';
  const clean = val.replace(/\D/g, '').slice(0, 11);
  return clean
    .replace(/(\d{3})(\d)/, '$1.$2')
    .replace(/(\d{3})(\d)/, '$1.$2')
    .replace(/(\d{3})(\d{1,2})$/, '$1-$2');
};

export const formatPhone = (val) => {
  if (!val) return '';
  const clean = val.replace(/\D/g, '').slice(0, 11);
  if (clean.length <= 10) {
    return clean
      .replace(/(\d{2})(\d)/, '($1) $2')
      .replace(/(\d{4})(\d{1,4})$/, '$1-$2');
  }
  return clean
    .replace(/(\d{2})(\d)/, '($1) $2')
    .replace(/(\d{5})(\d{1,4})$/, '$1-$2');
};

export const formatCEP = (val) => {
  if (!val) return '';
  const clean = val.replace(/\D/g, '').slice(0, 8);
  return clean.replace(/(\d{5})(\d{1,3})$/, '$1-$2');
};
