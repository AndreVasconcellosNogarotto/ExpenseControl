using ExpenseControl.Application.DTOs.Person;
using ExpenseControl.Domain.Interfaces;
using MediatR;

namespace ExpenseControl.Application.Commands.Person;

public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, PersonDto>
{
    private readonly IPersonRepository _personRepository;

    public CreatePersonCommandHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<PersonDto> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        var person = new Domain.Entities.Person
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Age = request.Age,
            CreatedAt = DateTime.UtcNow
        };

        await _personRepository.AddAsync(person, cancellationToken);
        await _personRepository.SaveChangesAsync(cancellationToken);

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
