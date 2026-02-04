namespace ExpenseControl.Application.DTOs.Transaction;

public class TransactionDto
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public string Type { get; set; } = string.Empty;
    public Guid PersonId { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public string CategoryDescription { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
