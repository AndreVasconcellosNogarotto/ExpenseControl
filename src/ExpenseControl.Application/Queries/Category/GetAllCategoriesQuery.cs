using ExpenseControl.Application.DTOs.Category;
using MediatR;

namespace ExpenseControl.Application.Queries.Category;

public class GetAllCategoriesQuery : IRequest<List<CategoryDto>>
{
}
