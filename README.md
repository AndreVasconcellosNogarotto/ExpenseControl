# Sistema de Controle de Gastos Residenciais

## 📋 Visão Geral

Sistema desenvolvido para controle de gastos residenciais, permitindo gerenciar pessoas, categorias e transações financeiras (receitas e despesas).

## 🏗️ Arquitetura

O projeto segue **Clean Architecture** com separação em 4 camadas:

```
ExpenseControl/
├── backend/
│   ├── src/
│   │   ├── ExpenseControl.API          # Camada de apresentação (Controllers)
│   │   ├── ExpenseControl.Application  # Regras de aplicação (CQRS com MediatR)
│   │   ├── ExpenseControl.Domain       # Entidades e interfaces
│   │   └── ExpenseControl.Infrastructure # Implementação (EF Core, Repositórios)
│   └── ExpenseControl.sln
└── frontend/
    └── expense-control-app             # React + TypeScript
```

## 🚀 Tecnologias Utilizadas

### Backend
- **.NET 8** - Framework principal
- **Entity Framework Core 8** - ORM
- **PostgreSQL** - Banco de dados
- **MediatR** - CQRS pattern
- **FluentValidation** - Validação de dados
- **Swagger/OpenAPI** - Documentação da API

### Frontend
- **React 18** com **TypeScript**
- **Axios** - Requisições HTTP
- **React Router** - Roteamento
- **CSS Modules** ou **Tailwind CSS** - Estilização

## 📦 Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 18+](https://nodejs.org/)
- [PostgreSQL 15+](https://www.postgresql.org/download/)
- Visual Studio 2022 (ou VS Code)
- Git

## ⚙️ Configuração do Ambiente

### 1. Banco de Dados PostgreSQL

```sql
-- Criar banco de dados
CREATE DATABASE expense_control_db;

-- Criar usuário (opcional)
CREATE USER expense_user WITH PASSWORD 'sua_senha_aqui';
GRANT ALL PRIVILEGES ON DATABASE expense_control_db TO expense_user;
```

### 2. Backend (.NET)

#### Configurar Connection String

Edite o arquivo `appsettings.json` na camada API:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=expense_control_db;Username=postgres;Password=sua_senha"
  }
}
```

#### Restaurar Dependências

```bash
cd backend
dotnet restore
```

#### Aplicar Migrations

```bash
cd src/ExpenseControl.API
dotnet ef database update
```

#### Executar o Backend

```bash
dotnet run --project src/ExpenseControl.API
```

A API estará disponível em: `https://localhost:5001` ou `http://localhost:5000`
Swagger UI: `https://localhost:5001/swagger`

### 3. Frontend (React)

```bash
cd frontend/expense-control-app
npm install
npm run dev
```

O frontend estará disponível em: `http://localhost:5173`

## 📚 Funcionalidades Implementadas

### ✅ Cadastro de Pessoas
- Criar, editar, listar e deletar pessoas
- Campos: Nome (máx. 200 chars), Idade
- **Regra de negócio**: Ao deletar uma pessoa, todas suas transações são removidas (cascade delete)

### ✅ Cadastro de Categorias
- Criar e listar categorias
- Campos: Descrição (máx. 400 chars), Finalidade (Despesa/Receita/Ambas)

### ✅ Cadastro de Transações
- Criar e listar transações
- Campos: Descrição (máx. 400 chars), Valor (positivo), Tipo (Despesa/Receita), Pessoa, Categoria
- **Regras de negócio**:
  - Menores de 18 anos podem criar apenas DESPESAS
  - Categoria deve ser compatível com o tipo da transação:
    - Transação de Despesa: categoria com finalidade Despesa ou Ambas
    - Transação de Receita: categoria com finalidade Receita ou Ambas

### ✅ Consulta de Totais por Pessoa
- Lista todas as pessoas com:
  - Total de receitas
  - Total de despesas
  - Saldo (receita - despesa)
- Totais gerais no final da listagem

### ✅ Consulta de Totais por Categoria (Opcional)
- Lista todas as categorias com:
  - Total de receitas
  - Total de despesas
  - Saldo (receita - despesa)
- Totais gerais no final da listagem

## 🔌 Endpoints da API

### Pessoas

```
GET    /api/persons              # Listar todas as pessoas
GET    /api/persons/{id}         # Obter pessoa por ID
POST   /api/persons              # Criar nova pessoa
PUT    /api/persons/{id}         # Atualizar pessoa
DELETE /api/persons/{id}         # Deletar pessoa (e suas transações)
GET    /api/persons/summary      # Resumo financeiro de todas as pessoas
```

