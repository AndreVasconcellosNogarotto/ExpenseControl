using ExpenseControl.Domain.Enums;

namespace ExpenseControl.Domain.Entities;

/// <summary>
/// Representa uma transação financeira (despesa ou receita).
/// Cada transação está associada a uma pessoa e uma categoria.
/// </summary>
public class Transaction : BaseEntity
{
    /// <summary>
    /// Descrição da transação.
    /// Máximo de 400 caracteres.
    /// Exemplo: "Compra no supermercado", "Pagamento salário", etc.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Valor da transação.
    /// Deve ser um número positivo.
    /// </summary>
    public decimal Value { get; set; }
    
    /// <summary>
    /// Tipo da transação (Despesa ou Receita).
    /// Para menores de 18 anos, apenas despesas são permitidas.
    /// </summary>
    public TransactionType Type { get; set; }
    
    /// <summary>
    /// Identificador da pessoa responsável pela transação.
    /// </summary>
    public Guid PersonId { get; set; }
    
    /// <summary>
    /// Pessoa responsável pela transação.
    /// </summary>
    public virtual Person Person { get; set; } = null!;
    
    /// <summary>
    /// Identificador da categoria da transação.
    /// </summary>
    public Guid CategoryId { get; set; }
    
    /// <summary>
    /// Categoria da transação.
    /// A categoria deve ser compatível com o tipo da transação (validação de negócio).
    /// </summary>
    public virtual Category Category { get; set; } = null!;
}
