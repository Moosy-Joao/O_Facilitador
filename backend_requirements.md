# Especificações para o Backend - O Facilitador

Este documento detalha os requisitos para a construção do backend do sistema **O Facilitador**. Ele foi elaborado com base na implementação atual do frontend (que consome uma API REST) e especifica os modelos de dados, endpoints necessários, formatos de payload e principais regras de negócio.

---

## 1. Visão Geral

- **URL Base Sugerida (Rotas)**: `/api` (Exemplo: `http://localhost:5000/api`)
- **Autenticação**: O sistema deve utilizar **JWT (JSON Web Token)**. O token deve ser enviado no cabeçalho das requisições privadas no formato: `Authorization: Bearer <token>`.
- **Formato de Dados**: Todas as requisições e respostas devem utilizar JSON (`application/json`).
- **Padrão de Erro**: Quando ocorrer um erro, o backend deve retornar o status HTTP correspondente (400, 401, 403, 404, 500, etc.) e um objeto JSON contendo a mensagem de erro.
  - Exemplo: `{ "message": "Email ou senha inválidos" }`

---

## 2. Modelos de Dados (Entidades)

### 2.1. Usuário (User)
Representa os administradores/funcionários que acessarão o sistema.
- `id` (Numérico ou UUID)
- `name` (String)
- `email` (String, único)
- `password` (String, hash seguro)
- `role` (String - ex: "gerente", "funcionario")

### 2.2. Cliente (Client)
Representa o cliente fiado (comerciante/comprador).
- `id` (Numérico ou UUID)
- `name` (String)
- `cnpj` (String) *(Nota: a validação pode permitir CPF ou CNPJ dependendo do negócio)*
- `phone` (String)
- `address` (String)
- `creditLimit` (Decimal) - Limite de crédito total concedido.
- `balance` (Decimal) - Saldo devedor atual (quanto o cliente deve).
- `active` (Boolean) - Se o cliente está ativo no sistema (Soft Delete/Desativação).
- `blocked` (Boolean) - Se o cliente está bloqueado para novas compras (ex: limite estourado ou inadimplente).
- `createdAt` (DateTime)
- `updatedAt` (DateTime)

### 2.3. Transação (Transaction / Compra / Pagamento)
Pode ser unificada em uma única tabela ou separada (Purchases e Payments). A modelagem sugerida unificada é:
- `id` (Numérico o UUID)
- `clientId` (Relacionamento com Client)
- `type` (Enum/String: `"purchase"` ou `"payment"`)
- `value` (Decimal) - Valor da transação.
- `description` (String) - Utilizado para detalhar a compra.
- `observation` (String) - Utilizado para detalhar o pagamento.
- `reversed` (Boolean) - Flag para indicar se a transação foi estornada/cancelada (Padrão: `false`).
- `date` (DateTime) - Data de ocorrência da transação.

---

## 3. Regras de Negócio Importantes

1. **Atualização do Saldo (Balance)**:
   - Ao criar uma **compra (purchase)**: O valor da compra é **somado** ao `balance` do cliente.
   - Ao executar um **pagamento (payment)**: O valor do pagamento é **subtraído** do `balance`. O `balance` não deve ser menor que `0`.
2. **Estorno (Reverse)**:
   - Ao **estornar uma compra**: O valor é **subtraído** do `balance` atual. A propriedade `reversed` vai para `true`.
   - Ao **estornar um pagamento**: O valor é **somado** ao `balance` atual. A propriedade `reversed` vai para `true`.
3. **Limite de Crédito**:
   - Não permitir registrar uma nova compra se tentar exceder o `creditLimit` (ou seja, `balance atual + valor nova compra > creditLimit`).
   - Retornar erro HTTP 400 com mensagem apropriada.
4. **Clientes Bloqueados (`blocked = true`)**:
   - Um cliente bloqueado **não pode** realizar novas compras.
   - Caso um cliente bloqueado quite sua dívida (`balance` atinge `0`), o sistema deve verificar se o bloqueio deve ser retirado automaticamente (regra opcional).
5. **Soft Delete / Cliente Ativo (`active`)**:
   - Clientes inativos não devem aparecer em listagens de cadastro convencionais ou estatísticas vitais, apenas em históricos se necessário.

---

## 4. Endpoints da API (Rotas)

Abaixo estão as rotas exatas que o Frontend está consumindo.

### 4.1. Autenticação (Aberto)

#### **Login**
- **Rota**: `POST /auth/login`
- **Payload Request**:
  ```json
  {
    "email": "usuario@email.com",
    "password": "senha"
  }
  ```
- **Payload Response** (Sucesso 200 OK):
  ```json
  {
    "token": "eyJhbGciOiJIUzI...",
    "user": { "id": 1, "name": "Nome", "email": "email", "role": "gerente" }
  }
  ```

