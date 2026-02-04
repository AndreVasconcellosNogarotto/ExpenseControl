using ExpenseControl.Application.DTOs.Person;
using ExpenseControl.Domain.Interfaces;
using MediatR;

namespace ExpenseControl.Application.Queries.Person;

public class GetPersonSummaryQueryHandler : IRequestHandler<GetPersonSummaryQuery, PersonSummaryResponseDto>
{
    private readonly IPersonRepository _personRepository;

    public GetPersonSummaryQueryHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<PersonSummaryResponseDto> Handle(GetPersonSummaryQuery request, CancellationToken cancellationToken)
    {
        var personSummaries = await _personRepository.GetPersonSummariesAsync(cancellationToken);

        var personDtos = personSummaries.Select(ps => new PersonSummaryDto
        {
            Id = ps.Id,
            Name = ps.Name,
            Age = ps.Age,
            TotalIncome = ps.TotalIncome,
            TotalExpense = ps.TotalExpense,
            Balance = ps.Balance
        }).ToList();

        var totalIncome = personDtos.Sum(p => p.TotalIncome);
        var totalExpense = personDtos.Sum(p => p.TotalExpense);

        return new PersonSummaryResponseDto
        {
            Persons = personDtos,
            TotalIncome = totalIncome,
            TotalExpense = totalExpense,
            NetBalance = totalIncome - totalExpense
        };
    }
}
