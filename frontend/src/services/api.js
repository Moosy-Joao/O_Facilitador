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

const getEmpresaIdFromToken = () => {
  const token = localStorage.getItem('auth_token');
  if (!token) return null;
  try {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));
    const payload = JSON.parse(jsonPayload);
    return payload.empresaId || null;
  } catch (e) {
    console.error('Erro ao decodificar token:', e);
    return null;
  }
};

// Helper para pegar um ID default de Endereço/Empresa para não quebrar a FK
const getDefaultIds = async () => {
  let empresaId = getEmpresaIdFromToken() || '00000000-0000-0000-0000-000000000000';
  let enderecoId = '00000000-0000-0000-0000-000000000000';
  
  try {
    const resEnd = await fetchWithAuth(`${API_URL}/v1/endereco/obter`);
    if (resEnd.ok) {
      const ends = await resEnd.json();
      if (ends && ends.length > 0) enderecoId = ends[0].id;
    }
  } catch (e) {
    console.error(e);
  }

  return { empresaId, enderecoId };
};

/* ─────────── Auth ─────────── */

export const authLogin = async (email, senha) => {
  const res = await fetch(`${API_URL}/v1/autenticacao`, {
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

export const recuperarSenha = async (email) => {
  const res = await fetch(`${API_URL}/v1/usuario/esqueci-senha`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ email })
  });
  if (!res.ok) {
    const errText = await res.text().catch(() => '');
    throw new Error(errText || 'Erro ao processar solicitação.');
  }
  return true;
};

export const resetarSenha = async (token, novaSenha) => {
  const res = await fetch(`${API_URL}/v1/usuario/resetar-senha`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ token, novaSenha })
  });
  if (!res.ok) {
    const errText = await res.text().catch(() => '');
    throw new Error(errText || 'O link de redefinição expirou ou é inválido.');
  }
  return true;
};

/* ─────────── Clientes ─────────── */

export const getClientes = async (filtros = {}) => {
  const res = await fetchWithAuth(`${API_URL}/v1/cliente/obter`);
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
  try {
    const res = await fetchWithAuth(`${API_URL}/v1/cliente/obterporid/${id}`);
    if (res.ok) {
      return await res.json();
    }
  } catch (e) {
    console.warn('Erro ao buscar cliente por ID direto, usando fallback:', e);
  }
  // Fallback: busca na lista geral
  const clientes = await getClientes();
  return clientes.find(c => c.id === id) || null;
};

export const criarEndereco = async (enderecoData) => {
  const dto = {
    pais: enderecoData.pais || 'Brasil',
    estado: enderecoData.estado || '',
    cidade: enderecoData.cidade || '',
    bairro: enderecoData.bairro || '',
    rua: enderecoData.rua || '',
    numero: enderecoData.numero || '',
    cep: enderecoData.cep || ''
  };
  const res = await fetchWithAuth(`${API_URL}/v1/endereco/criar`, {
    method: 'POST',
    body: JSON.stringify(dto)
  });
  if (!res.ok) {
    const txt = await res.text();
    throw new Error(txt || 'Erro ao criar endereço');
  }
  return await res.json(); // retorna { id, pais, estado, ... }
};

export const criarCliente = async (data) => {
  // 1. Criar o endereço do cliente e obter o ID gerado
  let enderecoId;
  try {
    const enderecoCriado = await criarEndereco({
      pais: data.pais || 'Brasil',
      estado: data.estado || '',
      cidade: data.cidade || '',
      bairro: data.bairro || '',
      rua: data.rua || '',
      numero: data.numero || '',
      cep: data.cep || ''
    });
    enderecoId = enderecoCriado.id;
  } catch (e) {
    console.warn('Erro ao criar endereço, usando fallback:', e);
    // Fallback: pega o primeiro endereço disponível
    const { enderecoId: fallbackId } = await getDefaultIds();
    enderecoId = fallbackId;
  }

  const empresaId = getEmpresaIdFromToken() || '00000000-0000-0000-0000-000000000000';

  const dto = {
    nome: data.nome,
    email: data.email,
    documento: data.documento,
    telefone: (data.telefone || '').replace(/\D/g, ''),
    saldo: isNaN(Number(data.saldo)) ? 0 : Number(data.saldo),
    limiteCredito: isNaN(Number(data.limiteCredito)) ? 0 : Number(data.limiteCredito),
    enderecoId: enderecoId,
    empresaId: empresaId
  };

  const res = await fetchWithAuth(`${API_URL}/v1/cliente/criar`, {
    method: 'POST',
    body: JSON.stringify(dto)
  });
  if (!res.ok) {
     const txt = await res.text();
     throw new Error(txt || 'Erro ao criar cliente');
  }
  // O backend retorna true (bool) - tratar adequadamente
  const body = await res.text();
  try { return JSON.parse(body); } catch { return body; }
};

