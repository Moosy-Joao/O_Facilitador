// Mock data for development when backend is not available
const MOCK_DELAY = 400;

const delay = (ms) => new Promise((resolve) => setTimeout(resolve, ms));

let mockClients = [
  {
    id: 1,
    name: 'Maria Silva',
    cnpj: '123.456.789-00',
    phone: '(44) 99999-1234',
    address: 'Rua das Flores, 123 - Maringá/PR',
    creditLimit: 500.0,
    balance: 150.0,
    active: true,
    blocked: false,
    createdAt: '2026-01-15T10:00:00Z',
    updatedAt: '2026-03-20T14:30:00Z',
  },
  {
    id: 2,
    name: 'José Santos',
    cnpj: '987.654.321-00',
    phone: '(44) 98888-5678',
    address: 'Av. Brasil, 456 - Maringá/PR',
    creditLimit: 300.0,
    balance: 300.0,
    active: true,
    blocked: true,
    createdAt: '2026-02-10T08:00:00Z',
    updatedAt: '2026-03-18T09:15:00Z',
  },
  {
    id: 3,
    name: 'Ana Oliveira',
    cnpj: '456.789.123-00',
    phone: '(44) 97777-9012',
    address: 'Rua Paraná, 789 - Maringá/PR',
    creditLimit: 1000.0,
    balance: 0,
    active: true,
    blocked: false,
    createdAt: '2026-01-20T12:00:00Z',
    updatedAt: '2026-03-25T16:00:00Z',
  },
  {
    id: 4,
    name: 'Carlos Pereira',
    cnpj: '321.654.987-00',
    phone: '(44) 96666-3456',
    address: 'Rua XV de Novembro, 321 - Maringá/PR',
    creditLimit: 750.0,
    balance: 420.0,
    active: true,
    blocked: false,
    createdAt: '2026-02-28T09:00:00Z',
    updatedAt: '2026-03-22T11:00:00Z',
  },
  {
    id: 5,
    name: 'Fernanda Costa',
    cnpj: '654.321.987-00',
    phone: '(44) 95555-7890',
    address: 'Rua Maringá, 654 - Maringá/PR',
    creditLimit: 200.0,
    balance: 50.0,
    active: false,
    blocked: false,
    createdAt: '2026-03-01T14:00:00Z',
    updatedAt: '2026-03-10T08:00:00Z',
  },
];

let mockTransactions = [
  { id: 1, clientId: 1, type: 'purchase', value: 85.0, description: 'Compras do mês - alimentos', date: '2026-03-20T10:30:00Z', reversed: false },
  { id: 2, clientId: 1, type: 'purchase', value: 120.0, description: 'Produtos de limpeza', date: '2026-03-18T14:00:00Z', reversed: false },
  { id: 3, clientId: 1, type: 'payment', value: 55.0, observation: 'Pagamento parcial', date: '2026-03-22T09:00:00Z', reversed: false },
  { id: 4, clientId: 2, type: 'purchase', value: 200.0, description: 'Rancho do mês', date: '2026-03-10T11:00:00Z', reversed: false },
  { id: 5, clientId: 2, type: 'purchase', value: 100.0, description: 'Bebidas', date: '2026-03-15T16:00:00Z', reversed: false },
  { id: 6, clientId: 4, type: 'purchase', value: 320.0, description: 'Compras gerais', date: '2026-03-12T08:00:00Z', reversed: false },
  { id: 7, clientId: 4, type: 'payment', value: 100.0, observation: 'Pagamento em dinheiro', date: '2026-03-19T10:00:00Z', reversed: true },
  { id: 8, clientId: 4, type: 'purchase', value: 200.0, description: 'Material escolar dos filhos', date: '2026-03-21T13:00:00Z', reversed: false },
  { id: 9, clientId: 5, type: 'purchase', value: 50.0, description: 'Itens básicos', date: '2026-03-05T07:30:00Z', reversed: false },
];

let nextClientId = 6;
let nextTransactionId = 10;

// === AUTH MOCK ===
export const mockAuthService = {
  login: async (email, password) => {
    await delay(MOCK_DELAY);
    if (email && password) {
      return {
        token: 'mock-jwt-token-12345',
        user: { id: 1, name: 'Seu João', email, role: 'gerente' },
      };
    }
    throw new Error('Email ou senha inválidos');
  },
  register: async (data) => {
    await delay(MOCK_DELAY);
    return {
      token: 'mock-jwt-token-12345',
      user: { id: 2, name: data.name, email: data.email, role: data.role || 'funcionario' },
    };
  },
};