#### **Registro**
- **Rota**: `POST /auth/register`
- **Payload Request**: `{ "name": "...", "email": "...", "password": "...", "role": "..." }`
- **Payload Response** (Sucesso 201 Created): Retorna token e usuário, idêntico ao Login.

---

### 4.2. Clientes (Protegido por Token JWT)

#### **Listar Todos os Clientes**
- **Rota**: `GET /clients`
- **Payload Response** (200 OK): Array de objetos Client.
  ```json
  [
    {
      "id": 1,
      "name": "Maria Silva",
      "cnpj": "123.456.789-00",
      "phone": "(44) 99999-1234",
      "address": "Rua, Numero",
      "creditLimit": 500.0,
      "balance": 150.0,
      "active": true,
      "blocked": false
    }
  ]
  ```

#### **Buscar Cliente por ID**
- **Rota**: `GET /clients/:id`
- **Payload Response** (200 OK): Objeto Client.

#### **Criar Cliente**
- **Rota**: `POST /clients`
- **Payload Request**:
  ```json
  {
    "name": "Maria Silva",
    "cnpj": "123.456.789-00",
    "phone": "(44) 99999-1234",
    "address": "Rua...",
    "creditLimit": 500.0
  }
  ```
- **Ação no servidor**: Inserir no DB. Inicializar `balance` como `0`, `active` como `true`, e `blocked` como `false`.

#### **Atualizar Cliente**
- **Rota**: `PUT /clients/:id`
- **Payload Request**: Campos a serem atualizados (ex: name, cnpj, address, creditLimit, blocked).
- **Payload Response** (200 OK): Cliente atualizado.

#### **Ativar / Inativar Cliente**
- **Rota**: `PATCH /clients/:id/toggle-active`
- **Payload Response** (200 OK): Inverte o valor booleano atual da propriedade `active` do cliente no banco. Retorna o cliente atualizado.

---

### 4.3. Compras (Purchases) (Protegido por Token)

#### **Listar Compras de um Cliente**
- **Rota**: `GET /clients/:clientId/purchases`
- **Payload Response** (200 OK):
  ```json
  [
    {
      "id": 1,
      "clientId": 1,
      "type": "purchase",
      "value": 85.0,
      "description": "Compras do mês",
      "reversed": false,
      "date": "2026-03-20T10:30:00Z"
    }
  ]
  ```

#### **Registrar Compra**
- **Rota**: `POST /clients/:clientId/purchases`
- **Payload Request**:
  ```json
  {
    "value": 85.0,
    "description": "Compras do mês"
  }
  ```
- **Ação no servidor**: Validar bloqueio do cliente e limite de crédito. Se passar, inserir compra, somar o `value` ao `balance` do cliente.

#### **Estornar Compra**
- **Rota**: `POST /clients/:clientId/purchases/:purchaseId/reverse`
- **Ação no servidor**: Marcar a compra como `reversed: true`. Subtrair valor do `balance` do cliente. Não pode apagar o registro físico para manter a auditoria.

---

### 4.4. Pagamentos (Payments) (Protegido por Token)

#### **Listar Pagamentos de um Cliente**
- **Rota**: `GET /clients/:clientId/payments`
- **Payload Response** (200 OK): Mesma estrutura das compras, mas com `type`: `"payment"` e campo `observation` em vez de `description`.

#### **Registrar Pagamento**
- **Rota**: `POST /clients/:clientId/payments`
- **Payload Request**:
  ```json
  {
    "value": 50.0,
    "observation": "Pagamento parcial"
  }
  ```
- **Ação no servidor**: Inserir pagamento, subtrair `value` do `balance` do cliente.

#### **Estornar Pagamento**
- **Rota**: `POST /clients/:clientId/payments/:paymentId/reverse`
- **Ação no servidor**: Marcar pagamento como `reversed: true`. Somar valor ao `balance` do cliente (pois o pagamento foi cancelado, a dívida volta).

---

### 4.5. Dashboard (Opcional, mas Altamente Recomendado)

Apesar de poder ser calculado no frontend buscando todos os clientes e compras, por motivos de performance, sugere-se uma rota no backend responsável por agregar os dados do dashboard.

#### **Resumo do Dashboard**
- **Rota**: `GET /dashboard/summary`
- **Payload Response** (200 OK):
  ```json
  {
    "totalClients": 45,
    "totalDebtors": 12,
    "totalBlocked": 3,
    "totalReceivable": 2450.50,
    "todayPurchases": 350.0,
    "todayPayments": 120.0,
    "recentDebtors": [ /* array com top 5 ou mais recentes clientes com balance > 0 */ ],
    "recentTransactions": [ /* array com as ultimas 5 ou 10 transações globais não estornadas */ ]
  }
  ```
*(Se implementado pelo backend, a requisição no frontend na pasta API poderá referenciar esta rota explicitamente)*.
