using ExpenseControl.Application.Commands.Person;
using ExpenseControl.Domain.Entities;
using ExpenseControl.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace ExpenseControl.Tests.Application.Handlers;

public class CreatePersonCommandHandlerTests
{
    private readonly Mock<IPersonRepository> _repositoryMock;
    private readonly CreatePersonCommandHandler _handler;

    public CreatePersonCommandHandlerTests()
    {
        _repositoryMock = new Mock<IPersonRepository>();
        _handler = new CreatePersonCommandHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ComandoValido_DeveCriarPessoaComSucesso()
    {
        // Arrange
        var command = new CreatePersonCommand
        {
            Name = "João Silva",
            Age = 25
        };

        Person? capturedPerson = null;
        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()))
            .Callback<Person, CancellationToken>((person, _) => capturedPerson = person)
            .ReturnsAsync((Person p, CancellationToken _) => p);

        _repositoryMock
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("João Silva");
        result.Age.Should().Be(25);
        result.Id.Should().NotBeEmpty();
        result.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));

        _repositoryMock.Verify(
            r => r.AddAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()),
            Times.Once);

        _repositoryMock.Verify(
            r => r.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);

        capturedPerson.Should().NotBeNull();
        capturedPerson!.Name.Should().Be("João Silva");
        capturedPerson.Age.Should().Be(25);
    }

    [Theory]
    [InlineData("Maria Santos", 17)]
    [InlineData("Pedro Oliveira", 18)]
    [InlineData("Ana Costa", 65)]
    public async Task Handle_DiferentesNomesEIdades_DeveCriarCorretamente(string name, int age)
    {
        // Arrange
        var command = new CreatePersonCommand
        {
            Name = name,
            Age = age
        };

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Person p, CancellationToken _) => p);

        _repositoryMock
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Name.Should().Be(name);
        result.Age.Should().Be(age);
    }

    [Fact]
    public async Task Handle_DeveGerarIdUnico()
    {
        // Arrange
        var command = new CreatePersonCommand
        {
            Name = "Teste",
            Age = 30
        };

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Person p, CancellationToken _) => p);

        _repositoryMock
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result1 = await _handler.Handle(command, CancellationToken.None);
        var result2 = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result1.Id.Should().NotBe(result2.Id);
        result1.Id.Should().NotBeEmpty();
        result2.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Handle_DeveDefinirCreatedAt()
    {
        // Arrange
        var command = new CreatePersonCommand
        {
            Name = "Teste",
            Age = 30
        };

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Person p, CancellationToken _) => p);

        _repositoryMock
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var beforeExecution = DateTime.UtcNow;

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        var afterExecution = DateTime.UtcNow;

        // Assert
        result.CreatedAt.Should().BeAfter(beforeExecution.AddSeconds(-1));
        result.CreatedAt.Should().BeBefore(afterExecution.AddSeconds(1));
    }

    [Fact]
    public async Task Handle_UpdatedAtDeveSerNulo()
    {
        // Arrange
        var command = new CreatePersonCommand
        {
            Name = "Teste",
            Age = 30
        };

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Person p, CancellationToken _) => p);

        _repositoryMock
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public async Task Handle_DeveChamarRepositoryNaOrdemCorreta()
    {
        // Arrange
        var command = new CreatePersonCommand
        {
            Name = "Teste",
            Age = 30
        };

        var callOrder = new List<string>();

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()))
            .Callback(() => callOrder.Add("AddAsync"))
            .ReturnsAsync((Person p, CancellationToken _) => p);

        _repositoryMock
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Callback(() => callOrder.Add("SaveChangesAsync"))
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        callOrder.Should().HaveCount(2);
        callOrder[0].Should().Be("AddAsync");
        callOrder[1].Should().Be("SaveChangesAsync");
    }
}