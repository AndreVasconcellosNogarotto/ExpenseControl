using ExpenseControl.Application.DTOs.Person;
using MediatR;

namespace ExpenseControl.Application.Commands.Person;

public class CreatePersonCommand : IRequest<PersonDto>
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}
