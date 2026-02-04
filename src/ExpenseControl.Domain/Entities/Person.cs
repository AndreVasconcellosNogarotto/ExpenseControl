namespace ExpenseControl.Domain.Entities;

public class Person : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public int Age { get; set; }

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
