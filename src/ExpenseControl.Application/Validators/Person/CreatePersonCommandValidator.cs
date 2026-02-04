using ExpenseControl.Application.Commands.Person;
using FluentValidation;

namespace ExpenseControl.Application.Validators.Person;

public class CreatePersonCommandValidator : AbstractValidator<CreatePersonCommand>
{
    public CreatePersonCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("O nome é obrigatório.")
            .MaximumLength(200)
            .WithMessage("O nome deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Age)
            .GreaterThan(0)
            .WithMessage("A idade deve ser maior que 0.")
            .LessThanOrEqualTo(120)
            .WithMessage("A idade deve ser menor ou igual a 120.");
    }
}