using ExpenseControl.Domain.Entities;

namespace ExpenseControl.Domain.Interfaces;

public interface IPersonRepository : IBaseRepository<Person>
{
    Task<Person?> GetByIdWithTransactionsAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<List<PersonSummary>> GetPersonSummariesAsync(CancellationToken cancellationToken = default);
}

public class PersonSummary
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal Balance => TotalIncome - TotalExpense;
}
