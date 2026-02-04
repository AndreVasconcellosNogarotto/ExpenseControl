namespace ExpenseControl.Application.DTOs.Category;

/// <summary>
/// DTO para representar o resumo financeiro de uma categoria.
/// Usado na consulta opcional de totais por categoria.
/// </summary>
public class CategorySummaryDto
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;
    
    /// <summary>
    /// Total de receitas da categoria.
    /// </summary>
    public decimal TotalIncome { get; set; }
    
    /// <summary>
    /// Total de despesas da categoria.
    /// </summary>
    public decimal TotalExpense { get; set; }
    
    /// <summary>
    /// Saldo da categoria (receita - despesa).
    /// </summary>
    public decimal Balance { get; set; }
}

/// <summary>
/// DTO para representar o resumo geral de todas as categorias.
/// </summary>
public class CategorySummaryResponseDto
{
    /// <summary>
    /// Lista de resumos individuais de cada categoria.
    /// </summary>
    public List<CategorySummaryDto> Categories { get; set; } = new();
    
    /// <summary>
    /// Total geral de receitas de todas as categorias.
    /// </summary>
    public decimal TotalIncome { get; set; }
    
    /// <summary>
    /// Total geral de despesas de todas as categorias.
    /// </summary>
    public decimal TotalExpense { get; set; }
    
    /// <summary>
    /// Saldo líquido geral (total receitas - total despesas).
    /// </summary>
    public decimal NetBalance { get; set; }
}
