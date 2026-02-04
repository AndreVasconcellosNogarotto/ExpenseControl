namespace ExpenseControl.Domain.Enums;

/// <summary>
/// Tipo de transação financeira.
/// Despesa: saída de dinheiro.
/// Receita: entrada de dinheiro.
/// </summary>
public enum TransactionType
{
    /// <summary>
    /// Despesa - saída de dinheiro
    /// </summary>
    Despesa = 1,
    
    /// <summary>
    /// Receita - entrada de dinheiro
    /// </summary>
    Receita = 2
}
