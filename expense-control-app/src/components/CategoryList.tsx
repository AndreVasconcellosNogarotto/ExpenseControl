import { useState, useEffect } from 'react';
import { categoriesApi } from '../services/api';
import type { Category } from '../types';

/**
 * Componente para listar e criar categorias.
 */
export default function CategoryList() {
  const [categories, setCategories] = useState<Category[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showForm, setShowForm] = useState(false);
  const [formData, setFormData] = useState({
    description: '',
    purpose: 'Ambas' as 'Despesa' | 'Receita' | 'Ambas'
  });

  useEffect(() => {
    loadCategories();
  }, []);

  const loadCategories = async () => {
    try {
      setLoading(true);
      const data = await categoriesApi.getAll();
      setCategories(data);
      setError(null);
    } catch (err) {
      setError('Erro ao carregar categorias');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    try {
      await categoriesApi.create({
        description: formData.description,
        purpose: formData.purpose
      });
      
      setFormData({ description: '', purpose: 'Ambas' });
      setShowForm(false);
      loadCategories();
    } catch (err) {
      setError('Erro ao criar categoria');
      console.error(err);
    }
  };

  const getPurposeLabel = (purpose: string) => {
    switch(purpose) {
      case 'Despesa': return '🔴 Despesa';
      case 'Receita': return '🟢 Receita';
      case 'Ambas': return '🟡 Ambas';
      default: return purpose;
    }
  };

  if (loading) return <div>Carregando...</div>;
  if (error) return <div style={{ color: 'red' }}>{error}</div>;

  return (
    <div style={{ padding: '20px', fontFamily: 'Segoe UI, Roboto, sans-serif' }}>
      <h2 style={{ color: '#2c3e50', marginBottom: '25px', fontWeight: '600' }}>Categorias</h2>
      
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
          boxShadow: '0 4px 6px rgba(50, 50, 93, 0.11), 0 1px 3px rgba(0, 0, 0, 0.08)'
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
        {showForm ? 'Cancelar' : '+ Nova Categoria'}
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
          <div style={{ marginBottom: '20px' }}>
            <label style={{ display: 'block', marginBottom: '8px', fontWeight: '500', color: '#2c3e50' }}>
              Descrição:
            </label>
            <input
              type="text"
              value={formData.description}
              onChange={(e) => setFormData({ ...formData, description: e.target.value })}
              required
              maxLength={400}
              placeholder="Digite a descrição da categoria"
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
          
          <div style={{ marginBottom: '25px' }}>
            <label style={{ display: 'block', marginBottom: '8px', fontWeight: '500', color: '#2c3e50' }}>
              Finalidade:
            </label>
            <select
              value={formData.purpose}
              onChange={(e) => setFormData({ ...formData, purpose: e.target.value as any })}
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
              <option value="Despesa">🔴 Despesa</option>
              <option value="Receita">🟢 Receita</option>
              <option value="Ambas">🟡 Ambas</option>
            </select>
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
              Criar Categoria
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

      {/* Lista de categorias */}
      <table style={{ 
        width: '100%', 
        borderCollapse: 'separate', 
        borderSpacing: '0',
        borderRadius: '12px',
        overflow: 'hidden',
        boxShadow: '0 4px 12px rgba(0, 0, 0, 0.05)'
      }}>
        <thead>
          <tr style={{ backgroundColor: '#f1f5f9' }}>
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
              textAlign: 'center',
              fontWeight: '600',
              color: '#2c3e50'
            }}>Finalidade</th>
          </tr>
        </thead>
        <tbody>
          {categories.map((category, index) => (
            <tr 
              key={category.id}
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
                color: '#2c3e50'
              }}>{category.description}</td>
              <td style={{ 
                padding: '14px 20px', 
                borderBottom: '1px solid #e1e8ed', 
                textAlign: 'center',
                fontWeight: '500'
              }}>
                {getPurposeLabel(category.purpose)}
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      {categories.length === 0 && (
        <div style={{
          textAlign: 'center',
          padding: '40px 20px',
          color: '#7f8c8d',
          backgroundColor: '#f8f9fa',
          borderRadius: '12px',
          marginTop: '20px'
        }}>
          <svg width="48" height="48" viewBox="0 0 24 24" fill="none" stroke="#bdc3c7" strokeWidth="1" style={{ marginBottom: '16px' }}>
            <rect x="3" y="3" width="18" height="18" rx="2" ry="2"></rect>
            <line x1="9" y1="9" x2="15" y2="15"></line>
            <line x1="15" y1="9" x2="9" y2="15"></line>
          </svg>
          <p style={{ fontSize: '16px', margin: 0 }}>Nenhuma categoria cadastrada</p>
          <p style={{ fontSize: '14px', marginTop: '8px', opacity: 0.8 }}>Clique em "Nova Categoria" para adicionar uma</p>
        </div>
      )}
    </div>
  );
}