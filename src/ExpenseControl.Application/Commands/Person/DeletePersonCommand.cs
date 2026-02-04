using MediatR;

namespace ExpenseControl.Application.Commands.Person;

public class DeletePersonCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    
    public DeletePersonCommand(Guid id)
    {
        Id = id;
    }
}
