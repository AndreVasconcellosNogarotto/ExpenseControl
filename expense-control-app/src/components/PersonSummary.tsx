import { useState, useEffect } from 'react';
import { personsApi } from '../services/api';
import type { PersonSummaryResponse } from '../types';

/**
 * Componente para exibir resumo financeiro de todas as pessoas.
 * Mostra receitas, despesas e saldo de cada pessoa, além dos totais gerais.
 */
export default function PersonSummary() {
  const [summary, setSummary] = useState<PersonSummaryResponse | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    loadSummary();
  }, []);

  const loadSummary = async () => {
    try {
      setLoading(true);
      const data = await personsApi.getSummary();
      setSummary(data);
      setError(null);
    } catch (err) {
      setError('Erro ao carregar resumo');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  // Formata valor monetário
  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL'
    }).format(value);
  };

  if (loading) return (
    <div style={{ 
      display: 'flex', 
      justifyContent: 'center', 
      alignItems: 'center', 
      height: '300px',
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
        Carregando resumo financeiro...
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
  
  if (!summary || summary.persons.length === 0) return (
    <div style={{
      textAlign: 'center',
      padding: '60px 20px',
      color: '#7f8c8d',
      backgroundColor: '#f8f9fa',
      borderRadius: '12px',
      margin: '20px',
      fontFamily: 'Segoe UI, Roboto, sans-serif'
    }}>
      <svg width="64" height="64" viewBox="0 0 24 24" fill="none" stroke="#bdc3c7" strokeWidth="1.5" style={{ marginBottom: '20px' }}>
        <path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"></path>
        <circle cx="9" cy="7" r="4"></circle>
        <path d="M23 21v-2a4 4 0 0 0-3-3.87"></path>
        <path d="M16 3.13a4 4 0 0 1 0 7.75"></path>
      </svg>
      <h3 style={{ color: '#2c3e50', marginBottom: '10px', fontWeight: '500' }}>Nenhum dado disponível</h3>
      <p style={{ fontSize: '15px', marginTop: '8px', opacity: 0.8, maxWidth: '400px', margin: '0 auto' }}>
        Cadastre pessoas e transações para visualizar o resumo financeiro.
      </p>
    </div>
  );

  return (
    <div style={{ padding: '20px', fontFamily: 'Segoe UI, Roboto, sans-serif' }}>
      <div style={{ 
        display: 'flex', 
        justifyContent: 'space-between', 
        alignItems: 'center',
        marginBottom: '25px' 
      }}>
        <h2 style={{ color: '#2c3e50', fontWeight: '600' }}>Resumo Financeiro por Pessoa</h2>
        <button 
          onClick={loadSummary}
          style={{
            padding: '10px 20px',
            backgroundColor: '#3498db',
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
          <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
            <path d="M23 4v6h-6"></path>
            <path d="M1 20v-6h6"></path>
            <path d="M3.51 9a9 9 0 0 1 14.85-3.36L23 10M1 14l4.64 4.36A9 9 0 0 0 20.49 15"></path>
          </svg>
          Atualizar
        </button>
      </div>

      {/* Cards de Resumo Geral */}
      <div style={{ 
        display: 'grid', 
        gridTemplateColumns: 'repeat(4, 1fr)', 
        gap: '15px', 
        marginBottom: '30px' 
      }}>
        <div style={{ 
          backgroundColor: '#f8f9fa', 
          padding: '20px', 
          borderRadius: '12px',
          borderLeft: '4px solid #3498db',
          boxShadow: '0 4px 6px rgba(0, 0, 0, 0.05)'
        }}>
          <div style={{ fontSize: '14px', color: '#7f8c8d', marginBottom: '8px' }}>Total Pessoas</div>
          <div style={{ fontSize: '24px', fontWeight: '600', color: '#2c3e50' }}>
            {summary.persons.length}
          </div>
        </div>
        
        <div style={{ 
          backgroundColor: '#f8f9fa', 
          padding: '20px', 
          borderRadius: '12px',
          borderLeft: '4px solid #2ecc71',
          boxShadow: '0 4px 6px rgba(0, 0, 0, 0.05)'
        }}>
          <div style={{ fontSize: '14px', color: '#7f8c8d', marginBottom: '8px' }}>Receitas Totais</div>
          <div style={{ fontSize: '24px', fontWeight: '600', color: '#2ecc71' }}>
            {formatCurrency(summary.totalIncome)}
          </div>
        </div>
        
        <div style={{ 
          backgroundColor: '#f8f9fa', 
          padding: '20px', 
          borderRadius: '12px',
          borderLeft: "4px solid #e74c3c",
          boxShadow: '0 4px 6px rgba(0, 0, 0, 0.05)'
        }}>
          <div style={{ fontSize: '14px', color: '#7f8c8d', marginBottom: '8px' }}>Despesas Totais</div>
          <div style={{ fontSize: '24px', fontWeight: '600', color: '#e74c3c' }}>
            {formatCurrency(summary.totalExpense)}
          </div>
        </div>
        
        <div style={{ 
          backgroundColor: '#f8f9fa', 
          padding: '20px', 
          borderRadius: '12px',
          borderLeft: "4px solid #9b59b6",
          boxShadow: '0 4px 6px rgba(0, 0, 0, 0.05)'
        }}>
          <div style={{ fontSize: '14px', color: '#7f8c8d', marginBottom: '8px' }}>Saldo Total</div>
          <div style={{ 
            fontSize: '24px', 
            fontWeight: '600', 
            color: summary.netBalance >= 0 ? '#2ecc71' : '#e74c3c'
          }}>
            {formatCurrency(summary.netBalance)}
          </div>
        </div>
      </div>

      {/* Tabela de Resumo por Pessoa */}
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
              }}>Pessoa</th>
              <th style={{ 
                padding: '16px 20px', 
                borderBottom: '2px solid #e1e8ed', 
                textAlign: 'right',
                fontWeight: '600',
                color: '#2c3e50'
              }}>Receitas</th>
              <th style={{ 
                padding: '16px 20px', 
                borderBottom: '2px solid #e1e8ed', 
                textAlign: 'right',
                fontWeight: '600',
                color: '#2c3e50'
              }}>Despesas</th>
              <th style={{ 
                padding: '16px 20px', 
                borderBottom: '2px solid #e1e8ed', 
                textAlign: 'right',
                fontWeight: '600',
                color: '#2c3e50'
              }}>Saldo</th>
            </tr>
          </thead>
          <tbody>
            {summary.persons.map((person, index) => (
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
                  borderBottom: '1px solid #e1e8ed'
                }}>
                  <div>
                    <div style={{ 
                      color: '#2c3e50',
                      fontWeight: '500',
                      marginBottom: '4px'
                    }}>
                      {person.name}
                    </div>
                    <div style={{ 
                      fontSize: '13px', 
                      color: '#7f8c8d',
                      display: 'flex',
                      alignItems: 'center',
                      gap: '6px'
                    }}>
                      <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="#7f8c8d" strokeWidth="2">
                        <circle cx="12" cy="12" r="10"></circle>
                        <polyline points="12 6 12 12 16 14"></polyline>
                      </svg>
                      {person.age} anos
                    </div>
                  </div>
                </td>
                <td style={{ 
                  padding: '14px 20px', 
                  borderBottom: '1px solid #e1e8ed', 
                  textAlign: 'right',
                  color: '#2ecc71',
                  fontWeight: '500'
                }}>
                  {formatCurrency(person.totalIncome)}
                </td>
                <td style={{ 
                  padding: '14px 20px', 
                  borderBottom: '1px solid #e1e8ed', 
                  textAlign: 'right',
                  color: '#e74c3c',
                  fontWeight: '500'
                }}>
                  {formatCurrency(person.totalExpense)}
                </td>
                <td style={{ 
                  padding: '14px 20px', 
                  borderBottom: '1px solid #e1e8ed', 
                  textAlign: 'right'
                }}>
                  <span style={{
                    display: 'inline-flex',
                    alignItems: 'center',
                    padding: '6px 12px',
                    backgroundColor: person.balance >= 0 ? '#d5f4e6' : '#fadbd8',
                    color: person.balance >= 0 ? '#2ecc71' : '#e74c3c',
                    borderRadius: '20px',
                    fontWeight: '600',
                    fontSize: '14px',
                    minWidth: '100px',
                    justifyContent: 'center',
                    gap: '6px'
                  }}>
                    {person.balance >= 0 ? (
                      <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                        <path d="M5 10l7-7 7 7"></path>
                        <path d="M12 22V4"></path>
                      </svg>
                    ) : (
                      <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                        <path d="M19 14l-7 7-7-7"></path>
                        <path d="M12 22V2"></path>
                      </svg>
                    )}
                    {formatCurrency(person.balance)}
                  </span>
                </td>
              </tr>
            ))}
          </tbody>
          <tfoot>
            <tr style={{ backgroundColor: '#f1f5f9' }}>
              <td style={{ 
                padding: '16px 20px', 
                borderTop: '2px solid #e1e8ed',
                fontWeight: '600',
                color: '#2c3e50'
              }}>
                <div style={{ display: 'flex', alignItems: 'center', gap: '8px' }}>
                  <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="#2c3e50" strokeWidth="2">
                    <path d="M12 2L2 7l10 5 10-5-10-5zM2 17l10 5 10-5M2 12l10 5 10-5"></path>
                  </svg>
                  TOTAIS GERAIS
                </div>
              </td>
              <td style={{ 
                padding: '16px 20px', 
                borderTop: '2px solid #e1e8ed', 
                textAlign: 'right',
                fontWeight: '600',
                color: '#2ecc71'
              }}>
                {formatCurrency(summary.totalIncome)}
              </td>
              <td style={{ 
                padding: '16px 20px', 
                borderTop: '2px solid #e1e8ed', 
                textAlign: 'right',
                fontWeight: '600',
                color: '#e74c3c'
              }}>
                {formatCurrency(summary.totalExpense)}
              </td>
              <td style={{ 
                padding: '16px 20px', 
                borderTop: '2px solid #e1e8ed', 
                textAlign: 'right'
              }}>
                <span style={{
                  display: 'inline-flex',
                  alignItems: 'center',
                  padding: '8px 16px',
                  backgroundColor: summary.netBalance >= 0 ? '#d5f4e6' : '#fadbd8',
                  color: summary.netBalance >= 0 ? '#2ecc71' : '#e74c3c',
                  borderRadius: '20px',
                  fontWeight: '600',
                  fontSize: '15px',
                  minWidth: '120px',
                  justifyContent: 'center',
                  gap: '8px'
                }}>
                  {summary.netBalance >= 0 ? (
                    <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                      <path d="M5 10l7-7 7 7"></path>
                      <path d="M12 22V4"></path>
                    </svg>
                  ) : (
                    <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                      <path d="M19 14l-7 7-7-7"></path>
                      <path d="M12 22V2"></path>
                    </svg>
                  )}
                  {formatCurrency(summary.netBalance)}
                </span>
              </td>
            </tr>
          </tfoot>
        </table>
      </div>

      {/* Estatísticas Adicionais */}
      {summary.persons.length > 1 && (
        <div style={{ 
          display: 'grid', 
          gridTemplateColumns: 'repeat(2, 1fr)', 
          gap: '20px', 
          marginTop: '30px' 
        }}>
          <div style={{ 
            backgroundColor: '#f8f9fa', 
            padding: '20px', 
            borderRadius: '12px',
            border: '1px solid #e1e8ed'
          }}>
            <h3 style={{ color: '#2c3e50', marginBottom: '15px', fontSize: '16px' }}>
              Pessoa com Maior Receita
            </h3>
            {(() => {
              const topPerson = summary.persons.reduce((max, person) => 
                person.totalIncome > max.totalIncome ? person : max
              );
              return (
                <div style={{ display: 'flex', alignItems: 'center', gap: '12px' }}>
                  <div style={{
                    width: '40px',
                    height: '40px',
                    backgroundColor: '#2ecc71',
                    borderRadius: '50%',
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                    color: 'white',
                    fontWeight: '600',
                    fontSize: '14px'
                  }}>
                    {topPerson.name.charAt(0)}
                  </div>
                  <div>
                    <div style={{ fontWeight: '500', color: '#2c3e50' }}>{topPerson.name}</div>
                    <div style={{ fontSize: '14px', color: '#7f8c8d' }}>
                      Receita: <span style={{ color: '#2ecc71', fontWeight: '500' }}>{formatCurrency(topPerson.totalIncome)}</span>
                    </div>
                  </div>
                </div>
              );
            })()}
          </div>
          
          <div style={{ 
            backgroundColor: '#f8f9fa', 
            padding: '20px', 
            borderRadius: '12px',
            border: '1px solid #e1e8ed'
          }}>
            <h3 style={{ color: '#2c3e50', marginBottom: '15px', fontSize: '16px' }}>
              Pessoa com Maior Saldo
            </h3>
            {(() => {
              const topPerson = summary.persons.reduce((max, person) => 
                person.balance > max.balance ? person : max
              );
              return (
                <div style={{ display: 'flex', alignItems: 'center', gap: '12px' }}>
                  <div style={{
                    width: '40px',
                    height: '40px',
                    backgroundColor: topPerson.balance >= 0 ? '#2ecc71' : '#e74c3c',
                    borderRadius: '50%',
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                    color: 'white',
                    fontWeight: '600',
                    fontSize: '14px'
                  }}>
                    {topPerson.name.charAt(0)}
                  </div>
                  <div>
                    <div style={{ fontWeight: '500', color: '#2c3e50' }}>{topPerson.name}</div>
                    <div style={{ fontSize: '14px', color: '#7f8c8d' }}>
                      Saldo: <span style={{ 
                        color: topPerson.balance >= 0 ? '#2ecc71' : '#e74c3c', 
                        fontWeight: '500' 
                      }}>
                        {formatCurrency(topPerson.balance)}
                      </span>
                    </div>
                  </div>
                </div>
              );
            })()}
          </div>
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