using ExpenseControl.Application.DTOs.Person;
using ExpenseControl.Domain.Interfaces;
using MediatR;

namespace ExpenseControl.Application.Commands.Person;

public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand, PersonDto>
{
    private readonly IPersonRepository _personRepository;

    public UpdatePersonCommandHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<PersonDto> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (person == null)
        {
            throw new KeyNotFoundException($"Pessoa com ID {request.Id} não encontrada.");
        }

        person.Name = request.Name;
        person.Age = request.Age;
        person.UpdatedAt = DateTime.UtcNow;

        await _personRepository.UpdateAsync(person, cancellationToken);
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
