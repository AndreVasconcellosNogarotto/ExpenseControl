using ExpenseControl.Application.DTOs.Person;
using ExpenseControl.Domain.Interfaces;
using MediatR;

namespace ExpenseControl.Application.Queries.Person;

public class GetPersonByIdQueryHandler : IRequestHandler<GetPersonByIdQuery, PersonDto>
{
    private readonly IPersonRepository _personRepository;

    public GetPersonByIdQueryHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<PersonDto> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (person == null)
        {
            throw new KeyNotFoundException($"Pessoa com ID {request.Id} não encontrada.");
        }

        return new PersonDto
        {
            Id = person.Id,
            Name = person.Name,
            Age = person.Age,
            CreatedAt = person.CreatedAt,
            UpdatedAt = person.UpdatedAt
        };
    }
}
