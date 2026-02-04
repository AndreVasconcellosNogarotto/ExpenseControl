using ExpenseControl.Domain.Entities;
using ExpenseControl.Domain.Interfaces;
using ExpenseControl.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseControl.Infrastructure.Repositories;

/// <summary>
/// Implementação específica do repositório de Transaction.
/// Herda operações básicas e adiciona métodos específicos.
/// </summary>
public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(ExpenseControlDbContext context) : base(context)
    {
    }

    public async Task<List<Transaction>> GetAllWithIncludesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Person)
            .Include(t => t.Category)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Transaction>> GetByPersonIdAsync(Guid personId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(t => t.Category)
            .Where(t => t.PersonId == personId)
            .ToListAsync(cancellationToken);
    }
}
