import { useState, useEffect } from 'react';
import { transactionsApi, personsApi, categoriesApi } from '../services/api';
import type { Transaction, Person, Category } from '../types';

/**
 * Componente para listar e criar transações.
 */
export default function TransactionList() {
  const [transactions, setTransactions] = useState<Transaction[]>([]);
  const [persons, setPersons] = useState<Person[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showForm, setShowForm] = useState(false);
  const [formData, setFormData] = useState({
    description: '',
    value: '',
    type: 'Despesa' as 'Despesa' | 'Receita',
    personId: '',
    categoryId: ''
  });

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      setLoading(true);
      const [transactionsData, personsData, categoriesData] = await Promise.all([
        transactionsApi.getAll(),
        personsApi.getAll(),
        categoriesApi.getAll()
      ]);
      
      setTransactions(transactionsData);
      setPersons(personsData);
      setCategories(categoriesData);
      setError(null);
    } catch (err) {
      setError('Erro ao carregar dados');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!formData.personId || !formData.categoryId) {
      setError('Selecione uma pessoa e uma categoria');
      return;
    }
    
    try {
      await transactionsApi.create({
        description: formData.description,
        value: Number.parseFloat(formData.value),
        type: formData.type,
        personId: formData.personId,
        categoryId: formData.categoryId
      });
      
      setFormData({
        description: '',
        value: '',
        type: 'Despesa',
        personId: '',
        categoryId: ''
      });
      setShowForm(false);
      loadData();
    } catch (err: any) {
      const errorMessage = err.response?.data?.error || 'Erro ao criar transação';
      setError(errorMessage);
      console.error(err);
    }
  };

  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL'
    }).format(value);
  };

  const formatDate = (date: string) => {
    return new Date(date).toLocaleDateString('pt-BR');
  };

  const getFilteredCategories = () => {
    if (!formData.type) return categories;
    
    return categories.filter(cat => 
      cat.purpose === 'Ambas' || 
      cat.purpose === formData.type
    );
  };

  const getTypeColor = (type: string) => {
    return type === 'Receita' ? '#2ecc71' : '#e74c3c';
  };

  const getTypeIcon = (type: string) => {
    return type === 'Receita' ? '🟢' : '🔴';
  };

  const calculateTotals = () => {
    const receitas = transactions
      .filter(t => t.type === 'Receita')
      .reduce((sum, t) => sum + t.value, 0);
    
    const despesas = transactions
      .filter(t => t.type === 'Despesa')
      .reduce((sum, t) => sum + t.value, 0);
    
    const saldo = receitas - despesas;
    
    return { receitas, despesas, saldo };
  };

  const totals = calculateTotals();

  if (loading) return (
    <div style={{ 
      display: 'flex', 
      justifyContent: 'center', 
      alignItems: 'center', 
      height: '200px',
      color: '#7f8c8d',
      fontFamily: 'Segoe UI, Roboto, sans-serif'
    }}>
      <div style={{ textAlign: 'center' }}>
        <div style={{ 
          width: '40px', 
          height: '40px', 
          border: '3px solid #f3f3f3',
          borderTop: '3px solid #3498db',
          borderRadius: '50%',
          animation: 'spin 1s linear infinite',
          margin: '0 auto 15px'
        }}></div>
        Carregando transações...
      </div>
    </div>
  );

  return (
    <div style={{ padding: '20px', fontFamily: 'Segoe UI, Roboto, sans-serif' }}>
      <h2 style={{ color: '#2c3e50', marginBottom: '25px', fontWeight: '600' }}>Transações</h2>
      
      {/* Cards de Resumo */}
      <div style={{ 
        display: 'grid', 
        gridTemplateColumns: 'repeat(3, 1fr)', 
        gap: '15px', 
        marginBottom: '30px' 
      }}>
        <div style={{ 
          backgroundColor: '#f8f9fa', 
          padding: '20px', 
          borderRadius: '12px',
          borderLeft: '4px solid #2ecc71',
          boxShadow: '0 4px 6px rgba(0, 0, 0, 0.05)'
        }}>
          <div style={{ fontSize: '14px', color: '#7f8c8d', marginBottom: '8px' }}>Receitas</div>
          <div style={{ fontSize: '24px', fontWeight: '600', color: '#2ecc71' }}>
            {formatCurrency(totals.receitas)}
          </div>
        </div>
        
        <div style={{ 
          backgroundColor: '#f8f9fa', 
          padding: '20px', 
          borderRadius: '12px',
          borderLeft: '4px solid #e74c3c',
          boxShadow: '0 4px 6px rgba(0, 0, 0, 0.05)'
        }}>
          <div style={{ fontSize: '14px', color: '#7f8c8d', marginBottom: '8px' }}>Despesas</div>
          <div style={{ fontSize: '24px', fontWeight: '600', color: '#e74c3c' }}>
            {formatCurrency(totals.despesas)}
          </div>
        </div>
        
        <div style={{ 
          backgroundColor: '#f8f9fa', 
          padding: '20px', 
          borderRadius: '12px',
          borderLeft: "4px solid '#3498db",
          boxShadow: '0 4px 6px rgba(0, 0, 0, 0.05)'
        }}>
          <div style={{ fontSize: '14px', color: '#7f8c8d', marginBottom: '8px' }}>Saldo</div>
          <div style={{ 
            fontSize: '24px', 
            fontWeight: '600', 
            color: totals.saldo >= 0 ? '#2ecc71' : '#e74c3c'
          }}>
            {formatCurrency(totals.saldo)}
          </div>
        </div>
      </div>
      
      {error && (
        <div style={{ 
          padding: '15px 20px', 
          backgroundColor: '#fdf2f2',
          color: '#e74c3c', 
          marginBottom: '20px',
          borderRadius: '8px',
          borderLeft: '4px solid #e74c3c',
          display: 'flex',
          justifyContent: 'space-between',
          alignItems: 'center'
        }}>
          <span>{error}</span>
          <button 
            onClick={() => setError(null)}
            style={{
              background: 'none',
              border: 'none',
              color: '#e74c3c',
              cursor: 'pointer',
              fontSize: '18px',
              padding: '0',
              width: '24px',
              height: '24px',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              borderRadius: '50%',
              transition: 'background-color 0.2s'
            }}
            onMouseOver={(e) => e.currentTarget.style.backgroundColor = '#f8d7da'}
            onMouseOut={(e) => e.currentTarget.style.backgroundColor = 'transparent'}
          >
            ✕
          </button>
        </div>
      )}
      
      <button 
        onClick={() => setShowForm(!showForm)}
        style={{
          marginBottom: '25px',
          padding: '12px 24px',
          backgroundColor: showForm ? '#e74c3c' : '#3498db',
          color: 'white',
          border: 'none',
          borderRadius: '8px',
          cursor: 'pointer',
          fontSize: '14px',
          fontWeight: '600',
          transition: 'all 0.3s ease',
          boxShadow: '0 4px 6px rgba(50, 50, 93, 0.11), 0 1px 3px rgba(0, 0, 0, 0.08)',
          display: 'flex',
          alignItems: 'center',
          gap: '8px'
        }}
        onMouseOver={(e) => {
          e.currentTarget.style.transform = 'translateY(-2px)';
          e.currentTarget.style.boxShadow = '0 7px 14px rgba(50, 50, 93, 0.1), 0 3px 6px rgba(0, 0, 0, 0.08)';
        }}
        onMouseOut={(e) => {
          e.currentTarget.style.transform = 'translateY(0)';
          e.currentTarget.style.boxShadow = '0 4px 6px rgba(50, 50, 93, 0.11), 0 1px 3px rgba(0, 0, 0, 0.08)';
        }}
      >
        {showForm ? (
          <>
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
              <line x1="18" y1="6" x2="6" y2="18"></line>
              <line x1="6" y1="6" x2="18" y2="18"></line>
            </svg>
            Cancelar
          </>
        ) : (
          <>
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
              <path d="M12 5v14M5 12h14" />
            </svg>
            Nova Transação
          </>
        )}
      </button>

      {/* Formulário de criação */}
      {showForm && (
        <form 
          onSubmit={handleSubmit} 
          style={{ 
            marginBottom: '30px', 
            padding: '25px', 
            border: '1px solid #e1e8ed', 
            borderRadius: '12px',
            backgroundColor: '#f8fafc',
            boxShadow: '0 10px 20px rgba(0, 0, 0, 0.05)'
          }}
        >
          <div style={{ 
            display: 'grid', 
            gridTemplateColumns: 'repeat(2, 1fr)', 
            gap: '20px',
            marginBottom: '25px'
          }}>
            <div>
              <label style={{ display: 'block', marginBottom: '8px', fontWeight: '500', color: '#2c3e50' }}>
                Descrição:
              </label>
              <input
                type="text"
                value={formData.description}
                onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                required
                maxLength={400}
                placeholder="Digite a descrição da transação"
                style={{
                  width: '100%',
                  padding: '12px 16px',
                  border: '2px solid #e1e8ed',
                  borderRadius: '8px',
                  fontSize: '14px',
                  transition: 'border-color 0.3s ease',
                  boxSizing: 'border-box'
                }}
                onFocus={(e) => e.target.style.borderColor = '#3498db'}
                onBlur={(e) => e.target.style.borderColor = '#e1e8ed'}
              />
            </div>
            
            <div>
              <label style={{ display: 'block', marginBottom: '8px', fontWeight: '500', color: '#2c3e50' }}>
                Valor (R$):
              </label>
              <input
                type="number"
                step="0.01"
                min="0.01"
                value={formData.value}
                onChange={(e) => setFormData({ ...formData, value: e.target.value })}
                required
                placeholder="0,00"
                style={{
                  width: '100%',
                  padding: '12px 16px',
                  border: '2px solid #e1e8ed',
                  borderRadius: '8px',
                  fontSize: '14px',
                  transition: 'border-color 0.3s ease',
                  boxSizing: 'border-box'
                }}
                onFocus={(e) => e.target.style.borderColor = '#3498db'}
                onBlur={(e) => e.target.style.borderColor = '#e1e8ed'}
              />
            </div>
            
            <div>
              <label style={{ display: 'block', marginBottom: '8px', fontWeight: '500', color: '#2c3e50' }}>
                Tipo:
              </label>
              <div style={{ display: 'flex', gap: '10px' }}>
                <button
                  type="button"
                  onClick={() => setFormData({ ...formData, type: 'Despesa', categoryId: '' })}
                  style={{
                    flex: 1,
                    padding: '12px 16px',
                    backgroundColor: formData.type === 'Despesa' ? '#e74c3c' : 'white',
                    color: formData.type === 'Despesa' ? 'white' : '#e74c3c',
                    border: `2px solid ${formData.type === 'Despesa' ? '#e74c3c' : '#e1e8ed'}`,
                    borderRadius: '8px',
                    cursor: 'pointer',
                    fontSize: '14px',
                    fontWeight: '600',
                    transition: 'all 0.2s ease',
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                    gap: '8px'
                  }}
                >
                  🔴 Despesa
                </button>
                <button
                  type="button"
                  onClick={() => setFormData({ ...formData, type: 'Receita', categoryId: '' })}
                  style={{
                    flex: 1,
                    padding: '12px 16px',
                    backgroundColor: formData.type === 'Receita' ? '#2ecc71' : 'white',
                    color: formData.type === 'Receita' ? 'white' : '#2ecc71',
                    border: `2px solid ${formData.type === 'Receita' ? '#2ecc71' : '#e1e8ed'}`,
                    borderRadius: '8px',
                    cursor: 'pointer',
                    fontSize: '14px',
                    fontWeight: '600',
                    transition: 'all 0.2s ease',
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                    gap: '8px'
                  }}
                >
                  🟢 Receita
                </button>
              </div>
            </div>
            
            <div>
              <label style={{ display: 'block', marginBottom: '8px', fontWeight: '500', color: '#2c3e50' }}>
                Pessoa:
              </label>
              <select
                value={formData.personId}
                onChange={(e) => setFormData({ ...formData, personId: e.target.value })}
                required
                style={{
                  width: '100%',
                  padding: '12px 16px',
                  border: '2px solid #e1e8ed',
                  borderRadius: '8px',
                  fontSize: '14px',
                  backgroundColor: 'white',
                  cursor: 'pointer',
                  appearance: 'none',
                  backgroundImage: 'url("data:image/svg+xml;charset=UTF-8,%3csvg xmlns=\'http://www.w3.org/2000/svg\' viewBox=\'0 0 24 24\' fill=\'none\' stroke=\'%232c3e50\' stroke-width=\'2\' stroke-linecap=\'round\' stroke-linejoin=\'round\'%3e%3cpolyline points=\'6 9 12 15 18 9\'%3e%3c/polyline%3e%3c/svg%3e")',
                  backgroundRepeat: 'no-repeat',
                  backgroundPosition: 'right 16px center',
                  backgroundSize: '16px',
                  paddingRight: '40px'
                }}
              >
                <option value="">Selecione uma pessoa...</option>
                {persons.map((person) => (
                  <option key={person.id} value={person.id}>
                    {person.name} ({person.age} anos)
                  </option>
                ))}
              </select>
            </div>
            
            <div style={{ gridColumn: 'span 2' }}>
              <label style={{ display: 'block', marginBottom: '8px', fontWeight: '500', color: '#2c3e50' }}>
                Categoria:
              </label>
              <select
                value={formData.categoryId}
                onChange={(e) => setFormData({ ...formData, categoryId: e.target.value })}
                required
                disabled={getFilteredCategories().length === 0}
                style={{
                  width: '100%',
                  padding: '12px 16px',
                  border: '2px solid #e1e8ed',
                  borderRadius: '8px',
                  fontSize: '14px',
                  backgroundColor: 'white',
                  cursor: 'pointer',
                  appearance: 'none',
                  backgroundImage: 'url("data:image/svg+xml;charset=UTF-8,%3csvg xmlns=\'http://www.w3.org/2000/svg\' viewBox=\'0 0 24 24\' fill=\'none\' stroke=\'%232c3e50\' stroke-width=\'2\' stroke-linecap=\'round\' stroke-linejoin=\'round\'%3e%3cpolyline points=\'6 9 12 15 18 9\'%3e%3c/polyline%3e%3c/svg%3e")',
                  backgroundRepeat: 'no-repeat',
                  backgroundPosition: 'right 16px center',
                  backgroundSize: '16px',
                  paddingRight: '40px'
                }}
              >
                <option value="">
                  {getFilteredCategories().length === 0 
                    ? 'Selecione um tipo primeiro' 
                    : 'Selecione uma categoria...'}
                </option>
                {getFilteredCategories().map((category) => (
                  <option key={category.id} value={category.id}>
                    {category.description} ({category.purpose})
                  </option>
                ))}
              </select>
            </div>
          </div>
          
          <div style={{ display: 'flex', gap: '12px' }}>
            <button
              type="submit"
              style={{
                flex: 1,
                padding: '14px 20px',
                backgroundColor: '#2ecc71',
                color: 'white',
                border: 'none',
                borderRadius: '8px',
                cursor: 'pointer',
                fontSize: '15px',
                fontWeight: '600',
                transition: 'all 0.3s ease',
                boxShadow: '0 4px 6px rgba(50, 50, 93, 0.11), 0 1px 3px rgba(0, 0, 0, 0.08)',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                gap: '8px'
              }}
              onMouseOver={(e) => {
                e.currentTarget.style.backgroundColor = '#27ae60';
                e.currentTarget.style.transform = 'translateY(-2px)';
                e.currentTarget.style.boxShadow = '0 7px 14px rgba(50, 50, 93, 0.1), 0 3px 6px rgba(0, 0, 0, 0.08)';
              }}
              onMouseOut={(e) => {
                e.currentTarget.style.backgroundColor = '#2ecc71';
                e.currentTarget.style.transform = 'translateY(0)';
                e.currentTarget.style.boxShadow = '0 4px 6px rgba(50, 50, 93, 0.11), 0 1px 3px rgba(0, 0, 0, 0.08)';
              }}
            >
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                <path d="M12 5v14M5 12h14" />
              </svg>
              Criar Transação
            </button>
            
            <button
              type="button"
              onClick={() => setShowForm(false)}
              style={{
                padding: '14px 24px',
                backgroundColor: 'transparent',
                color: '#7f8c8d',
                border: '2px solid #e1e8ed',
                borderRadius: '8px',
                cursor: 'pointer',
                fontSize: '15px',
                fontWeight: '500',
                transition: 'all 0.3s ease'
              }}
              onMouseOver={(e) => {
                e.currentTarget.style.backgroundColor = '#f8f9fa';
                e.currentTarget.style.borderColor = '#bdc3c7';
                e.currentTarget.style.color = '#2c3e50';
              }}
              onMouseOut={(e) => {
                e.currentTarget.style.backgroundColor = 'transparent';
                e.currentTarget.style.borderColor = '#e1e8ed';
                e.currentTarget.style.color = '#7f8c8d';
              }}
            >
              Cancelar
            </button>
          </div>
        </form>
      )}

      {/* Lista de transações */}
      {transactions.length > 0 ? (
        <div style={{ 
          borderRadius: '12px',
          overflow: 'hidden',
          boxShadow: '0 4px 12px rgba(0, 0, 0, 0.05)'
        }}>
          <table style={{ 
            width: '100%', 
            borderCollapse: 'separate', 
            borderSpacing: '0'
          }}>
            <thead>
              <tr style={{ backgroundColor: '#f1f5f9' }}>
                <th style={{ 
                  padding: '16px 20px', 
                  borderBottom: '2px solid #e1e8ed', 
                  textAlign: 'left',
                  fontWeight: '600',
                  color: '#2c3e50'
                }}>Data</th>
                <th style={{ 
                  padding: '16px 20px', 
                  borderBottom: '2px solid #e1e8ed', 
                  textAlign: 'left',
                  fontWeight: '600',
                  color: '#2c3e50'
                }}>Descrição</th>
                <th style={{ 
                  padding: '16px 20px', 
                  borderBottom: '2px solid #e1e8ed', 
                  textAlign: 'right',
                  fontWeight: '600',
                  color: '#2c3e50'
                }}>Valor</th>
                <th style={{ 
                  padding: '16px 20px', 
                  borderBottom: '2px solid #e1e8ed', 
                  textAlign: 'center',
                  fontWeight: '600',
                  color: '#2c3e50'
                }}>Tipo</th>
                <th style={{ 
                  padding: '16px 20px', 
                  borderBottom: '2px solid #e1e8ed', 
                  textAlign: 'left',
                  fontWeight: '600',
                  color: '#2c3e50'
                }}>Pessoa</th>
                <th style={{ 
                  padding: '16px 20px', 
                  borderBottom: '2px solid #e1e8ed', 
                  textAlign: 'left',
                  fontWeight: '600',
                  color: '#2c3e50'
                }}>Categoria</th>
              </tr>
            </thead>
            <tbody>
              {transactions.map((transaction, index) => (
                <tr 
                  key={transaction.id}
                  style={{ 
                    backgroundColor: index % 2 === 0 ? 'white' : '#f8fafc',
                    transition: 'background-color 0.2s ease'
                  }}
                  onMouseOver={(e) => e.currentTarget.style.backgroundColor = '#f1f5f9'}
                  onMouseOut={(e) => e.currentTarget.style.backgroundColor = index % 2 === 0 ? 'white' : '#f8fafc'}
                >
                  <td style={{ 
                    padding: '14px 20px', 
                    borderBottom: '1px solid #e1e8ed',
                    color: '#7f8c8d'
                  }}>
                    {formatDate(transaction.createdAt)}
                  </td>
                  <td style={{ 
                    padding: '14px 20px', 
                    borderBottom: '1px solid #e1e8ed',
                    color: '#2c3e50',
                    fontWeight: '500'
                  }}>
                    {transaction.description}
                  </td>
                  <td style={{ 
                    padding: '14px 20px', 
                    borderBottom: '1px solid #e1e8ed', 
                    textAlign: 'right',
                    color: getTypeColor(transaction.type),
                    fontWeight: '600',
                    fontSize: '15px'
                  }}>
                    {formatCurrency(transaction.value)}
                  </td>
                  <td style={{ 
                    padding: '14px 20px', 
                    borderBottom: '1px solid #e1e8ed', 
                    textAlign: 'center'
                  }}>
                    <span style={{
                      display: 'inline-flex',
                      alignItems: 'center',
                      gap: '6px',
                      padding: '6px 12px',
                      backgroundColor: transaction.type === 'Receita' ? '#d5f4e6' : '#fadbd8',
                      color: getTypeColor(transaction.type),
                      borderRadius: '20px',
                      fontWeight: '500',
                      fontSize: '13px'
                    }}>
                      {getTypeIcon(transaction.type)} {transaction.type}
                    </span>
                  </td>
                  <td style={{ 
                    padding: '14px 20px', 
                    borderBottom: '1px solid #e1e8ed',
                    color: '#2c3e50'
                  }}>
                    {transaction.personName}
                  </td>
                  <td style={{ 
                    padding: '14px 20px', 
                    borderBottom: '1px solid #e1e8ed',
                    color: '#7f8c8d'
                  }}>
                    {transaction.categoryDescription}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      ) : (
        <div style={{
          textAlign: 'center',
          padding: '60px 20px',
          color: '#7f8c8d',
          backgroundColor: '#f8f9fa',
          borderRadius: '12px',
          marginTop: '20px'
        }}>
          <svg width="64" height="64" viewBox="0 0 24 24" fill="none" stroke="#bdc3c7" strokeWidth="1.5" style={{ marginBottom: '20px' }}>
            <line x1="12" y1="8" x2="12" y2="16"></line>
            <line x1="8" y1="12" x2="16" y2="12"></line>
            <rect x="3" y="3" width="18" height="18" rx="2" ry="2"></rect>
          </svg>
          <h3 style={{ color: '#2c3e50', marginBottom: '10px', fontWeight: '500' }}>Nenhuma transação cadastrada</h3>
          <p style={{ fontSize: '15px', marginTop: '8px', opacity: 0.8, maxWidth: '400px', margin: '0 auto' }}>
            Clique em "Nova Transação" para registrar sua primeira transação financeira.
          </p>
        </div>
      )}

      {/* Adicionar animação CSS para o spinner */}
      <style>{`
        @keyframes spin {
          0% { transform: rotate(0deg); }
          100% { transform: rotate(360deg); }
        }
      `}</style>
    </div>
);
}