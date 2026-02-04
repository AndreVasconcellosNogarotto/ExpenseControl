using ExpenseControl.Domain.Enums;

namespace ExpenseControl.Domain.Entities;

/// <summary>
/// Representa uma categoria de transação no sistema.
/// Categorias podem ser de despesa, receita ou ambas.
/// </summary>
public class Category : BaseEntity
{
    /// <summary>
    /// Descrição da categoria.
    /// Máximo de 400 caracteres.
    /// Exemplo: "Alimentação", "Salário", "Lazer", etc.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Finalidade da categoria.
    /// Define se a categoria pode ser usada para despesas, receitas ou ambas.
    /// </summary>
    public CategoryPurpose Purpose { get; set; }
    
    /// <summary>
    /// Lista de transações associadas a esta categoria.
    /// </summary>
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