export const atualizarCliente = async (id, data) => {
  const dto = { 
    ...data,
  };

  if (data.telefone !== undefined) {
    const cleanPhone = (data.telefone || '').replace(/\D/g, '');
    dto.telefone = cleanPhone === '' ? null : cleanPhone;
  }

  if (data.saldo !== undefined) {
    dto.saldo = isNaN(Number(data.saldo)) ? 0 : Number(data.saldo);
  }

  if (data.limiteCredito !== undefined) {
    dto.limiteCredito = isNaN(Number(data.limiteCredito)) ? 0 : Number(data.limiteCredito);
  }

  const res = await fetchWithAuth(`${API_URL}/v1/cliente/atualizar/${id}`, {
    method: 'PATCH',
    body: JSON.stringify(dto)
  });
  if (!res.ok) {
    const txt = await res.text();
    throw new Error(txt || 'Erro ao atualizar cliente');
  }
  return { id, ...data };
};

export const atualizarEndereco = async (id, data) => {
  const dto = {
    pais: data.pais || 'Brasil',
    estado: data.estado || '',
    cidade: data.cidade || '',
    bairro: data.bairro || '',
    rua: data.rua || '',
    numero: data.numero || '',
    cep: data.cep || ''
  };
  const res = await fetchWithAuth(`${API_URL}/v1/endereco/atualizar/${id}`, {
    method: 'PATCH',
    body: JSON.stringify(dto)
  });
  if (!res.ok) {
    const txt = await res.text();
    throw new Error(txt || 'Erro ao atualizar endereço');
  }
  return { id, ...data };
};

export const toggleClienteStatus = async (id) => {
  const cliente = await getClienteById(id);
  if (!cliente) throw new Error('Cliente não encontrado');

  const method = cliente.ativo ? 'DELETE' : 'POST';
  const urlSuffix = cliente.ativo ? `desativar/${id}` : `ativar/${id}`;

  const res = await fetchWithAuth(`${API_URL}/v1/cliente/${urlSuffix}`, {
    method: method
  });
  if (!res.ok) throw new Error('Erro ao alterar status do cliente');
  
  cliente.ativo = !cliente.ativo;
  return cliente;
};

export const sincronizarSaldos = async () => {
  // 1. Obter todos os clientes
  const resClientes = await fetchWithAuth(`${API_URL}/v1/cliente/obter`);
  if (!resClientes.ok) throw new Error('Erro ao carregar clientes para sincronização');
  const clientes = await resClientes.json();

  // 2. Obter todas as compras (vendas)
  const resCompras = await fetchWithAuth(`${API_URL}/v1/compras/obter`);
  if (!resCompras.ok) throw new Error('Erro ao carregar compras para sincronização');
  const compras = await resCompras.json();

  // 3. Obter todos os pagamentos
  const resPagamentos = await fetchWithAuth(`${API_URL}/v1/pagamentos/obter`);
  if (!resPagamentos.ok) throw new Error('Erro ao carregar pagamentos para sincronização');
  const pagamentos = await resPagamentos.json();

  // 4. Recalcular e atualizar o saldo de cada cliente no banco de dados
  for (const cliente of clientes) {
    const totalCompras = compras
      .filter(c => c.ativo && c.clienteId === cliente.id)
      .reduce((sum, c) => sum + (c.valor || 0), 0);

    const totalPagamentos = pagamentos
      .filter(p => p.ativo && p.clienteId === cliente.id)
      .reduce((sum, p) => sum + (p.valorPagamento || 0), 0);

    const saldoCorreto = totalCompras - totalPagamentos;

    // Atualiza apenas se houver diferença
    if (Math.abs(cliente.saldo - saldoCorreto) > 0.001) {
      await atualizarCliente(cliente.id, { saldo: saldoCorreto });
    }
  }
  return true;
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

  const res = await fetchWithAuth(`${API_URL}/v1/compras/criar`, {
    method: 'POST',
    body: JSON.stringify(dto)
  });
  
  if (!res.ok) {
    const txt = await res.text();
    throw new Error(txt || 'Erro ao registrar venda');
  }

  // Atualizar saldo do cliente no frontend após venda com sucesso
  try {
    const cliente = await getClienteById(clienteId);
    if (cliente) {
      const novoSaldo = (cliente.saldo || 0) + Number(valor);
      await atualizarCliente(clienteId, { saldo: novoSaldo });
    }
  } catch (err) {
    console.warn('Erro ao atualizar saldo após venda:', err);
  }
  
  const body = await res.text();
  try { return JSON.parse(body); } catch { return body; }
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

  const res = await fetchWithAuth(`${API_URL}/v1/pagamentos/criar`, {
    method: 'POST',
    body: JSON.stringify(dto)
  });

  if (!res.ok) {
    const txt = await res.text();
    throw new Error(txt || 'Erro ao registrar pagamento');
  }

  // Atualizar saldo do cliente no frontend após pagamento com sucesso
  try {
    const cliente = await getClienteById(clienteId);
    if (cliente) {
      const novoSaldo = (cliente.saldo || 0) - Number(valor);
      await atualizarCliente(clienteId, { saldo: novoSaldo });
    }
  } catch (err) {
    console.warn('Erro ao atualizar saldo após pagamento:', err);
  }

  const body = await res.text();
  try { return JSON.parse(body); } catch { return body; }
};

