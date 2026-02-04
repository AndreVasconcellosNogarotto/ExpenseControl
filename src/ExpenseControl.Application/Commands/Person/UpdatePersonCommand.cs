using ExpenseControl.Application.DTOs.Person;
using MediatR;

namespace ExpenseControl.Application.Commands.Person;

public class UpdatePersonCommand : IRequest<PersonDto>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}
