namespace ExpenseControl.Application.DTOs.Transaction;

public class CreateTransactionDto
{
    public string Description { get; set; } = string.Empty;

    public decimal Value { get; set; }

    public string Type { get; set; } = string.Empty;
 
    public Guid PersonId { get; set; }

    public Guid CategoryId { get; set; }
}
