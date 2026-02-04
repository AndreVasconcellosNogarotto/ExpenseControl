using ExpenseControl.Application.Commands.Category;
using ExpenseControl.Application.DTOs.Category;
using ExpenseControl.Application.Queries.Category;
using ExpenseControl.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseControl.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(IMediator mediator, ILogger<CategoriesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetAllCategoriesQuery();
            var categories = await _mediator.Send(query, cancellationToken);
            return Ok(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter categorias");
            return StatusCode(500, "Erro ao obter categorias");
        }
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create([FromBody] CreateCategoryDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!Enum.TryParse<CategoryPurpose>(dto.Purpose, out var purpose))
            {
                return BadRequest("Finalidade inválida. Valores aceitos: Despesa, Receita, Ambas");
            }

            var command = new CreateCategoryCommand
            {
                Description = dto.Description,
                Purpose = purpose
            };

            var category = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetAll), new { id = category.Id }, category);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar categoria");
            return StatusCode(500, "Erro ao criar categoria");
        }
    }
}