using ExpenseControl.Application.DTOs.Category;
using ExpenseControl.Domain.Enums;
using MediatR;

namespace ExpenseControl.Application.Commands.Category;

public class CreateCategoryCommand : IRequest<CategoryDto>
{
    public string Description { get; set; } = string.Empty;
    public CategoryPurpose Purpose { get; set; }
}
