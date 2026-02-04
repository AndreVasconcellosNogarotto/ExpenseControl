using FluentValidation;

namespace ExpenseControl.Application.Commands.Category;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Descrição é obrigatória")
            .MinimumLength(3).WithMessage("Descrição deve ter no mínimo 3 caracteres")
            .MaximumLength(400).WithMessage("Descrição deve ter no máximo 400 caracteres")
            .Must(NotBeOnlyWhitespace).WithMessage("Descrição não pode conter apenas espaços");

        RuleFor(x => x.Purpose)
            .IsInEnum().WithMessage("Finalidade inválida. Valores aceitos: Despesa, Receita, Ambas");
    }

    private bool NotBeOnlyWhitespace(string value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }
}