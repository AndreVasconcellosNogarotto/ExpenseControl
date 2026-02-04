using ExpenseControl.Domain.Entities;
using ExpenseControl.Domain.Enums;
using ExpenseControl.Domain.Interfaces;
using ExpenseControl.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseControl.Infrastructure.Repositories;

/// <summary>
/// Implementação específica do repositório de Category.
/// Herda operações básicas e adiciona métodos específicos.
/// </summary>
public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(ExpenseControlDbContext context) : base(context)
    {
    }

    public async Task<List<CategorySummary>> GetCategorySummariesAsync(CancellationToken cancellationToken = default)
    {
        // Usa LINQ para calcular os totais de cada categoria
        var summaries = await _dbSet
            .Select(c => new CategorySummary
            {
                Id = c.Id,
                Description = c.Description,
                Purpose = c.Purpose.ToString(),
                // Soma todas as receitas da categoria
                TotalIncome = c.Transactions
                    .Where(t => t.Type == TransactionType.Receita)
                    .Sum(t => t.Value),
                // Soma todas as despesas da categoria
                TotalExpense = c.Transactions
                    .Where(t => t.Type == TransactionType.Despesa)
                    .Sum(t => t.Value)
            })
            .ToListAsync(cancellationToken);

        return summaries;
    }
}
