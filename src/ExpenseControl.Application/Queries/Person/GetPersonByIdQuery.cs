using ExpenseControl.Application.DTOs.Person;
using MediatR;

namespace ExpenseControl.Application.Queries.Person;

public class GetPersonByIdQuery : IRequest<PersonDto>
{
    public Guid Id { get; set; }
}
