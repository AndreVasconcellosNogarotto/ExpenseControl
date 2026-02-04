using ExpenseControl.Application.DTOs.Transaction;
using MediatR;

namespace ExpenseControl.Application.Queries.Transaction;

public class GetAllTransactionsQuery : IRequest<List<TransactionDto>>
{
}
