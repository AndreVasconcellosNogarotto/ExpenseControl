# 🚀 GUIA DE INÍCIO RÁPIDO - ExpenseControl

## 📋 O que foi criado

Este projeto implementa um **Sistema de Controle de Gastos Residenciais** completo com:

### Backend (.NET 8 + PostgreSQL)
- ✅ Clean Architecture (4 camadas)
- ✅ CQRS com MediatR
- ✅ Entity Framework Core + PostgreSQL
- ✅ FluentValidation
- ✅ Swagger/OpenAPI
- ✅ Repositórios implementados
- ✅ Controllers de Persons (completo)
- ✅ Regras de negócio implementadas

### Frontend (React + TypeScript)
- ✅ Estrutura completa do projeto
- ✅ Componente de lista de pessoas
- ✅ Componente de resumo financeiro
- ✅ Serviço de API com Axios
- ✅ TypeScript types
- ✅ Estilização básica

---

## ⚡ Execução Rápida

### 1️⃣ Backend

```bash
# Navegar para o backend
cd backend

# Restaurar dependências
dotnet restore

# Configurar banco de dados PostgreSQL
# Editar: src/ExpenseControl.API/appsettings.Development.json
# Ajustar a connection string com suas credenciais

# Criar migration inicial
cd src/ExpenseControl.API
dotnet ef migrations add InitialCreate

# Aplicar migration no banco
dotnet ef database update

# Executar API
dotnet run
```

**API disponível em:** https://localhost:5001  
**Swagger UI:** https://localhost:5001/swagger

### 2️⃣ Frontend

```bash
# Navegar para o frontend
cd frontend/expense-control-app

# Instalar dependências
npm install

# Executar aplicação
npm run dev
```

**Frontend disponível em:** http://localhost:5173

---

## 📂 Estrutura do Projeto

```
ExpenseControl/
├── README.md                           # Documentação principal
├── .gitignore                          # Git ignore
│
├── backend/
│   ├── ExpenseControl.sln              # Solution .NET
│   ├── IMPLEMENTATION_GUIDE.md         # Guia de implementação
│   │
│   └── src/
│       ├── ExpenseControl.API/         # Controllers, Program.cs
│       │   ├── Controllers/
│       │   │   └── PersonsController.cs ✅
│       │   ├── Program.cs ✅
│       │   └── appsettings.json ✅
│       │
│       ├── ExpenseControl.Application/ # CQRS, DTOs, Validators
│       │   ├── Commands/
│       │   │   ├── Person/ ✅
│       │   │   └── Transaction/ ✅
│       │   ├── Queries/
│       │   │   └── Person/ ✅
│       │   ├── DTOs/ ✅
│       │   └── Validators/ ✅
│       │
│       ├── ExpenseControl.Domain/      # Entidades, Interfaces
│       │   ├── Entities/ ✅
│       │   ├── Enums/ ✅
│       │   └── Interfaces/ ✅
│       │
│       └── ExpenseControl.Infrastructure/ # DbContext, Repositories
│           ├── Data/
│           │   └── ExpenseControlDbContext.cs ✅
│           ├── Configurations/ ✅
│           └── Repositories/ ✅
│
└── frontend/
    └── expense-control-app/
        ├── package.json ✅
        ├── vite.config.ts ✅
        ├── tsconfig.json ✅
        └── src/
            ├── main.tsx ✅
            ├── App.tsx ✅
            ├── types/index.ts ✅
            ├── services/api.ts ✅
            └── components/
                ├── PersonList.tsx ✅
                └── PersonSummary.tsx ✅
```

---

## 🔧 Configuração do PostgreSQL

### Opção 1: Docker (Recomendado)

```bash
docker run --name expense-postgres \
  -e POSTGRES_PASSWORD=postgres \
  -e POSTGRES_DB=expense_control_db \
  -p 5432:5432 \
  -d postgres:15
```

### Opção 2: Instalação Local

1. Instalar PostgreSQL 15+
2. Criar banco de dados:
```sql
CREATE DATABASE expense_control_db;
```

### Connection String

Editar `backend/src/ExpenseControl.API/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=expense_control_db;Username=postgres;Password=SUA_SENHA"
  }
}
```

---

## 📝 O que ainda precisa ser implementado

### Backend (seguir IMPLEMENTATION_GUIDE.md)

1. **Queries restantes:**
   - GetPersonByIdQuery + Handler
   - GetAllCategoriesQuery + Handler
   - GetCategorySummaryQuery + Handler (opcional)
   - GetAllTransactionsQuery + Handler

