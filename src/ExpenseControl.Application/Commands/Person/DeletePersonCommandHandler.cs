using ExpenseControl.Domain.Interfaces;
using MediatR;

namespace ExpenseControl.Application.Commands.Person;

public class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, Unit>
{
    private readonly IPersonRepository _personRepository;

    public DeletePersonCommandHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<Unit> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (person == null)
        {
            throw new KeyNotFoundException($"Pessoa com ID {request.Id} não encontrada.");
        }

        await _personRepository.DeleteAsync(person, cancellationToken);
        await _personRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
