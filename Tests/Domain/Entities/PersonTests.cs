using ExpenseControl.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace ExpenseControl.Tests.Domain.Entities;

public class PersonTests
{
    [Fact]
    public void Person_DeveSerCriadaComPropriedadesCorretas()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "João Silva";
        var age = 25;
        var createdAt = DateTime.UtcNow;

        // Act
        var person = new Person
        {
            Id = id,
            Name = name,
            Age = age,
            CreatedAt = createdAt
        };

        // Assert
        person.Id.Should().Be(id);
        person.Name.Should().Be(name);
        person.Age.Should().Be(age);
        person.CreatedAt.Should().Be(createdAt);
        person.UpdatedAt.Should().BeNull();
    }

    [Theory]
    [InlineData(18, true)]
    [InlineData(25, true)]
    [InlineData(17, false)]
    [InlineData(0, false)]
    public void Person_DeveDeterminarSeMaiorDeIdade(int age, bool esperadoMaiorDeIdade)
    {
        // Arrange
        var person = new Person
        {
            Id = Guid.NewGuid(),
            Name = "Teste",
            Age = age,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        var isMaiorDeIdade = person.Age >= 18;

        // Assert
        isMaiorDeIdade.Should().Be(esperadoMaiorDeIdade);
    }

    [Fact]
    public void Person_DevePermitirAtualizacaoDeNome()
    {
        // Arrange
        var person = new Person
        {
            Id = Guid.NewGuid(),
            Name = "Nome Original",
            Age = 30,
            CreatedAt = DateTime.UtcNow
        };

        var novoNome = "Nome Atualizado";
        var dataAtualizacao = DateTime.UtcNow.AddMinutes(5);

        // Act
        person.Name = novoNome;
        person.UpdatedAt = dataAtualizacao;

        // Assert
        person.Name.Should().Be(novoNome);
        person.UpdatedAt.Should().Be(dataAtualizacao);
    }

    [Fact]
    public void Person_DevePermitirAtualizacaoDeIdade()
    {
        // Arrange
        var person = new Person
        {
            Id = Guid.NewGuid(),
            Name = "João",
            Age = 25,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        person.Age = 26;
        person.UpdatedAt = DateTime.UtcNow;

        // Assert
        person.Age.Should().Be(26);
        person.UpdatedAt.Should().NotBeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Person_NomeVazioOuNulo_DeveSerPermitidoNaEntidade(string? nomeInvalido)
    {
        // Arrange & Act
        var person = new Person
        {
            Id = Guid.NewGuid(),
            Name = nomeInvalido!,
            Age = 25,
            CreatedAt = DateTime.UtcNow
        };

        // Assert
        person.Name.Should().Be(nomeInvalido);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-10)]
    [InlineData(150)]
    public void Person_IdadeInvalida_DeveSerPermitidaNaEntidade(int idadeInvalida)
    {
        // Arrange & Act
        var person = new Person
        {
            Id = Guid.NewGuid(),
            Name = "Teste",
            Age = idadeInvalida,
            CreatedAt = DateTime.UtcNow
        };

        // Assert
        person.Age.Should().Be(idadeInvalida);
    }
}