/* ─────────── Histórico / Transações ─────────── */

export const getTransacoes = async (filtros = {}) => {
  let transacoes = [];

  try {
    const resCompras = await fetchWithAuth(`${API_URL}/v1/compras/obter`);
    if (resCompras.ok) {
      const compras = await resCompras.json();
      const formatadas = compras.filter(c => c.ativo).map(c => ({
        id: c.id,
        tipo: 'venda',
        clienteId: c.clienteId,
        clienteNome: 'Cliente',
        valor: c.valor,
        descricao: c.descricao,
        data: c.criadoEm || new Date().toISOString(),
        status: 'concluido'
      }));
      transacoes = [...transacoes, ...formatadas];
    }

    const resPagamentos = await fetchWithAuth(`${API_URL}/v1/pagamentos/obter`);
    if (resPagamentos.ok) {
      const pagamentos = await resPagamentos.json();
      const formPagamentos = pagamentos.filter(p => p.ativo).map(p => ({
        id: p.id,
        tipo: 'pagamento',
        clienteId: p.clienteId,
        clienteNome: 'Cliente',
        valor: p.valorPagamento,
        descricao: p.observacao || 'Pagamento',
        data: p.dataPagamento || p.criadoEm || new Date().toISOString(),
        status: 'concluido'
      }));
      transacoes = [...transacoes, ...formPagamentos];
    }
  } catch (err) {
    console.error('Erro ao buscar transacoes:', err);
  }

  if (transacoes.length > 0) {
     try {
       const clientes = await getClientes();
       const mapClientes = {};
       clientes.forEach(c => { mapClientes[c.id] = c.nome; });

       // Para IDs não encontrados na lista da empresa, busca individualmente como fallback
       const idsDesconhecidos = [...new Set(
         transacoes
           .filter(t => !mapClientes[t.clienteId])
           .map(t => t.clienteId)
       )];

       for (const clienteId of idsDesconhecidos) {
         try {
           const res = await fetchWithAuth(`${API_URL}/v1/cliente/obterporid/${clienteId}`);
           if (res.ok) {
             const cliente = await res.json();
             if (cliente && cliente.nome) mapClientes[clienteId] = cliente.nome;
           }
         } catch (_) { /* ignora erros individuais */ }
       }

       transacoes.forEach(t => {
         t.clienteNome = mapClientes[t.clienteId] || 'Cliente Desconhecido';
       });
     } catch (e) {
       console.error(e);
     }
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

  transacoes.sort((a, b) => new Date(b.data) - new Date(a.data));
  return transacoes;
};

export const estornarTransacao = async (id) => {
  let clienteId = null;

  // Tenta estornar como compra (venda)
  try {
    // Primeiro, busca os dados da compra para obter o clienteId
    const resGet = await fetchWithAuth(`${API_URL}/v1/compras/obterporid/${id}`);
    if (resGet.ok) {
      const compra = await resGet.json();
      clienteId = compra.clienteId;
    }
    const resCompra = await fetchWithAuth(`${API_URL}/v1/compras/desativar/${id}`, { method: 'DELETE' });
    if (resCompra.ok) {
      // Recalcular saldo do cliente após estorno
      if (clienteId) {
        try { await sincronizarSaldos(); } catch (_) {}
      }
      return { id, status: 'estornado' };
    }
  } catch (e) {
    console.error(e);
  }

  // Tenta estornar como pagamento
  try {
    const resGet = await fetchWithAuth(`${API_URL}/v1/pagamentos/obterporid/${id}`);
    if (resGet.ok) {
      const pagamento = await resGet.json();
      clienteId = pagamento.clienteId;
    }
    const resPag = await fetchWithAuth(`${API_URL}/v1/pagamentos/desativar/${id}`, { method: 'DELETE' });
    if (resPag.ok) {
      // Recalcular saldo do cliente após estorno
      if (clienteId) {
        try { await sincronizarSaldos(); } catch (_) {}
      }
      return { id, status: 'estornado' };
    }
  } catch (e) {
    console.error(e);
  }

  throw new Error('Não foi possível estornar a transação');
};

/* ─────────── Dashboard ─────────── */

export const getDashboardStats = async () => {
  const res = await fetchWithAuth(`${API_URL}/v1/paineldados/dados`);
  if (!res.ok) throw new Error('Erro ao buscar stats do dashboard');
  return await res.json();
};

export const getDashboardTransactions = async () => {
  const res = await fetchWithAuth(`${API_URL}/v1/paineldados/transacoes`);
  if (!res.ok) throw new Error('Erro ao buscar transacoes do dashboard');
  return await res.json();
};

export const getDashboardChart = async () => {
  const res = await fetchWithAuth(`${API_URL}/v1/paineldados/grafico`);
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
  const clean = val.replace(/[^\d\w]+/g, '').toUpperCase();
  
  if (clean.length <= 11) {
    return clean
      .slice(0, 11)
      .replace(/(\d{3})(\d)/, '$1.$2')
      .replace(/(\d{3})(\d)/, '$1.$2')
      .replace(/(\d{3})(\d{1,2})$/, '$1-$2');
  } else {
    return clean
      .slice(0, 14)
      .replace(/^([A-Z0-9]{2})([A-Z0-9])/, '$1.$2')
      .replace(/^([A-Z0-9]{2})\.([A-Z0-9]{3})([A-Z0-9])/, '$1.$2.$3')
      .replace(/\.([A-Z0-9]{3})([A-Z0-9])/, '.$1/$2')
      .replace(/\/([A-Z0-9]{4})([A-Z0-9])/, '/$1-$2');
  }
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

export const validateCNPJ = (cnpj) => {
  if (!cnpj) return false;
  const clean = cnpj.replace(/[^A-Za-z0-9]/g, '').toUpperCase();
  if (clean.length !== 14) return false;

  // Se for CNPJ numérico tradicional
  if (/^\d+$/.test(clean)) {
    if (/^(\d)\1{13}$/.test(clean)) return false;
    let size = 12;
    let numbers = clean.substring(0, size);
    const digits = clean.substring(size);
    let sum = 0;
    let pos = size - 7;
    for (let i = size; i >= 1; i--) {
      sum += parseInt(numbers.charAt(size - i)) * pos--;
      if (pos < 2) pos = 9;
    }
    let result = sum % 11 < 2 ? 0 : 11 - (sum % 11);
    if (result !== parseInt(digits.charAt(0))) return false;

    size = 13;
    numbers = clean.substring(0, size);
    sum = 0;
    pos = size - 7;
    for (let i = size; i >= 1; i--) {
      sum += parseInt(numbers.charAt(size - i)) * pos--;
      if (pos < 2) pos = 9;
    }
    result = sum % 11 < 2 ? 0 : 11 - (sum % 11);
    if (result !== parseInt(digits.charAt(1))) return false;
    return true;
  }

  // Se for CNPJ alfanumérico novo
  const charToValue = (c) => {
    if (/[0-9]/.test(c)) return c.charCodeAt(0) - 48;
    return c.charCodeAt(0) - 65 + 10;
  };

  const valueToChar = (valor) => {
    if (valor >= 0 && valor <= 9) return String.fromCharCode(48 + valor);
    return String.fromCharCode(65 + valor - 10);
  };

  const pesos1 = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
  const pesos2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

  const baseCnpj = clean.substring(0, 12);

  let soma = 0;
  for (let i = 0; i < 12; i++) {
    soma += charToValue(baseCnpj[i]) * pesos1[i];
  }
  let resto = soma % 11;
  let digito1 = resto < 2 ? 0 : 11 - resto;
  if (digito1 !== charToValue(clean[12])) return false;

  const baseComPrimeiro = baseCnpj + valueToChar(digito1);
  soma = 0;
  for (let i = 0; i < 13; i++) {
    soma += charToValue(baseComPrimeiro[i]) * pesos2[i];
  }
  resto = soma % 11;
  let digito2 = resto < 2 ? 0 : 11 - resto;
  return digito2 === charToValue(clean[13]);
};

/* ─────────── Sign Up / Cadastro ─────────── */

export const registrarEmpresa = async (data) => {
  const { enderecoId } = await getDefaultIds();
  const dto = {
    nome: data.nomeEmpresa,
    cnpj: data.cnpj,
    email: data.emailEmpresa || data.emailUsuario,
    telefone: data.telefoneEmpresa || '',
    enderecoId: enderecoId
  };
  const res = await fetch(`${API_URL}/v1/empresa/criar`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(dto)
  });
  if (!res.ok) {
    const txt = await res.text();
    throw new Error(txt || 'Erro ao criar empresa');
  }
  return true;
};

export const getEmpresaByCNPJ = async (cnpj) => {
  const res = await fetch(`${API_URL}/v1/empresa/obter`);
  if (!res.ok) throw new Error('Erro ao buscar empresas');
  const empresas = await res.json();
  const cleanCNPJ = cnpj.replace(/\D/g, '');
  return empresas.find(e => e.cnpj && e.cnpj.replace(/\D/g, '') === cleanCNPJ) || null;
};

export const registrarUsuario = async (data) => {
  const dto = {
    nome: data.nomeUsuario,
    email: data.emailUsuario,
    senha: data.senhaUsuario,
    cargo: 0, // Administrador
    empresaId: data.empresaId
  };
  const res = await fetch(`${API_URL}/v1/usuario/criar`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(dto)
  });
  if (!res.ok) {
    const txt = await res.text();
    throw new Error(txt || 'Erro ao criar usuário');
  }
  return await res.json(); // Retorna o token de login para entrar direto!
};

