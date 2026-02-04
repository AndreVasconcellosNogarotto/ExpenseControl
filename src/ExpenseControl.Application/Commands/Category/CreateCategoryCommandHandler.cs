using ExpenseControl.Application.DTOs.Category;
using ExpenseControl.Domain.Interfaces;
using MediatR;

namespace ExpenseControl.Application.Commands.Category;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Domain.Entities.Category
        {
            Id = Guid.NewGuid(),
            Description = request.Description,
            Purpose = request.Purpose,
            CreatedAt = DateTime.UtcNow
        };

        await _categoryRepository.AddAsync(category, cancellationToken);
        await _categoryRepository.SaveChangesAsync(cancellationToken);

        return new CategoryDto
        {
            Id = category.Id,
            Description = category.Description,
            Purpose = category.Purpose.ToString(),
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt
        };
    }
}