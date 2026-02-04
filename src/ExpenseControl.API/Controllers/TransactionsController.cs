using ExpenseControl.Application.Commands.Transaction;
using ExpenseControl.Application.DTOs.Transaction;
using ExpenseControl.Application.Queries.Transaction;
using ExpenseControl.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseControl.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<TransactionsController> _logger;

    public TransactionsController(IMediator mediator, ILogger<TransactionsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<TransactionDto>>> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetAllTransactionsQuery();
            var transactions = await _mediator.Send(query, cancellationToken);
            return Ok(transactions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter transações");
            return StatusCode(500, "Erro ao obter transações");
        }
    }

    [HttpPost]
    public async Task<ActionResult<TransactionDto>> Create([FromBody] CreateTransactionDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!Enum.TryParse<TransactionType>(dto.Type, out var type))
            {
                return BadRequest($"Tipo inválido. Valores aceitos: {string.Join(", ", Enum.GetNames<TransactionType>())}");
            }

            var command = new CreateTransactionCommand
            {
                Description = dto.Description,
                Value = dto.Value,
                Type = type.ToString(),
                PersonId = dto.PersonId,
                CategoryId = dto.CategoryId
            };

            var transaction = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetAll), new { id = transaction.Id }, transaction);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Regra de negócio violada ao criar transação");
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar transação");
            return StatusCode(500, "Erro ao criar transação");
        }
    }
}