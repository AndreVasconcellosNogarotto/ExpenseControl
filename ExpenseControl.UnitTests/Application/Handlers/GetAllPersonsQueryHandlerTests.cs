using ExpenseControl.Application.Queries.Person;
using ExpenseControl.Domain.Entities;
using ExpenseControl.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace ExpenseControl.Tests.Application.Handlers;

public class GetAllPersonsQueryHandlerTests
{
    private readonly Mock<IPersonRepository> _repositoryMock;
    private readonly GetAllPersonsQueryHandler _handler;

    public GetAllPersonsQueryHandlerTests()
    {
        _repositoryMock = new Mock<IPersonRepository>();
        _handler = new GetAllPersonsQueryHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_QuandoExistemPessoas_DeveRetornarListaCompleta()
    {
        // Arrange
        var persons = new List<Person>
        {
            new Person
            {
                Id = Guid.NewGuid(),
                Name = "João Silva",
                Age = 25,
                CreatedAt = DateTime.UtcNow
            },
            new Person
            {
                Id = Guid.NewGuid(),
                Name = "Maria Santos",
                Age = 30,
                CreatedAt = DateTime.UtcNow
            }
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(persons);

        var query = new GetAllPersonsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result[0].Name.Should().Be("João Silva");
        result[0].Age.Should().Be(25);
        result[1].Name.Should().Be("Maria Santos");
        result[1].Age.Should().Be(30);
    }

    [Fact]
    public async Task Handle_QuandoNaoExistemPessoas_DeveRetornarListaVazia()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Person>());

        var query = new GetAllPersonsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_DeveConverterTodasPropriedadesCorretamente()
    {
        // Arrange
        var id = Guid.NewGuid();
        var createdAt = DateTime.UtcNow.AddDays(-10);
        var updatedAt = DateTime.UtcNow.AddDays(-5);

        var persons = new List<Person>
        {
            new Person
            {
                Id = id,
                Name = "Teste",
                Age = 40,
                CreatedAt = createdAt,
                UpdatedAt = updatedAt
            }
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(persons);

        var query = new GetAllPersonsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        result[0].Id.Should().Be(id);
        result[0].Name.Should().Be("Teste");
        result[0].Age.Should().Be(40);
        result[0].CreatedAt.Should().Be(createdAt);
        result[0].UpdatedAt.Should().Be(updatedAt);
    }

    [Fact]
    public async Task Handle_ComMultiplasPessoas_DeveRetornarNaMesmaOrdem()
    {
        // Arrange
        var persons = new List<Person>
        {
            new Person { Id = Guid.NewGuid(), Name = "Pessoa 1", Age = 20, CreatedAt = DateTime.UtcNow },
            new Person { Id = Guid.NewGuid(), Name = "Pessoa 2", Age = 30, CreatedAt = DateTime.UtcNow },
            new Person { Id = Guid.NewGuid(), Name = "Pessoa 3", Age = 40, CreatedAt = DateTime.UtcNow }
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(persons);

        var query = new GetAllPersonsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(3);
        result[0].Name.Should().Be("Pessoa 1");
        result[1].Name.Should().Be("Pessoa 2");
        result[2].Name.Should().Be("Pessoa 3");
    }

    [Fact]
    public async Task Handle_DeveChamarRepositoryUmaVez()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Person>());

        var query = new GetAllPersonsQuery();

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(
            r => r.GetAllAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_DeveTratarUpdatedAtNulo()
    {
        // Arrange
        var persons = new List<Person>
        {
            new Person
            {
                Id = Guid.NewGuid(),
                Name = "Teste",
                Age = 25,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null // Nunca foi atualizado
            }
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(persons);

        var query = new GetAllPersonsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        result[0].UpdatedAt.Should().BeNull();
    }
}