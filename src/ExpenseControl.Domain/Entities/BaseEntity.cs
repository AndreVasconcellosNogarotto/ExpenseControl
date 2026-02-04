namespace ExpenseControl.Domain.Entities;

/// <summary>
/// Classe base para todas as entidades do domínio.
/// Contém propriedades comuns como Id e timestamps de auditoria.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Identificador único da entidade.
    /// Gerado automaticamente pelo banco de dados.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Data e hora de criação do registro.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Data e hora da última atualização do registro.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
