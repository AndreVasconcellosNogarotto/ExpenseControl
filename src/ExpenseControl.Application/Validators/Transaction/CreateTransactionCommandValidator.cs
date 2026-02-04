using ExpenseControl.Application.Commands.Transaction;
using FluentValidation;

namespace ExpenseControl.Application.Validators.Transaction;

/// <summary>
/// Validator para o comando CreateTransactionCommand.
/// Define as regras de validação para criação de uma transação.
/// </summary>
public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        // Validação da Descrição
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("A descrição é obrigatória.")
            .MaximumLength(400)
            .WithMessage("A descrição deve ter no máximo 400 caracteres.");

        // Validação do Valor
        RuleFor(x => x.Value)
            .GreaterThan(0)
            .WithMessage("O valor deve ser maior que zero.");

        // Validação do Tipo
        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage("O tipo é obrigatório.")
            .Must(type => type == "Despesa" || type == "Receita")
            .WithMessage("O tipo deve ser 'Despesa' ou 'Receita'.");

        // Validação do PersonId
        RuleFor(x => x.PersonId)
            .NotEmpty()
            .WithMessage("O identificador da pessoa é obrigatório.");

        // Validação do CategoryId
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("O identificador da categoria é obrigatório.");
    }
}
