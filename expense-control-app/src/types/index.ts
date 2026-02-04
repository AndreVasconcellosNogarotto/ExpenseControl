// Tipos para Person
export interface Person {
  id: string;
  name: string;
  age: number;
  createdAt: string;
  updatedAt?: string;
}

export interface CreatePersonDto {
  name: string;
  age: number;
}

export interface PersonSummary {
  id: string;
  name: string;
  age: number;
  totalIncome: number;
  totalExpense: number;
  balance: number;
}

export interface PersonSummaryResponse {
  persons: PersonSummary[];
  totalIncome: number;
  totalExpense: number;
  netBalance: number;
}

// Tipos para Category
export interface Category {
  id: string;
  description: string;
  purpose: 'Despesa' | 'Receita' | 'Ambas';
  createdAt: string;
  updatedAt?: string;
}

export interface CreateCategoryDto {
  description: string;
  purpose: 'Despesa' | 'Receita' | 'Ambas';
}

// Tipos para Transaction
export interface Transaction {
  id: string;
  description: string;
  value: number;
  type: 'Despesa' | 'Receita';
  personId: string;
  personName: string;
  categoryId: string;
  categoryDescription: string;
  createdAt: string;
  updatedAt?: string;
}

export interface CreateTransactionDto {
  description: string;
  value: number;
  type: 'Despesa' | 'Receita';
  personId: string;
  categoryId: string;
}
