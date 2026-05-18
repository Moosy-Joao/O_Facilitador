import express from 'express';
import cors from 'cors';

const app = express();
app.use(cors());
app.use(express.json());

// Delay artificial para simular rede real (300ms)
const delay = (req, res, next) => setTimeout(next, 300);
app.use(delay);

// --- Auth ---
app.post('/api/v1/auth/login', (req, res) => {
  const { email, senha } = req.body;
  if (email === 'admin@teste.com' && senha === 'admin') {
    return res.json({ token: 'mock-jwt-token-12345', user: { id: 1, nome: 'Administrador Mock', email } });
  }
  // Sucesso genérico para facilitar o teste
  res.json({ token: 'mock-jwt-token-99999', user: { id: 99, nome: 'Usuário Teste', email } });
});

// --- Clientes ---
app.get('/api/v1/cliente', (req, res) => {
  res.json([
    { id: '1', nome: 'João Silva', email: 'joao@email.com', documento: '111.222.333-44', telefone: '(11) 99999-9999', saldo: 50, limiteCredito: 1000, ativo: true },
    { id: '2', nome: 'Maria Oliveira', email: 'maria@email.com', documento: '555.666.777-88', telefone: '(11) 88888-8888', saldo: 1500, limiteCredito: 1000, ativo: true },
    { id: '3', nome: 'Carlos Souza', email: 'carlos@email.com', documento: '999.888.777-66', telefone: '(21) 77777-7777', saldo: 500, limiteCredito: 200, ativo: false }
  ]);
});

app.post('/api/v1/cliente', (req, res) => {
  res.status(201).json({ id: String(Date.now()), ...req.body, ativo: true });
});

app.patch('/api/v1/cliente', (req, res) => {
  res.json({ id: req.query.id, ...req.body });
});

app.delete('/api/v1/cliente/desativar', (req, res) => {
  res.json({ success: true, id: req.query.id });
});

app.post('/api/v1/cliente/ativar', (req, res) => {
  res.json({ success: true, id: req.query.id });
});

// --- Compras / Vendas ---
app.get('/api/v1/compras', (req, res) => {
  res.json([
    { id: 'c1', clienteId: '1', valor: 50, descricao: 'Compra mock 1', criadoEm: new Date().toISOString(), ativo: true },
    { id: 'c2', clienteId: '2', valor: 250, descricao: 'Compra mock 2', criadoEm: new Date().toISOString(), ativo: true }
  ]);
});

app.post('/api/v1/compras', (req, res) => {
  res.status(201).json({ id: String(Date.now()), ...req.body, criadoEm: new Date().toISOString(), ativo: true });
});

app.delete('/api/v1/compras/:id', (req, res) => {
  res.json({ success: true });
});

// --- Pagamentos ---
app.get('/api/v1/pagamentos', (req, res) => {
  res.json([
    { id: 'p1', clienteId: '1', valorPagamento: 50, observacao: 'Pagamento mock 1', dataPagamento: new Date().toISOString(), ativo: true }
  ]);
});

app.post('/api/v1/pagamentos', (req, res) => {
  res.status(201).json({ id: String(Date.now()), ...req.body, dataPagamento: new Date().toISOString(), ativo: true });
});

app.delete('/api/v1/pagamentos/:id', (req, res) => {
  res.json({ success: true });
});

// --- Defaults (Empresa/Endereco) ---
app.get('/api/v1/empresa', (req, res) => {
  res.json([{ id: 'empresa-mock-id', nome: 'Empresa Mock' }]);
});

app.get('/api/v1/endereco', (req, res) => {
  res.json([{ id: 'endereco-mock-id', logradouro: 'Rua Mock' }]);
});

// --- Dashboard ---
app.get('/api/Dashboard/stats', (req, res) => {
  res.json({
    totalReceber: 12500.50,
    totalReceberVar: 5.2,
    inadimplentes: 3,
    inadimplentesValor: 1500.00,
    totalClientes: 42,
    novosClientesSemana: 2,
    vendasHoje: 150.00,
    pagamentosHoje: 300.00
  });
});

app.get('/api/Dashboard/transactions', (req, res) => {
  res.json([
    { id: 't1', type: 'venda', cliente: 'João Silva', valor: 150.00, hora: '14:30' },
    { id: 't2', type: 'pagamento', cliente: 'Maria Oliveira', valor: 300.00, hora: '15:00' },
    { id: 't3', type: 'venda', cliente: 'Carlos Souza', valor: 50.00, hora: '09:15' }
  ]);
});

app.get('/api/Dashboard/chart', (req, res) => {
  res.json([
    { value: 100 },
    { value: 200 },
    { value: 150 },
    { value: 400 },
    { value: 300 },
    { value: 500 }
  ]);
});

const PORT = 5238;
app.listen(PORT, () => {
  console.log(`===========================================`);
  console.log(`🚀 Mock Server rodando em http://localhost:${PORT}`);
  console.log(`===========================================`);
  console.log(`Dica: A porta 5238 é a mesma que o Vite espera no api.js`);
});