// === CLIENTS MOCK ===
export const mockClientService = {
  getAll: async () => {
    await delay(MOCK_DELAY);
    return [...mockClients];
  },
  getById: async (id) => {
    await delay(MOCK_DELAY);
    const client = mockClients.find((c) => c.id === Number(id));
    if (!client) throw new Error('Cliente não encontrado');
    return { ...client };
  },
  create: async (data) => {
    await delay(MOCK_DELAY);
    const newClient = {
      id: nextClientId++,
      ...data,
      balance: 0,
      active: true,
      blocked: false,
      createdAt: new Date().toISOString(),
      updatedAt: new Date().toISOString(),
    };
    mockClients.push(newClient);
    return { ...newClient };
  },
  update: async (id, data) => {
    await delay(MOCK_DELAY);
    const index = mockClients.findIndex((c) => c.id === Number(id));
    if (index === -1) throw new Error('Cliente não encontrado');
    mockClients[index] = { ...mockClients[index], ...data, updatedAt: new Date().toISOString() };
    return { ...mockClients[index] };
  },
  toggleActive: async (id) => {
    await delay(MOCK_DELAY);
    const index = mockClients.findIndex((c) => c.id === Number(id));
    if (index === -1) throw new Error('Cliente não encontrado');
    mockClients[index].active = !mockClients[index].active;
    mockClients[index].updatedAt = new Date().toISOString();
    return { ...mockClients[index] };
  },
};

// === PURCHASES MOCK ===
export const mockPurchaseService = {
  getByClient: async (clientId) => {
    await delay(MOCK_DELAY);
    return mockTransactions.filter((t) => t.clientId === Number(clientId) && t.type === 'purchase');
  },
  create: async (clientId, data) => {
    await delay(MOCK_DELAY);
    const client = mockClients.find((c) => c.id === Number(clientId));
    if (!client) throw new Error('Cliente não encontrado');
    if (client.blocked) throw new Error('Cliente bloqueado. Não é possível registrar venda.');
    if (client.balance + data.value > client.creditLimit) throw new Error('Limite de crédito excedido');

    const newTx = {
      id: nextTransactionId++,
      clientId: Number(clientId),
      type: 'purchase',
      value: data.value,
      description: data.description,
      date: new Date().toISOString(),
      reversed: false,
    };
    mockTransactions.push(newTx);
    client.balance += data.value;
    client.updatedAt = new Date().toISOString();
    return { ...newTx };
  },
  reverse: async (clientId, purchaseId) => {
    await delay(MOCK_DELAY);
    const tx = mockTransactions.find((t) => t.id === Number(purchaseId) && t.type === 'purchase');
    if (!tx) throw new Error('Compra não encontrada');
    tx.reversed = true;
    const client = mockClients.find((c) => c.id === Number(clientId));
    if (client) client.balance -= tx.value;
    return { ...tx };
  },
};

// === PAYMENTS MOCK ===
export const mockPaymentService = {
  getByClient: async (clientId) => {
    await delay(MOCK_DELAY);
    return mockTransactions.filter((t) => t.clientId === Number(clientId) && t.type === 'payment');
  },
  create: async (clientId, data) => {
    await delay(MOCK_DELAY);
    const client = mockClients.find((c) => c.id === Number(clientId));
    if (!client) throw new Error('Cliente não encontrado');

    const newTx = {
      id: nextTransactionId++,
      clientId: Number(clientId),
      type: 'payment',
      value: data.value,
      observation: data.observation,
      date: new Date().toISOString(),
      reversed: false,
    };
    mockTransactions.push(newTx);
    client.balance = Math.max(0, client.balance - data.value);
    if (client.balance === 0 && client.blocked) client.blocked = false;
    client.updatedAt = new Date().toISOString();
    return { ...newTx };
  },
  reverse: async (clientId, paymentId) => {
    await delay(MOCK_DELAY);
    const tx = mockTransactions.find((t) => t.id === Number(paymentId) && t.type === 'payment');
    if (!tx) throw new Error('Pagamento não encontrado');
    tx.reversed = true;
    const client = mockClients.find((c) => c.id === Number(clientId));
    if (client) client.balance += tx.value;
    return { ...tx };
  },
};

// === DASHBOARD MOCK ===
export const mockDashboardService = {
  getSummary: async () => {
    await delay(MOCK_DELAY);
    const activeClients = mockClients.filter((c) => c.active);
    const debtors = activeClients.filter((c) => c.balance > 0);
    const blocked = activeClients.filter((c) => c.blocked);
    const totalReceivable = debtors.reduce((sum, c) => sum + c.balance, 0);

    const today = new Date().toISOString().split('T')[0];
    const todayTransactions = mockTransactions.filter(
      (t) => t.date.split('T')[0] === today && !t.reversed
    );

    return {
      totalClients: activeClients.length,
      totalDebtors: debtors.length,
      totalBlocked: blocked.length,
      totalReceivable,
      todayPurchases: todayTransactions.filter((t) => t.type === 'purchase').reduce((s, t) => s + t.value, 0),
      todayPayments: todayTransactions.filter((t) => t.type === 'payment').reduce((s, t) => s + t.value, 0),
      recentDebtors: debtors.slice(0, 5),
      recentTransactions: mockTransactions.filter((t) => !t.reversed).slice(-5).reverse(),
    };
  },
};
