# Padrão de Commits (Semântico + Emojis)

Este documento define o padrão de mensagens de commit adotado no projeto, usando **commits semânticos** e **emojis** para facilitar a leitura do histórico.

---

## Tipo e descrição

O commit semântico possui elementos estruturais (tipos) que informam a intenção do seu commit a quem utiliza o código.

### Tipos

- **feat** — Inclui um novo recurso (relaciona com **MINOR** do versionamento semântico).
- **fix** — Corrige um problema/bug (relaciona com **PATCH** do versionamento semântico).
- **docs** — Mudanças na documentação (ex.: README). *(não inclui alterações em código)*.
- **test** — Alterações em testes (criar/alterar/remover). *(não inclui alterações em código)*.
- **build** — Modificações em arquivos de build e dependências.
- **perf** — Alterações relacionadas à performance.
- **style** — Formatação (lint, espaços, ponto e vírgula, etc.). *(não inclui alterações em código)*.
- **refactor** — Refatoração sem mudar a funcionalidade.
- **chore** — Tarefas de manutenção (configs, pacotes, gitignore, etc.). *(não inclui alterações em código)*.
- **ci** — Mudanças relacionadas à integração contínua.
- **raw** — Mudanças em arquivos de configuração/dados/parâmetros/features.
- **cleanup** — Limpeza de código (remover trechos desnecessários/comentados).
- **remove** — Exclusão de arquivos/diretórios/funcionalidades obsoletas.

---

## Padrões de emojis

Tabela de referência de emojis com palavra‑chave sugerida quando aplicável:

| Tipo do commit | Emoji | Código | Palavra‑chave |
|---|---:|---|---|
| Acessibilidade | ♿ | `:wheelchair:` |  |
| Adicionando um teste | ✅ | `:white_check_mark:` | `test` |
| Atualizando a versão de um submódulo | ⬆️ | `:arrow_up:` |  |
| Retrocedendo a versão de um submódulo | ⬇️ | `:arrow_down:` |  |
| Adicionando uma dependência | ➕ | `:heavy_plus_sign:` | `build` |
| Alterações de revisão de código | 👌 | `:ok_hand:` | `style` |
| Animações e transições | 💫 | `:dizzy:` |  |
| Bugfix | 🐛 | `:bug:` | `fix` |
| Comentários | 💡 | `:bulb:` | `docs` |
| Commit inicial | 🎉 | `:tada:` | `init` |
| Configuração | 🔧 | `:wrench:` | `chore` |
| Deploy | 🚀 | `:rocket:` |  |
| Documentação | 📚 | `:books:` | `docs` |
| Em progresso | 🚧 | `:construction:` |  |
| Estilização de interface | 💄 | `:lipstick:` | `feat` |
| Infraestrutura | 🧱 | `:bricks:` | `ci` |
| Lista de ideias (tasks) | 🔜 | `:soon:` |  |
| Mover/Renomear | 🚚 | `:truck:` | `chore` |
| Novo recurso | ✨ | `:sparkles:` | `feat` |
| Package.json em JS | 📦 | `:package:` | `build` |
| Performance | ⚡ | `:zap:` | `perf` |
| Refatoração | ♻️ | `:recycle:` | `refactor` |
| Limpeza de Código | 🧹 | `:broom:` | `cleanup` |
| Removendo um arquivo | 🗑️ | `:wastebasket:` | `remove` |
| Removendo uma dependência | ➖ | `:heavy_minus_sign:` | `build` |
| Responsividade | 📱 | `:iphone:` |  |
| Revertendo mudanças | 💥 | `:boom:` | `fix` |
| Segurança | 🔒️ | `:lock:` |  |
| SEO | 🔍️ | `:mag:` |  |
| Tag de versão | 🔖 | `:bookmark:` |  |
| Teste de aprovação | ✔️ | `:heavy_check_mark:` | `test` |
| Testes | 🧪 | `:test_tube:` | `test` |
| Texto | 📝 | `:pencil:` |  |
| Tipagem | 🏷️ | `:label:` |  |
| Tratamento de erros | 🥅 | `:goal_net:` |  |
| Dados | 🗃️ | `:card_file_box:` | `raw` |

