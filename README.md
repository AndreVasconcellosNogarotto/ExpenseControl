# Sistema de Controle de Gastos Residenciais - Frontend

## 📋 Visão Geral

Sistema desenvolvido para controle de gastos residenciais, permitindo gerenciar pessoas, categorias e transações financeiras (receitas e despesas).

### Frontend
- **React 18** com **TypeScript**
- **Axios** - Requisições HTTP
- **React Router** - Roteamento
- **CSS Modules** ou **Tailwind CSS** - Estilização

### Tecnologia Utilizada

- [Node.js 18+](https://nodejs.org/)
- [Visual code](https://code.visualstudio.com/download)

## 🏗️ Arquitetura - Componentes

frontend/src/
├── components/
│   ├── PersonList.tsx          // CRUD de pessoas
│   ├── CategoryList.tsx        // CRUD de categorias
│   ├── TransactionList.tsx     // CRUD de transações
│   └── FinancialSummary.tsx    // Resumo consolidado
├── services/
│   └── api.ts                  // Axios configuration
└── App.tsx                     // Roteamento

### 1.Executando o projeto localmente

```bash
cd frontend/expense-control-app
npm install
npm run dev
```

O frontend estará disponível em: `http://localhost:5173`


### 2. Padrões Utilizados
- ✅ Componentização
- ✅ TypeScript para type safety
- ✅ Hooks customizados
- ✅ Gerenciamento de estado
- ✅ Validação de formulários
- ✅ Feedback visual para usuário (loading, erros, sucesso)
- ✅ Responsividade

## 📄 Licença

Este projeto foi desenvolvido como teste técnico para processo seletivo.

## 👤 Autor

Andre - Backend .NET Developer

## 🔗 Links Úteis

- [React Documentation](https://react.dev/)
- [TypeScript Documentation](https://www.typescriptlang.org/)
