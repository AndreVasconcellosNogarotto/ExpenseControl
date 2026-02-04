using ExpenseControl.Domain.Entities;
using ExpenseControl.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace ExpenseControl.Tests.Domain.Entities;

public class TransactionTests
{
    [Fact]
    public void Transaction_DeveSerCriadaComPropriedadesCorretas()
    {
        // Arrange
        var id = Guid.NewGuid();
        var description = "Compra no supermercado";
        var value = 150.50m;
        var type = TransactionType.Despesa;
        var personId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;

        // Act
        var transaction = new Transaction
        {
            Id = id,
            Description = description,
            Value = value,
            Type = type,
            PersonId = personId,
            CategoryId = categoryId,
            CreatedAt = createdAt
        };

        // Assert
        transaction.Id.Should().Be(id);
        transaction.Description.Should().Be(description);
        transaction.Value.Should().Be(value);
        transaction.Type.Should().Be(type);
        transaction.PersonId.Should().Be(personId);
        transaction.CategoryId.Should().Be(categoryId);
        transaction.CreatedAt.Should().Be(createdAt);
        transaction.UpdatedAt.Should().BeNull();
    }

    [Theory]
    [InlineData(TransactionType.Despesa)]
    [InlineData(TransactionType.Receita)]
    public void Transaction_DeveAceitarTodosTiposDeTransacao(TransactionType type)
    {
        // Arrange & Act
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            Description = "Teste",
            Value = 100m,
            Type = type,
            PersonId = Guid.NewGuid(),
            CategoryId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };

        // Assert
        transaction.Type.Should().Be(type);
    }

    [Theory]
    [InlineData(0.01)]
    [InlineData(10.50)]
    [InlineData(1000)]
    [InlineData(999999.99)]
    public void Transaction_DeveAceitarValoresPositivos(decimal value)
    {
        // Arrange & Act
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            Description = "Teste",
            Value = value,
            Type = TransactionType.Despesa,
            PersonId = Guid.NewGuid(),
            CategoryId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };

        // Assert
        transaction.Value.Should().Be(value);
        transaction.Value.Should().BePositive();
    }

    [Fact]
    public void Transaction_Despesa_DeveTerValorPositivo()
    {
        // Arrange
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            Description = "Despesa Teste",
            Value = 50m,
            Type = TransactionType.Despesa,
            PersonId = Guid.NewGuid(),
            CategoryId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };

        // Assert
        transaction.Type.Should().Be(TransactionType.Despesa);
        transaction.Value.Should().BePositive();
    }

    [Fact]
    public void Transaction_Receita_DeveTerValorPositivo()
    {
        // Arrange
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            Description = "Receita Teste",
            Value = 3000m,
            Type = TransactionType.Receita,
            PersonId = Guid.NewGuid(),
            CategoryId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };

        // Assert
        transaction.Type.Should().Be(TransactionType.Receita);
        transaction.Value.Should().BePositive();
    }

    [Fact]
    public void Transaction_DevePermitirAtualizacao()
    {
        // Arrange
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            Description = "Descrição Original",
            Value = 100m,
            Type = TransactionType.Despesa,
            PersonId = Guid.NewGuid(),
            CategoryId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };

        var novaDescricao = "Descrição Atualizada";
        var novoValor = 150m;
        var dataAtualizacao = DateTime.UtcNow.AddMinutes(5);

        // Act
        transaction.Description = novaDescricao;
        transaction.Value = novoValor;
        transaction.UpdatedAt = dataAtualizacao;

        // Assert
        transaction.Description.Should().Be(novaDescricao);
        transaction.Value.Should().Be(novoValor);
        transaction.UpdatedAt.Should().Be(dataAtualizacao);
    }

    [Fact]
    public void Transaction_DeveManterReferenciasPerson()
    {
        // Arrange
        var personId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            Description = "Teste",
            Value = 100m,
            Type = TransactionType.Despesa,
            PersonId = personId,
            CategoryId = categoryId,
            CreatedAt = DateTime.UtcNow
        };

        // Assert
        transaction.PersonId.Should().Be(personId);
        transaction.CategoryId.Should().Be(categoryId);
    }

    [Fact]
    public void Transaction_ComNavigationProperties_DeveConterDadosRelacionados()
    {
        // Arrange
        var person = new Person
        {
            Id = Guid.NewGuid(),
            Name = "João Silva",
            Age = 25,
            CreatedAt = DateTime.UtcNow
        };

        var category = new Category
        {
            Id = Guid.NewGuid(),
            Description = "Alimentação",
            Purpose = CategoryPurpose.Despesa,
            CreatedAt = DateTime.UtcNow
        };

        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            Description = "Compra no mercado",
            Value = 150m,
            Type = TransactionType.Despesa,
            PersonId = person.Id,
            CategoryId = category.Id,
            Person = person,
            Category = category,
            CreatedAt = DateTime.UtcNow
        };

        // Assert
        transaction.Person.Should().NotBeNull();
        transaction.Person.Name.Should().Be("João Silva");
        transaction.Category.Should().NotBeNull();
        transaction.Category.Description.Should().Be("Alimentação");
    }
}