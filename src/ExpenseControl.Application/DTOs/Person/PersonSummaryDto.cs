namespace ExpenseControl.Application.DTOs.Person;

public class PersonSummaryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }

    public decimal TotalIncome { get; set; }
  
    public decimal TotalExpense { get; set; }

    public decimal Balance { get; set; }
}

public class PersonSummaryResponseDto
{
    public List<PersonSummaryDto> Persons { get; set; } = new();

    public decimal TotalIncome { get; set; }

    public decimal TotalExpense { get; set; }

    public decimal NetBalance { get; set; }
}
