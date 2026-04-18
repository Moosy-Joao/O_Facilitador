# O Facilitador

> **Sistema de crédito para pequenos comerciantes**

---

## 📋 Sumário

- [O que é o Facilitador?](#-o-que-é-o-facilitador)
- [É, Não É, Faz e Não Faz](#-é-não-é-faz-e-não-faz)
- [Objetivos do Produto](#-objetivos-do-produto)
- [Personas](#-personas)
- [Jornada do Usuário](#-jornada-do-usuário)
- [MVP – Mínimo Produto Viável](#-mvp--mínimo-produto-viável)
- [Requisitos Funcionais](#-requisitos-funcionais)
- [Requisitos Não Funcionais](#-requisitos-não-funcionais)
- [Equipe](#-equipe)

---

## 💡 O que é o Facilitador?

O **Facilitador** é um sistema de ERP criado para trazer facilidade de histórico e ferramentas de monitoramento de inadimplências para o pequeno comerciante.

Muitos ainda usam cadernos para registrar dívidas, gerando perda de informações e dificuldade para acompanhar pagamentos. O sistema permite:

- Cadastrar clientes
- Registrar compras
- Registrar pagamentos
- Visualizar quem está devendo

---

## ✅ É, Não É, Faz e Não Faz

| **É** | **Não É** |
|---|---|
| Software de ERP | Financiadora |
| Gerenciador financeiro | Sistema de pagamentos (Pix, cartão) |
| SaaS | Banco digital ou fintech |
| Ferramenta para pequenos comerciantes | |
| Sistema de controle de crédito informal (fiado) | |

| **Faz** | **Não Faz** |
|---|---|
| Cadastrar cliente | Dá acesso ao cliente final |
| Guardar histórico financeiro | Verifica limite automaticamente |
| Gerente define limite de crédito | Processa pagamentos bancários |
| Verifica se o cliente é devedor | |
| Cadastro de usuário | |

---

## 🎯 Objetivos do Produto

1. **Facilitar o pequeno varejista** — Oferecer uma ferramenta simples e acessível para gerenciar crédito e pagamentos no dia a dia do comércio.
2. **Adoção local** — Ser adotado por ao menos 5 varejistas da cidade de Maringá, resolvendo suas necessidades com a ferramenta.
3. **Reduzir inadimplência** — Fornecer visão clara para o comerciante de quem deve, quanto deve e há quanto tempo está sem pagar.

---

## 👤 Personas

### Seu João — Dono do mercadinho
- **Perfil:** 55 anos, dono de mercearia de bairro há 20 anos.
- **Dor:** Perde o controle de quem deve, esquece valores e sofre com inadimplência.
- **Objetivo:** Saber exatamente quem deve e quanto, sem depender de caderno.

### Maria — Cliente do fiado
- **Perfil:** 38 anos, trabalha como diarista, compra no fiado quando precisa.
- **Dor:** Às vezes não sabe exatamente quanto deve na mercearia.
- **Objetivo:** Manter o crédito aberto para emergências.

---

## 🗺️ Jornada do Usuário

```
Login → Tela Inicial → Buscar Cliente → Registrar Dívida → Registrar Pagamento
```

---

## 🚀 MVP – Mínimo Produto Viável

| Módulo | Descrição |
|---|---|
| **Cadastro** | Usuário, cliente e crédito |
| **Rotina de Login** | Sistema seguro de autenticação |
| **Relatórios** | Devedores e pagadores |
| **Pagamentos** | Registro de pagamento |

---

## 📌 Requisitos Funcionais

### Gestão de Clientes
- Cadastrar cliente com nome, CPF e telefone
- Definir limite de crédito ao cadastrar
- Listar, buscar e editar clientes
- Ativar e inativar clientes

### Controle de Crédito
- Registrar venda no fiado (data, valor, descrição)
- Impedir venda quando limite está esgotado
- Impedir venda para clientes bloqueados
- Alterar limite de crédito de um cliente

### Pagamentos
- Registrar pagamento parcial ou total
- Atualizar saldo devedor automaticamente
- Desbloquear cliente automaticamente quando quitar a dívida

### Inadimplência
- Marcar cliente como inadimplente após X dias sem pagamento
- Listar clientes inadimplentes na Tela Inicial
- Permitir bloqueio manual pelo comerciante

### Histórico
- Exibir extrato completo de transações por cliente
- Tela Inicial com: total a receber, clientes inadimplentes, movimentações do dia
- Nenhuma transação pode ser excluída — apenas estornada

---

## 🔧 Requisitos Não Funcionais

| Atributo | Descrição |
|---|---|
| **Usabilidade** | Interface simples e fácil de usar, pensada para comerciantes com pouca familiaridade com tecnologia. Layout adaptável para celular e computador. |
| **Desempenho** | O sistema deve responder rapidamente às ações do usuário e funcionar bem mesmo em dispositivos simples ou internet móvel. |
| **Segurança** | Acesso apenas para usuários cadastrados (login). Senhas armazenadas de forma segura. Diferentes permissões para gerente e funcionário. |
| **Confiabilidade** | Informações financeiras não podem ser perdidas ou apagadas. Todo histórico de transações deve ser preservado. Correções por estorno, mantendo registro. |
| **Manutenibilidade** | Código organizado e bem documentado. Projeto com instruções claras para instalação e execução. |
| **Portabilidade** | Sistema deve funcionar em diferentes sistemas operacionais e ambientes sem alterar o código. |

---

## 👥 Equipe

| Nome | Função |
|---|---|
| **Milena Cruz** | Analista de Projeto |
| **Marcos André** | Back-End |
| **João Marques** | Back-End |
| **Matheus Alende** | Front-End & QA |
| **João Antonio** | Front-End |

---

## Rodando o projeto

### Backend

docker compose up -d

Abra o Swagger em http://localhost:5238/swagger/index.html;

---

## 📄 Licença

Distribuído sob a licença disponível em [LICENSE](LICENSE).
