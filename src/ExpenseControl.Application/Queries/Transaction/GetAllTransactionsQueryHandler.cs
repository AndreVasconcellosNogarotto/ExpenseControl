using ExpenseControl.Application.DTOs.Transaction;
using ExpenseControl.Domain.Interfaces;
using MediatR;

namespace ExpenseControl.Application.Queries.Transaction;

public class GetAllTransactionsQueryHandler : IRequestHandler<GetAllTransactionsQuery, List<TransactionDto>>
{
    private readonly ITransactionRepository _transactionRepository;

    public GetAllTransactionsQueryHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<List<TransactionDto>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
    {
        var transactions = await _transactionRepository.GetAllWithIncludesAsync(cancellationToken);

        return transactions.Select(t => new TransactionDto
        {
            Id = t.Id,
            Description = t.Description,
            Value = t.Value,
            Type = t.Type.ToString(),
            PersonId = t.PersonId,
            PersonName = t.Person.Name,
            CategoryId = t.CategoryId,
            CategoryDescription = t.Category.Description,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt
        }).ToList();
    }
}