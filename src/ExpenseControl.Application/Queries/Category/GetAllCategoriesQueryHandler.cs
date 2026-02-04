using ExpenseControl.Application.DTOs.Category;
using ExpenseControl.Domain.Interfaces;
using MediatR;

namespace ExpenseControl.Application.Queries.Category;

public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, List<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);

        return categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Description = c.Description,
            Purpose = c.Purpose.ToString(),
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt
        }).ToList();
    }
}