---

## Exemplos

| Comando Git | Resultado no GitHub |
|---|---|
| `git commit -m ":tada: Commit inicial"` | 🎉 Commit inicial |
| `git commit -m ":books: docs: Atualização do README"` | 📚 docs: Atualização do README |
| `git commit -m ":bug: fix: Loop infinito na linha 50"` | 🐛 fix: Loop infinito na linha 50 |
| `git commit -m ":sparkles: feat: Página de login"` | ✨ feat: Página de login |
| `git commit -m ":bricks: ci: Modificação no Dockerfile"` | 🧱 ci: Modificação no Dockerfile |
| `git commit -m ":recycle: refactor: Passando para arrow functions"` | ♻️ refactor: Passando para arrow functions |
| `git commit -m ":zap: perf: Melhoria no tempo de resposta"` | ⚡ perf: Melhoria no tempo de resposta |
| `git commit -m ":boom: fix: Revertendo mudanças ineficientes"` | 💥 fix: Revertendo mudanças ineficientes |
| `git commit -m ":lipstick: feat: Estilização CSS do formulário"` | 💄 feat: Estilização CSS do formulário |
| `git commit -m ":test_tube: test: Criando novo teste"` | 🧪 test: Criando novo teste |
| `git commit -m ":bulb: docs: Comentários sobre a função LoremIpsum( )"` | 💡 docs: Comentários sobre a função LoremIpsum( ) |
| `git commit -m ":card_file_box: raw: RAW Data do ano aaaa"` | 🗃️ raw: RAW Data do ano aaaa |
| `git commit -m ":broom: cleanup: Eliminando blocos de código comentados e variáveis não utilizadas na função de validação de formulário"` | 🧹 cleanup: Eliminando blocos de código comentados e variáveis não utilizadas na função de validação de formulário |
| `git commit -m ":wastebasket: remove: Removendo arquivos não utilizados do projeto para manter a organização e atualização contínua"` | 🗑️ remove: Removendo arquivos não utilizados do projeto para manter a organização e atualização contínua |

---

## Principais comandos do Git

- `git clone url-do-repositorio-no-github` — Clona um repositório remoto existente.
- `git init` — Inicializa um novo repositório Git no diretório atual.
- `git add .` — Adiciona todos os arquivos/alterações para a área de stage.
- `git commit -m "mensagem do commit"` — Registra as alterações com uma mensagem.
- `git branch -M main` — Renomeia a branch atual (master) para main.
- `git remote add origin https://github.com/usuario/nome-do-repositorio.git` — Adiciona o remoto `origin`.
- `git push -u origin main` — Envia commits para o remoto e define upstream.
- `git remote add origin git@github.com:usuario/projeto.git` / `git branch -M main` / `git push -u origin main` — Conecta um repo local a um remoto e envia commits iniciais.
- `git fetch` — Busca atualizações do remoto sem integrar.
- `git pull origin main` — Atualiza a branch local com mudanças do remoto (fetch + merge).
- `git push --force-with-lease` — Força push de forma mais segura (evita sobrescrever trabalho alheio).
- `git revert id_do_commit` — Cria um commit que desfaz outro, preservando histórico.
- `git reset --hard id_do_commit` — Volta o repo para um commit (apaga mudanças após ele). *(uso local; para sincronizar remoto, usar force-with-lease)*
- `git commit --amend -m "mensagem_reescrita"` — Reescreve a mensagem do último commit (depois, force-with-lease).
- `git cherry-pick HASH_DO_COMMIT` — Aplica um commit específico em outra branch.
- `git switch <branch>` — Troca de branch (use `git switch -c <branch>` para criar e trocar).