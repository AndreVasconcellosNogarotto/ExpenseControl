using ExpenseControl.Domain.Entities;

namespace ExpenseControl.Domain.Interfaces;

/// <summary>
/// Repositório específico para a entidade Category.
/// Herda operações básicas e adiciona métodos específicos.
/// </summary>
public interface ICategoryRepository : IBaseRepository<Category>
{
    /// <summary>
    /// Obtém todas as categorias com resumo financeiro (receitas, despesas e saldo).
    /// Funcionalidade opcional conforme requisitos.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de categorias com seus totais financeiros</returns>
    Task<List<CategorySummary>> GetCategorySummariesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Representa um resumo financeiro de uma categoria.
/// </summary>
public class CategorySummary
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal Balance => TotalIncome - TotalExpense;
}
