using ExpenseControl.Application.DTOs.Transaction;
using ExpenseControl.Domain.Enums;
using ExpenseControl.Domain.Interfaces;
using MediatR;

namespace ExpenseControl.Application.Commands.Transaction;

public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, TransactionDto>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IPersonRepository _personRepository;
    private readonly ICategoryRepository _categoryRepository;

    public CreateTransactionCommandHandler(
        ITransactionRepository transactionRepository,
        IPersonRepository personRepository,
        ICategoryRepository categoryRepository)
    {
        _transactionRepository = transactionRepository;
        _personRepository = personRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<TransactionDto> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetByIdAsync(request.PersonId, cancellationToken);
        if (person == null)
        {
            throw new KeyNotFoundException($"Pessoa com ID {request.PersonId} não encontrada.");
        }

        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category == null)
        {
            throw new KeyNotFoundException($"Categoria com ID {request.CategoryId} não encontrada.");
        }

        if (!Enum.TryParse<TransactionType>(request.Type, out var transactionType))
        {
            throw new ArgumentException($"Tipo de transação inválido: {request.Type}");
        }

        if (person.Age < 18 && transactionType == TransactionType.Receita)
        {
            throw new InvalidOperationException("Menores de 18 anos só podem criar despesas.");
        }

        var isCompatible = category.Purpose switch
        {
            CategoryPurpose.Despesa => transactionType == TransactionType.Despesa,
            CategoryPurpose.Receita => transactionType == TransactionType.Receita,
            CategoryPurpose.Ambas => true,
            _ => false
        };

        if (!isCompatible)
        {
            throw new InvalidOperationException(
                $"A categoria '{category.Description}' com finalidade '{category.Purpose}' " +
                $"não pode ser usada para transações do tipo '{transactionType}'.");
        }

        var transaction = new Domain.Entities.Transaction
        {
            Id = Guid.NewGuid(),
            Description = request.Description,
            Value = request.Value,
            Type = transactionType,
            PersonId = request.PersonId,
            CategoryId = request.CategoryId,
            CreatedAt = DateTime.UtcNow
        };

        await _transactionRepository.AddAsync(transaction, cancellationToken);
        await _transactionRepository.SaveChangesAsync(cancellationToken);

        return new TransactionDto
        {
            Id = transaction.Id,
            Description = transaction.Description,
            Value = transaction.Value,
            Type = transaction.Type.ToString(),
            PersonId = transaction.PersonId,
            PersonName = person.Name,
            CategoryId = transaction.CategoryId,
            CategoryDescription = category.Description,
            CreatedAt = transaction.CreatedAt,
            UpdatedAt = transaction.UpdatedAt
        };
    }
}