export const getCargoFromToken = () => {
  const token = localStorage.getItem('auth_token');
  if (!token) return null;
  try {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));
    const payload = JSON.parse(jsonPayload);
    return payload.cargo || null;
  } catch (e) {
    console.error('Erro ao decodificar token:', e);
    return null;
  }
};

export const isGerente = () => {
  const cargo = getCargoFromToken();
  return cargo === 'Administrador' || cargo === 'Gerente';
};

export const getUsuarios = async () => {
  const res = await fetchWithAuth(`${API_URL}/v1/usuario/obter`);
  if (!res.ok) throw new Error('Erro ao buscar funcionários');
  return await res.json();
};

export const criarFuncionario = async (data) => {
  const empresaId = getEmpresaIdFromToken();
  const dto = {
    nome: data.nome,
    email: data.email,
    senha: data.senha,
    cargo: Number(data.cargo), // 1 = Gerente, 2 = Funcionario
    empresaId: empresaId
  };
  const res = await fetchWithAuth(`${API_URL}/v1/usuario/criar`, {
    method: 'POST',
    body: JSON.stringify(dto)
  });
  if (!res.ok) {
    const txt = await res.text();
    throw new Error(txt || 'Erro ao criar funcionário');
  }
  return await res.json();
};

export const atualizarFuncionario = async (id, data) => {
  const dto = {
    nome: data.nome,
    email: data.email,
    cargo: data.cargo !== undefined ? Number(data.cargo) : undefined
  };
  if (data.senha) {
    dto.senha = data.senha;
  }
  const res = await fetchWithAuth(`${API_URL}/v1/usuario/atualizar/${id}`, {
    method: 'PATCH',
    body: JSON.stringify(dto)
  });
  if (!res.ok) {
    const txt = await res.text();
    throw new Error(txt || 'Erro ao atualizar funcionário');
  }
  return true;
};

export const toggleFuncionarioStatus = async (id, ativo) => {
  const urlSuffix = ativo ? `ativar/${id}` : `desativar/${id}`;
  const method = ativo ? 'POST' : 'DELETE';
  const res = await fetchWithAuth(`${API_URL}/v1/usuario/${urlSuffix}`, {
    method: method
  });
  if (!res.ok) throw new Error('Erro ao alterar status do funcionário');
  return true;
};
