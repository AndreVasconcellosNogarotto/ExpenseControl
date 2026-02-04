using ExpenseControl.Application.DTOs.Transaction;
using MediatR;

namespace ExpenseControl.Application.Commands.Transaction;

public class CreateTransactionCommand : IRequest<TransactionDto>
{
    public string Description { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public string Type { get; set; } = string.Empty;
    public Guid PersonId { get; set; }
    public Guid CategoryId { get; set; }
}
