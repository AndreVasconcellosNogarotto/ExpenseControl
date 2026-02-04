using ExpenseControl.Application.Commands.Person;
using ExpenseControl.Application.DTOs.Person;
using ExpenseControl.Application.Queries.Person;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseControl.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<PersonsController> _logger;

    public PersonsController(IMediator mediator, ILogger<PersonsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<PersonDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetAllPersonsQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar pessoas");
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PersonDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetPersonByIdQuery { Id = id };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Pessoa com ID {id} não encontrada");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar pessoa {PersonId}", id);
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(PersonDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreatePersonDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var command = new CreatePersonCommand
            {
                Name = dto.Name,
                Age = dto.Age
            };

            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar pessoa");
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(PersonDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePersonDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var command = new UpdatePersonCommand
            {
                Id = id,
                Name = dto.Name,
                Age = dto.Age
            };

            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Pessoa com ID {id} não encontrada");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar pessoa {PersonId}", id);
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var command = new DeletePersonCommand(id);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Pessoa com ID {id} não encontrada");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar pessoa {PersonId}", id);
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    [HttpGet("summary")]
    [ProducesResponseType(typeof(PersonSummaryResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSummary(CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetPersonSummaryQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar resumo de pessoas");
            return StatusCode(500, "Erro interno do servidor");
        }
    }
}
