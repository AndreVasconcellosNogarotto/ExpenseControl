using ExpenseControl.Application.Commands.Transaction;
using ExpenseControl.Domain.Entities;
using ExpenseControl.Domain.Enums;
using ExpenseControl.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace ExpenseControl.Tests.Application.Handlers;

public class CreateTransactionCommandHandlerTests
{
    private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
    private readonly Mock<IPersonRepository> _personRepositoryMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly CreateTransactionCommandHandler _handler;

    public CreateTransactionCommandHandlerTests()
    {
        _transactionRepositoryMock = new Mock<ITransactionRepository>();
        _personRepositoryMock = new Mock<IPersonRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();

        _handler = new CreateTransactionCommandHandler(
            _transactionRepositoryMock.Object,
            _personRepositoryMock.Object,
            _categoryRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_TransacaoValida_DeveCriarComSucesso()
    {
        var personId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        var person = new Person
        {
            Id = personId,
            Name = "João Silva",
            Age = 25,
            CreatedAt = DateTime.UtcNow
        };

        var category = new Category
        {
            Id = categoryId,
            Description = "Alimentação",
            Purpose = CategoryPurpose.Ambas,
            CreatedAt = DateTime.UtcNow
        };

        var command = new CreateTransactionCommand
        {
            Description = "Compra no mercado",
            Value = 150m,
            Type = "Despesa",
            PersonId = personId,
            CategoryId = categoryId
        };

        _personRepositoryMock
            .Setup(r => r.GetByIdAsync(personId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(person);

        _categoryRepositoryMock
            .Setup(r => r.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        _transactionRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Transaction t, CancellationToken _) => t);

        _transactionRepositoryMock
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Description.Should().Be("Compra no mercado");
        result.Value.Should().Be(150m);
        result.Type.Should().Be("Despesa");
        result.PersonName.Should().Be("João Silva");
        result.CategoryDescription.Should().Be("Alimentação");
    }

    [Fact]
    public async Task Handle_MenorDe18TentandoCriarReceita_DeveLancarExcecao()
    {
        var personId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        var person = new Person
        {
            Id = personId,
            Name = "Maria Santos",
            Age = 16,
            CreatedAt = DateTime.UtcNow
        };

        var category = new Category
        {
            Id = categoryId,
            Description = "Mesada",
            Purpose = CategoryPurpose.Receita,
            CreatedAt = DateTime.UtcNow
        };

        var command = new CreateTransactionCommand
        {
            Description = "Mesada do mês",
            Value = 100m,
            Type = "Receita",
            PersonId = personId,
            CategoryId = categoryId
        };

        _personRepositoryMock
            .Setup(r => r.GetByIdAsync(personId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(person);

        _categoryRepositoryMock
            .Setup(r => r.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Contain("Menores de 18 anos só podem criar despesas");
    }

    [Fact]
    public async Task Handle_MenorDe18CriandoDespesa_DevePermitir()
    {
        var personId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        var person = new Person
        {
            Id = personId,
            Name = "Pedro Costa",
            Age = 17,
            CreatedAt = DateTime.UtcNow
        };

        var category = new Category
        {
            Id = categoryId,
            Description = "Lanche",
            Purpose = CategoryPurpose.Despesa,
            CreatedAt = DateTime.UtcNow
        };

        var command = new CreateTransactionCommand
        {
            Description = "Lanche na escola",
            Value = 20m,
            Type = "Despesa",
            PersonId = personId,
            CategoryId = categoryId
        };

        _personRepositoryMock
            .Setup(r => r.GetByIdAsync(personId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(person);

        _categoryRepositoryMock
            .Setup(r => r.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        _transactionRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Transaction t, CancellationToken _) => t);

        _transactionRepositoryMock
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Type.Should().Be("Despesa");
    }

    [Fact]
    public async Task Handle_CategoriaDespesaComTransacaoReceita_DeveLancarExcecao()
    {
        var personId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        var person = new Person
        {
            Id = personId,
            Name = "João Silva",
            Age = 25,
            CreatedAt = DateTime.UtcNow
        };

        var category = new Category
        {
            Id = categoryId,
            Description = "Transporte",
            Purpose = CategoryPurpose.Despesa,
            CreatedAt = DateTime.UtcNow
        };

        var command = new CreateTransactionCommand
        {
            Description = "Tentando criar receita",
            Value = 100m,
            Type = "Receita",
            PersonId = personId,
            CategoryId = categoryId
        };

        _personRepositoryMock
            .Setup(r => r.GetByIdAsync(personId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(person);

        _categoryRepositoryMock
            .Setup(r => r.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Contain("não pode ser usada para transações do tipo");
    }

    [Fact]
    public async Task Handle_CategoriaAmbasComQualquerTipo_DevePermitir()
    {
        var personId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        var person = new Person
        {
            Id = personId,
            Name = "Ana Silva",
            Age = 30,
            CreatedAt = DateTime.UtcNow
        };

        var category = new Category
        {
            Id = categoryId,
            Description = "Diversos",
            Purpose = CategoryPurpose.Ambas,
            CreatedAt = DateTime.UtcNow
        };

        var commandReceita = new CreateTransactionCommand
        {
            Description = "Receita",
            Value = 100m,
            Type = "Receita",
            PersonId = personId,
            CategoryId = categoryId
        };

        _personRepositoryMock
            .Setup(r => r.GetByIdAsync(personId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(person);

        _categoryRepositoryMock
            .Setup(r => r.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        _transactionRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Transaction t, CancellationToken _) => t);

        _transactionRepositoryMock
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var result = await _handler.Handle(commandReceita, CancellationToken.None);

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_PessoaNaoEncontrada_DeveLancarExcecao()
    {
        var command = new CreateTransactionCommand
        {
            Description = "Teste",
            Value = 100m,
            Type = "Despesa",
            PersonId = Guid.NewGuid(),
            CategoryId = Guid.NewGuid()
        };

        _personRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Person?)null);

        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Contain("não encontrada");
    }

    [Fact]
    public async Task Handle_CategoriaNaoEncontrada_DeveLancarExcecao()
    {
        var personId = Guid.NewGuid();

        var person = new Person
        {
            Id = personId,
            Name = "Teste",
            Age = 25,
            CreatedAt = DateTime.UtcNow
        };

        var command = new CreateTransactionCommand
        {
            Description = "Teste",
            Value = 100m,
            Type = "Despesa",
            PersonId = personId,
            CategoryId = Guid.NewGuid()
        };

        _personRepositoryMock
            .Setup(r => r.GetByIdAsync(personId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(person);

        _categoryRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category?)null);

        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Contain("não encontrada");
    }

    [Fact]
    public async Task Handle_TipoInvalido_DeveLancarExcecao()
    {
        // Arrange
        var personId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        var command = new CreateTransactionCommand
        {
            Description = "Teste",
            Value = 100m,
            Type = "TipoInvalido",
            PersonId = personId,
            CategoryId = categoryId
        };

        var person = new Person
        {
            Id = personId,
            Name = "Teste",
            Age = 25,
            CreatedAt = DateTime.UtcNow
        };

        var category = new Category
        {
            Id = categoryId,
            Description = "Teste",
            Purpose = CategoryPurpose.Ambas,
            CreatedAt = DateTime.UtcNow
        };

        _personRepositoryMock
            .Setup(r => r.GetByIdAsync(personId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(person);

        _categoryRepositoryMock
            .Setup(r => r.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Contain("inválido");
    }
}