### Categorias

```
GET    /api/categories           # Listar todas as categorias
GET    /api/categories/{id}      # Obter categoria por ID
POST   /api/categories           # Criar nova categoria
GET    /api/categories/summary   # Resumo financeiro por categoria (opcional)
```

### Transações

```
GET    /api/transactions         # Listar todas as transações
GET    /api/transactions/{id}    # Obter transação por ID
POST   /api/transactions         # Criar nova transação
```

## 🧪 Exemplos de Requisições

### Criar Pessoa

```json
POST /api/persons
{
  "name": "João Silva",
  "age": 25
}
```

### Criar Categoria

```json
POST /api/categories
{
  "description": "Alimentação",
  "purpose": "Ambas"
}
```

### Criar Transação

```json
POST /api/transactions
{
  "description": "Compra supermercado",
  "value": 150.50,
  "type": "Despesa",
  "personId": "guid-da-pessoa",
  "categoryId": "guid-da-categoria"
}
```

## 🗂️ Estrutura do Código

### Domain (Entidades)

- **Person**: Representa uma pessoa no sistema
- **Category**: Representa uma categoria de transação
- **Transaction**: Representa uma transação financeira
- **Enums**: TransactionType, CategoryPurpose

### Application (CQRS)

#### Commands (Escrita)
- CreatePersonCommand
- UpdatePersonCommand
- DeletePersonCommand
- CreateCategoryCommand
- CreateTransactionCommand

#### Queries (Leitura)
- GetAllPersonsQuery
- GetPersonByIdQuery
- GetPersonSummaryQuery
- GetAllCategoriesQuery
- GetCategorySummaryQuery
- GetAllTransactionsQuery

### Infrastructure

- **DbContext**: ExpenseControlDbContext
- **Repositórios**: Implementação das interfaces de repositório
- **Configurações**: Entity configurations (Fluent API)

## 🔒 Validações Implementadas

### Person
- Nome obrigatório (1-200 caracteres)
- Idade obrigatória (maior que 0)

### Category
- Descrição obrigatória (1-400 caracteres)
- Finalidade obrigatória (Despesa/Receita/Ambas)

### Transaction
- Descrição obrigatória (1-400 caracteres)
- Valor obrigatório (maior que 0)
- Tipo obrigatório (Despesa/Receita)
- PersonId e CategoryId obrigatórios
- Validação de idade para receitas (apenas maiores de 18)
- Validação de compatibilidade categoria x tipo transação

## 📝 Boas Práticas Implementadas

### Backend
- ✅ Clean Architecture com separação de responsabilidades
- ✅ CQRS com MediatR
- ✅ Repository Pattern
- ✅ Dependency Injection
- ✅ FluentValidation para validação de dados
- ✅ DTOs para separar domain de API
- ✅ Async/await para operações assíncronas
- ✅ Tratamento de erros com middleware
- ✅ Documentação com comentários XML
- ✅ Swagger para documentação da API
- ✅ Migrations do Entity Framework

### Frontend
- ✅ Componentização
- ✅ TypeScript para type safety
- ✅ Hooks customizados
- ✅ Gerenciamento de estado
- ✅ Validação de formulários
- ✅ Feedback visual para usuário (loading, erros, sucesso)
- ✅ Responsividade

## 🐛 Tratamento de Erros

A API retorna códigos HTTP apropriados:

- `200 OK` - Sucesso
- `201 Created` - Recurso criado
- `204 No Content` - Sucesso sem conteúdo (Delete)
- `400 Bad Request` - Erro de validação
- `404 Not Found` - Recurso não encontrado
- `500 Internal Server Error` - Erro interno

## 🧹 Limpeza de Dados

Antes de publicar no GitHub:
- ✅ Remover referências a "Maxiprod"
- ✅ Limpar connection strings sensíveis
- ✅ Adicionar .gitignore apropriado
- ✅ Remover comentários de desenvolvimento temporários

## 📄 Licença

Este projeto foi desenvolvido como teste técnico para processo seletivo.

## 👤 Autor

Andre - Backend .NET Developer

## 🔗 Links Úteis

- [Documentação .NET](https://docs.microsoft.com/dotnet/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [MediatR](https://github.com/jbogard/MediatR)
- [FluentValidation](https://fluentvalidation.net/)
- [React Documentation](https://react.dev/)
- [TypeScript Documentation](https://www.typescriptlang.org/)