2. **Commands restantes:**
   - CreateCategoryCommand + Handler + Validator

3. **Controllers restantes:**
   - CategoriesController
   - TransactionsController

**Tempo estimado:** 2-3 horas (seguindo os exemplos fornecidos)

### Frontend

1. **Componentes adicionais:**
   - CategoryList
   - TransactionList
   - TransactionForm
   - Navegação com React Router

**Tempo estimado:** 3-4 horas

---

## 🧪 Testando a API

### Via Swagger

1. Acessar: https://localhost:5001/swagger
2. Testar endpoints disponíveis

### Via Frontend

1. Executar frontend: `npm run dev`
2. Acessar: http://localhost:5173
3. Usar as telas de Pessoas e Resumo Financeiro

### Via cURL

```bash
# Criar pessoa
curl -X POST https://localhost:5001/api/persons \
  -H "Content-Type: application/json" \
  -d '{"name":"João Silva","age":25}' \
  -k

# Listar pessoas
curl https://localhost:5001/api/persons -k

# Ver resumo
curl https://localhost:5001/api/persons/summary -k
```

---

## ✅ Funcionalidades Implementadas

### Pessoas
- ✅ Criar pessoa
- ✅ Listar todas as pessoas
- ✅ Obter pessoa por ID
- ✅ Atualizar pessoa
- ✅ Deletar pessoa (com cascade delete de transações)
- ✅ Consulta de resumo financeiro

### Categorias
- ⚠️ Estrutura criada, implementação pendente
- Criar categoria
- Listar categorias
- Resumo por categoria (opcional)

### Transações
- ⚠️ Estrutura criada, implementação parcial
- ✅ Commands e Handlers criados
- ✅ Validação de regras de negócio:
  - Menores de 18 anos só podem criar despesas
  - Categoria compatível com tipo de transação
- ⚠️ Controller pendente

---

## 🎯 Regras de Negócio Implementadas

1. **Pessoa**
   - Nome obrigatório (máx 200 chars)
   - Idade obrigatória (> 0)
   - Ao deletar pessoa, todas transações são removidas (cascade)

2. **Categoria**
   - Descrição obrigatória (máx 400 chars)
   - Finalidade: Despesa, Receita ou Ambas

3. **Transação**
   - Descrição obrigatória (máx 400 chars)
   - Valor positivo obrigatório
   - Menores de 18 anos: **só podem criar DESPESAS**
   - Categoria deve ser compatível com tipo:
     - Despesa → categoria com finalidade "Despesa" ou "Ambas"
     - Receita → categoria com finalidade "Receita" ou "Ambas"

---

## 📚 Próximos Passos

1. **Completar implementação do backend**
   - Seguir `IMPLEMENTATION_GUIDE.md`
   - Implementar controllers restantes
   - Testar todas as funcionalidades

2. **Expandir frontend**
   - Criar telas para Categorias
   - Criar telas para Transações
   - Adicionar validações no formulário
   - Melhorar UX com loading states

3. **Melhorias opcionais**
   - Adicionar autenticação
   - Implementar paginação
   - Adicionar filtros e buscas
   - Criar gráficos com Chart.js
   - Implementar testes unitários

4. **Deploy**
   - Configurar CI/CD
   - Deploy no Azure/AWS
   - Configurar PostgreSQL em produção

---

## 🐛 Troubleshooting

### Erro de conexão com PostgreSQL
```
Verificar se PostgreSQL está rodando:
- Windows: services.msc
- Linux/Mac: sudo systemctl status postgresql
```

### Erro de certificado SSL
```
Aceitar certificado de desenvolvimento:
dotnet dev-certs https --trust
```

### Porta já em uso
```
Alterar porta no Properties/launchSettings.json
ou matar processo: 
- Windows: netstat -ano | findstr :5001
- Linux/Mac: lsof -ti:5001 | xargs kill
```

### npm install falha
```
Limpar cache:
npm cache clean --force
rm -rf node_modules package-lock.json
npm install
```

---

## 📞 Suporte

- Documentação .NET: https://docs.microsoft.com/dotnet/
- Documentação React: https://react.dev/
- Entity Framework: https://docs.microsoft.com/ef/core/
- PostgreSQL: https://www.postgresql.org/docs/

---

## 🎉 Parabéns!

Você tem agora uma base sólida de um sistema de controle financeiro seguindo as melhores práticas de desenvolvimento!

**Bom desenvolvimento!** 💻
