using ExpenseControl.Application.DTOs.Person;
using ExpenseControl.Domain.Interfaces;
using MediatR;

namespace ExpenseControl.Application.Queries.Person;

public class GetAllPersonsQueryHandler : IRequestHandler<GetAllPersonsQuery, List<PersonDto>>
{
    private readonly IPersonRepository _personRepository;

    public GetAllPersonsQueryHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<List<PersonDto>> Handle(GetAllPersonsQuery request, CancellationToken cancellationToken)
    {
        
        var persons = await _personRepository.GetAllAsync(cancellationToken);

        return persons.Select(p => new PersonDto
        {
            Id = p.Id,
            Name = p.Name,
            Age = p.Age,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt
        }).ToList();
    }
}
