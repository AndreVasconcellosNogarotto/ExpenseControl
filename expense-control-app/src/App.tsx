import { useState } from 'react';
import PersonList from './components/PersonList';
import PersonSummary from './components/PersonSummary';
import CategoryList from './components/CategoryList';
import TransactionList from './components/TransactionList';
import './App.css';

/**
 * Componente principal da aplicação.
 * Gerencia a navegação entre diferentes telas.
 */
function App() {
  const [currentPage, setCurrentPage] = useState<'persons' | 'categories' | 'transactions' | 'summary'>('persons');

  return (
    <div className="App">
      <header style={{
        backgroundColor: '#282c34',
        padding: '20px',
        color: 'white',
        marginBottom: '20px'
      }}>
        <h1>💰 Controle de Gastos Residenciais</h1>
        
        <nav style={{ marginTop: '15px', display: 'flex', gap: '10px', flexWrap: 'wrap' }}>
          <button
            onClick={() => setCurrentPage('persons')}
            style={{
              padding: '10px 20px',
              backgroundColor: currentPage === 'persons' ? '#61dafb' : '#fff',
              border: 'none',
              borderRadius: '5px',
              cursor: 'pointer',
              fontWeight: currentPage === 'persons' ? 'bold' : 'normal'
            }}
          >
            👤 Pessoas
          </button>
          
          <button
            onClick={() => setCurrentPage('categories')}
            style={{
              padding: '10px 20px',
              backgroundColor: currentPage === 'categories' ? '#61dafb' : '#fff',
              border: 'none',
              borderRadius: '5px',
              cursor: 'pointer',
              fontWeight: currentPage === 'categories' ? 'bold' : 'normal'
            }}
          >
            📁 Categorias
          </button>

          <button
            onClick={() => setCurrentPage('transactions')}
            style={{
              padding: '10px 20px',
              backgroundColor: currentPage === 'transactions' ? '#61dafb' : '#fff',
              border: 'none',
              borderRadius: '5px',
              cursor: 'pointer',
              fontWeight: currentPage === 'transactions' ? 'bold' : 'normal'
            }}
          >
            💳 Transações
          </button>
          
          <button
            onClick={() => setCurrentPage('summary')}
            style={{
              padding: '10px 20px',
              backgroundColor: currentPage === 'summary' ? '#61dafb' : '#fff',
              border: 'none',
              borderRadius: '5px',
              cursor: 'pointer',
              fontWeight: currentPage === 'summary' ? 'bold' : 'normal'
            }}
          >
            📊 Resumo Financeiro
          </button>
        </nav>
      </header>

      <main style={{ padding: '0 20px' }}>
        {currentPage === 'persons' && <PersonList />}
        {currentPage === 'categories' && <CategoryList />}
        {currentPage === 'transactions' && <TransactionList />}
        {currentPage === 'summary' && <PersonSummary />}
      </main>

      
    </div>
  );
}

export default App;