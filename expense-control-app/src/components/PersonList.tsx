import { useState, useEffect } from 'react';
import { personsApi } from '../services/api';
import type { Person } from '../types';

/**
 * Componente para listar e gerenciar pessoas.
 * Exibe lista de pessoas com opções de criar, editar e deletar.
 */
export default function PersonList() {
  const [persons, setPersons] = useState<Person[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showForm, setShowForm] = useState(false);
  const [formData, setFormData] = useState({ name: '', age: '' });

  // Carrega lista de pessoas ao montar o componente
  useEffect(() => {
    loadPersons();
  }, []);

  const loadPersons = async () => {
    try {
      setLoading(true);
      const data = await personsApi.getAll();
      setPersons(data);
      setError(null);
    } catch (err) {
      setError('Erro ao carregar pessoas');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    try {
      await personsApi.create({
        name: formData.name,
        age: Number.parseInt(formData.age)
      });
      
      // Limpa formulário e recarrega lista
      setFormData({ name: '', age: '' });
      setShowForm(false);
      loadPersons();
    } catch (err) {
      setError('Erro ao criar pessoa');
      console.error(err);
    }
  };

  const handleDelete = async (id: string) => {
    if (!globalThis.confirm('Tem certeza que deseja excluir esta pessoa? Todas as transações associadas serão removidas permanentemente.')) {
      return;
    }

    try {
      await personsApi.delete(id);
      loadPersons();
    } catch (err) {
      setError('Erro ao deletar pessoa');
      console.error(err);
    }
  };

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
        Carregando pessoas...
      </div>
    </div>
  );
  
  if (error) return (
    <div style={{ 
      padding: '20px', 
      color: '#e74c3c', 
      backgroundColor: '#fdf2f2',
      borderLeft: '4px solid #e74c3c',
      borderRadius: '4px',
      margin: '20px',
      fontFamily: 'Segoe UI, Roboto, sans-serif'
    }}>
      {error}
    </div>
  );

  return (
    <div style={{ padding: '20px', fontFamily: 'Segoe UI, Roboto, sans-serif' }}>
      <h2 style={{ color: '#2c3e50', marginBottom: '25px', fontWeight: '600' }}>Pessoas</h2>
      
      <button 
        onClick={() => setShowForm(!showForm)}
        onMouseOver={(e) => {
          e.currentTarget.style.transform = 'translateY(-2px)';
          e.currentTarget.style.boxShadow = '0 7px 14px rgba(50, 50, 93, 0.1), 0 3px 6px rgba(0, 0, 0, 0.08)';
        }}
        onMouseOut={(e) => {
          e.currentTarget.style.transform = 'translateY(0)';
          e.currentTarget.style.boxShadow = '0 4px 6px rgba(50, 50, 93, 0.11), 0 1px 3px rgba(0, 0, 0, 0.08)';
        }}
        onKeyDown={(e) => {
          if (e.key === 'Enter' || e.key === ' ') {
            setShowForm(!showForm);
          }
        }}
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
        aria-label={showForm ? 'Cancelar criação de nova pessoa' : 'Criar nova pessoa'}
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
            Nova Pessoa
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
          aria-labelledby="form-title"
        >
          <h3 id="form-title" style={{ marginBottom: '20px', color: '#2c3e50', fontWeight: '600' }}>
            Adicionar Nova Pessoa
          </h3>
          
          <div style={{ marginBottom: '20px' }}>
            <label 
              htmlFor="person-name"
              style={{ display: 'block', marginBottom: '8px', fontWeight: '500', color: '#2c3e50' }}
            >
              Nome:
            </label>
            <input
              id="person-name"
              type="text"
              value={formData.name}
              onChange={(e) => setFormData({ ...formData, name: e.target.value })}
              required
              maxLength={200}
              placeholder="Digite o nome completo"
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
              aria-required="true"
              aria-describedby="name-help"
            />
            <div id="name-help" style={{ fontSize: '12px', color: '#7f8c8d', marginTop: '6px' }}>
              Nome completo da pessoa (máximo 200 caracteres)
            </div>
          </div>
          
          <div style={{ marginBottom: '25px' }}>
            <label 
              htmlFor="person-age"
              style={{ display: 'block', marginBottom: '8px', fontWeight: '500', color: '#2c3e50' }}
            >
              Idade:
            </label>
            <input
              id="person-age"
              type="number"
              value={formData.age}
              onChange={(e) => setFormData({ ...formData, age: e.target.value })}
              required
              min="1"
              placeholder="Digite a idade"
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
              aria-required="true"
              aria-describedby="age-help"
            />
            <div id="age-help" style={{ fontSize: '12px', color: '#7f8c8d', marginTop: '6px' }}>
              Idade deve ser maior que 0
            </div>
          </div>
          
          <div style={{ display: 'flex', gap: '12px' }}>
            <button
              type="submit"
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
              onKeyDown={(e) => {
                if (e.key === 'Enter' || e.key === ' ') {
                  e.currentTarget.click();
                }
              }}
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
              aria-label="Criar nova pessoa"
            >
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                <path d="M12 5v14M5 12h14" />
              </svg>
              Criar Pessoa
            </button>
            
            <button
              type="button"
              onClick={() => setShowForm(false)}
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
              onKeyDown={(e) => {
                if (e.key === 'Enter' || e.key === ' ') {
                  setShowForm(false);
                }
              }}
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
              aria-label="Cancelar criação de pessoa"
            >
              Cancelar
            </button>
          </div>
        </form>
      )}

      {/* Lista de pessoas */}
      {persons.length > 0 ? (
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
              }}>Nome</th>
              <th style={{ 
                padding: '16px 20px', 
                borderBottom: '2px solid #e1e8ed', 
                textAlign: 'center',
                fontWeight: '600',
                color: '#2c3e50'
              }}>Idade</th>
              <th style={{ 
                padding: '16px 20px', 
                borderBottom: '2px solid #e1e8ed', 
                textAlign: 'center',
                fontWeight: '600',
                color: '#2c3e50'
              }}>Ações</th>
            </tr>
          </thead>
          <tbody>
            {persons.map((person, index) => (
              <tr 
                key={person.id}
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
                  color: '#2c3e50',
                  fontWeight: '500'
                }}>{person.name}</td>
                <td style={{ 
                  padding: '14px 20px', 
                  borderBottom: '1px solid #e1e8ed', 
                  textAlign: 'center',
                  color: '#7f8c8d'
                }}>{person.age} anos</td>
                <td style={{ 
                  padding: '14px 20px', 
                  borderBottom: '1px solid #e1e8ed', 
                  textAlign: 'center'
                }}>
                  <button
                    onClick={() => handleDelete(person.id)}
                    onMouseOver={(e) => {
                      e.currentTarget.style.backgroundColor = '#e74c3c';
                      e.currentTarget.style.color = 'white';
                    }}
                    onMouseOut={(e) => {
                      e.currentTarget.style.backgroundColor = 'transparent';
                      e.currentTarget.style.color = '#e74c3c';
                    }}
                    onKeyDown={(e) => {
                      if (e.key === 'Enter' || e.key === ' ') {
                        handleDelete(person.id);
                      }
                    }}
                    style={{
                      padding: '8px 16px',
                      backgroundColor: 'transparent',
                      color: '#e74c3c',
                      border: '2px solid #e74c3c',
                      borderRadius: '6px',
                      cursor: 'pointer',
                      fontSize: '13px',
                      fontWeight: '600',
                      transition: 'all 0.2s ease',
                      display: 'flex',
                      alignItems: 'center',
                      gap: '6px',
                      margin: '0 auto'
                    }}
                    aria-label={`Deletar pessoa ${person.name}`}
                  >
                    <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                      <polyline points="3 6 5 6 21 6"></polyline>
                      <path d="M19 6l-2 14a2 2 0 0 1-2 2H9a2 2 0 0 1-2-2L5 6"></path>
                      <line x1="10" y1="11" x2="10" y2="17"></line>
                      <line x1="14" y1="11" x2="14" y2="17"></line>
                      <path d="M9 6V4a2 2 0 0 1 2-2h2a2 2 0 0 1 2 2v2"></path>
                    </svg>
                    Deletar
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
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
            <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"></path>
            <circle cx="12" cy="7" r="4"></circle>
          </svg>
          <h3 style={{ color: '#2c3e50', marginBottom: '10px', fontWeight: '500' }}>Nenhuma pessoa cadastrada</h3>
          <p style={{ fontSize: '15px', marginTop: '8px', opacity: 0.8, maxWidth: '400px', margin: '0 auto' }}>
            Clique em "Nova Pessoa" para adicionar pessoas ao sistema.
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