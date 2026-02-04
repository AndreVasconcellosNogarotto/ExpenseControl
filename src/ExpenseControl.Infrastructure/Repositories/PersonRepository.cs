using ExpenseControl.Domain.Entities;
using ExpenseControl.Domain.Enums;
using ExpenseControl.Domain.Interfaces;
using ExpenseControl.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseControl.Infrastructure.Repositories;

public class PersonRepository : BaseRepository<Person>, IPersonRepository
{
    public PersonRepository(ExpenseControlDbContext context) : base(context)
    {
    }

    public async Task<Person?> GetByIdWithTransactionsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Transactions)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<List<PersonSummary>> GetPersonSummariesAsync(CancellationToken cancellationToken = default)
    {
        var summaries = await _dbSet
            .Select(p => new PersonSummary
            {
                Id = p.Id,
                Name = p.Name,
                Age = p.Age,
                TotalIncome = p.Transactions
                    .Where(t => t.Type == TransactionType.Receita)
                    .Sum(t => t.Value),
                TotalExpense = p.Transactions
                    .Where(t => t.Type == TransactionType.Despesa)
                    .Sum(t => t.Value)
            })
            .ToListAsync(cancellationToken);

        return summaries;
    }
}
