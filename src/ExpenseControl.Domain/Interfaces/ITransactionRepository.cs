using ExpenseControl.Domain.Entities;

namespace ExpenseControl.Domain.Interfaces;

/// <summary>
/// Repositório específico para a entidade Transaction.
/// Herda operações básicas e adiciona métodos específicos.
/// </summary>
public interface ITransactionRepository : IBaseRepository<Transaction>
{
    /// <summary>
    /// Obtém todas as transações incluindo dados de pessoa e categoria.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de transações com relacionamentos carregados</returns>
    Task<List<Transaction>> GetAllWithIncludesAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Obtém transações de uma pessoa específica.
    /// </summary>
    /// <param name="personId">Identificador da pessoa</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de transações da pessoa</returns>
    Task<List<Transaction>> GetByPersonIdAsync(Guid personId, CancellationToken cancellationToken = default);
}
