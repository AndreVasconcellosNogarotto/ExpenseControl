namespace ExpenseControl.Application.DTOs.Category;

public class CategoryDto
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
