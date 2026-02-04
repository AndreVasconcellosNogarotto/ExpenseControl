using ExpenseControl.Application.DTOs.Person;
using MediatR;

namespace ExpenseControl.Application.Queries.Person;

public class GetAllPersonsQuery : IRequest<List<PersonDto>>
{
}
