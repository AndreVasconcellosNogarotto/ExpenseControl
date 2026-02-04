# GUIA DE IMPLEMENTAÇÃO - Arquivos Restantes

## 📋 Status do Projeto

### ✅ Arquivos já criados:

#### Domain Layer
- ✅ BaseEntity
- ✅ Person, Category, Transaction (Entities)
- ✅ TransactionType, CategoryPurpose (Enums)
- ✅ Todas as interfaces de repositório

#### Application Layer  
- ✅ CreatePersonCommand + Handler + Validator
- ✅ UpdatePersonCommand + Handler
- ✅ DeletePersonCommand + Handler
- ✅ GetAllPersonsQuery + Handler
- ✅ GetPersonSummaryQuery + Handler
- ✅ CreateTransactionCommand + Handler + Validator
- ✅ Todos os DTOs (Person, Category, Transaction)

#### Infrastructure Layer
- ✅ ExpenseControlDbContext
- ✅ Configurações EF Core (Person, Category, Transaction)
- ✅ PersonRepository, CategoryRepository, TransactionRepository

#### API Layer
- ✅ Program.cs
- ✅ appsettings.json
- ✅ PersonsController (completo)

---

## 📝 Arquivos que faltam implementar

### 1. Application Layer - Queries Restantes

#### GetPersonByIdQuery.cs
```csharp
using ExpenseControl.Application.DTOs.Person;
using MediatR;

namespace ExpenseControl.Application.Queries.Person;

public class GetPersonByIdQuery : IRequest<PersonDto>
{
    public Guid Id { get; set; }
}
```

#### GetPersonByIdQueryHandler.cs
```csharp
using ExpenseControl.Application.DTOs.Person;
using ExpenseControl.Domain.Interfaces;
using MediatR;

namespace ExpenseControl.Application.Queries.Person;

public class GetPersonByIdQueryHandler : IRequestHandler<GetPersonByIdQuery, PersonDto>
{
    private readonly IPersonRepository _personRepository;

    public GetPersonByIdQueryHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<PersonDto> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (person == null)
            throw new KeyNotFoundException($"Pessoa com ID {request.Id} não encontrada.");

        return new PersonDto
        {
            Id = person.Id,
            Name = person.Name,
            Age = person.Age,
            CreatedAt = person.CreatedAt,
            UpdatedAt = person.UpdatedAt
        };
    }
}
```

### 2. Category - Commands e Queries

#### CreateCategoryCommand.cs
```csharp
using ExpenseControl.Application.DTOs.Category;
using MediatR;

namespace ExpenseControl.Application.Commands.Category;

public class CreateCategoryCommand : IRequest<CategoryDto>
{
    public string Description { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;
}
```

#### CreateCategoryCommandHandler.cs
```csharp
using ExpenseControl.Application.DTOs.Category;
using ExpenseControl.Domain.Enums;
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
        if (!Enum.TryParse<CategoryPurpose>(request.Purpose, out var purpose))
        {
            throw new ArgumentException($"Finalidade inválida: {request.Purpose}");
        }

        var category = new Domain.Entities.Category
        {
            Id = Guid.NewGuid(),
            Description = request.Description,
            Purpose = purpose,
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
```

#### CreateCategoryCommandValidator.cs
```csharp
using FluentValidation;

namespace ExpenseControl.Application.Validators.Category;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A descrição é obrigatória.")
            .MaximumLength(400).WithMessage("A descrição deve ter no máximo 400 caracteres.");

        RuleFor(x => x.Purpose)
            .NotEmpty().WithMessage("A finalidade é obrigatória.")
            .Must(p => p == "Despesa" || p == "Receita" || p == "Ambas")
            .WithMessage("A finalidade deve ser 'Despesa', 'Receita' ou 'Ambas'.");
    }
}
```

#### GetAllCategoriesQuery.cs e Handler
```csharp
// Query
using ExpenseControl.Application.DTOs.Category;
using MediatR;

namespace ExpenseControl.Application.Queries.Category;

public class GetAllCategoriesQuery : IRequest<List<CategoryDto>>
{
}

// Handler
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
```

#### GetCategorySummaryQuery.cs e Handler (Opcional)
```csharp
// Similar ao GetPersonSummaryQuery, mas para categorias
```

### 3. Transaction - Queries

#### GetAllTransactionsQuery.cs
```csharp
using ExpenseControl.Application.DTOs.Transaction;
using MediatR;

namespace ExpenseControl.Application.Queries.Transaction;

public class GetAllTransactionsQuery : IRequest<List<TransactionDto>>
{
}
```

#### GetAllTransactionsQueryHandler.cs
```csharp
using ExpenseControl.Application.DTOs.Transaction;
using ExpenseControl.Domain.Interfaces;
using MediatR;

namespace ExpenseControl.Application.Queries.Transaction;

public class GetAllTransactionsQueryHandler : IRequestHandler<GetAllTransactionsQuery, List<TransactionDto>>
{
    private readonly ITransactionRepository _transactionRepository;

    public GetAllTransactionsQueryHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<List<TransactionDto>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
    {
        var transactions = await _transactionRepository.GetAllWithIncludesAsync(cancellationToken);
        
        return transactions.Select(t => new TransactionDto
        {
            Id = t.Id,
            Description = t.Description,
            Value = t.Value,
            Type = t.Type.ToString(),
            PersonId = t.PersonId,
            PersonName = t.Person.Name,
            CategoryId = t.CategoryId,
            CategoryDescription = t.Category.Description,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt
        }).ToList();
    }
}
```

### 4. API Controllers

#### CategoriesController.cs
```csharp
using ExpenseControl.Application.Commands.Category;
using ExpenseControl.Application.DTOs.Category;
using ExpenseControl.Application.Queries.Category;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseControl.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllCategoriesQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto, CancellationToken cancellationToken)
    {
        var command = new CreateCategoryCommand
        {
            Description = dto.Description,
            Purpose = dto.Purpose
        };
        
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(CancellationToken cancellationToken)
    {
        var query = new GetCategorySummaryQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}
```

#### TransactionsController.cs
```csharp
using ExpenseControl.Application.Commands.Transaction;
using ExpenseControl.Application.DTOs.Transaction;
using ExpenseControl.Application.Queries.Transaction;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseControl.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllTransactionsQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTransactionDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var command = new CreateTransactionCommand
            {
                Description = dto.Description,
                Value = dto.Value,
                Type = dto.Type,
                PersonId = dto.PersonId,
                CategoryId = dto.CategoryId
            };
            
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
```

---

## 🚀 Próximos Passos

1. **Implementar os arquivos listados acima**
   - Copiar e colar os códigos nos locais corretos
   - Seguir a estrutura de pastas já criada

2. **Criar as Migrations do Entity Framework**
   ```bash
   cd src/ExpenseControl.API
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

3. **Testar a API**
   ```bash
   dotnet run --project src/ExpenseControl.API
   ```
   - Acessar Swagger: https://localhost:5001/swagger

4. **Implementar o Frontend React** (próxima seção)

---

## ⚠️ Notas Importantes

- Todos os arquivos seguem o padrão CQRS com MediatR
- FluentValidation está configurado para validação automática
- Cascade delete está configurado no PersonConfiguration
- Tratamento de erros implementado nos controllers
- CORS configurado para desenvolvimento local

## 📚 Referências Rápidas

- **Padrão CQRS**: Commands (escrita) e Queries (leitura)
- **MediatR**: Mediator pattern para desacoplamento
- **FluentValidation**: Validação de dados de entrada
- **Entity Framework Core**: ORM para PostgreSQL
- **Clean Architecture**: Separação de responsabilidades em camadas
