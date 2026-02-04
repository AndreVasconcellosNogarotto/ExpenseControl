import axios from 'axios';
import type {
  Person,
  CreatePersonDto,
  PersonSummaryResponse,
  Category,
  CreateCategoryDto,
  Transaction,
  CreateTransactionDto
} from '../types';

// Com proxy do Vite, NÃO usamos baseURL!
// O Vite vai redirecionar /api/* para http://localhost:51746/api/*
console.log('🔧 Usando proxy do Vite');

const api = axios.create({
  // SEM baseURL - Vite proxy cuida disso!
  headers: {
    'Content-Type': 'application/json',
  },
  timeout: 10000,
});

// Interceptor para debug
api.interceptors.request.use(
  (config) => {
    console.log('📤', config.method?.toUpperCase(), config.url);
    return config;
  }
);

api.interceptors.response.use(
  (response) => {
    console.log('✅', response.status, response.config.url);
    return response;
  },
  (error) => {
    console.error('❌ Erro:', error.message);
    return Promise.reject(error);
  }
);

// ========== PERSONS API ==========

export const personsApi = {
  getAll: async (): Promise<Person[]> => {
    const response = await api.get<Person[]>('/api/Persons');
    return response.data;
  },

  getById: async (id: string): Promise<Person> => {
    const response = await api.get<Person>(`/api/Persons/${id}`);
    return response.data;
  },

  create: async (data: CreatePersonDto): Promise<Person> => {
    const response = await api.post<Person>('/api/Persons', data);
    return response.data;
  },

  update: async (id: string, data: CreatePersonDto): Promise<Person> => {
    const response = await api.put<Person>(`/api/Persons/${id}`, data);
    return response.data;
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/api/Persons/${id}`);
  },

  getSummary: async (): Promise<PersonSummaryResponse> => {
    const response = await api.get<PersonSummaryResponse>('/api/Persons/summary');
    return response.data;
  },
};

// ========== CATEGORIES API ==========

export const categoriesApi = {
  getAll: async (): Promise<Category[]> => {
    const response = await api.get<Category[]>('/api/Categories');
    return response.data;
  },

  create: async (data: CreateCategoryDto): Promise<Category> => {
    const response = await api.post<Category>('/api/Categories', data);
    return response.data;
  },
};

// ========== TRANSACTIONS API ==========

export const transactionsApi = {
  getAll: async (): Promise<Transaction[]> => {
    const response = await api.get<Transaction[]>('/api/Transactions');
    return response.data;
  },

  create: async (data: CreateTransactionDto): Promise<Transaction> => {
    const response = await api.post<Transaction>('/api/Transactions', data);
    return response.data;
  },
};

export default api;