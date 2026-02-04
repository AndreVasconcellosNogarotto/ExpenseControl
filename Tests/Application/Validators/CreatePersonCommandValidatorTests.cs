using ExpenseControl.Application.Commands.Person;
using ExpenseControl.Application.Validators.Person;
using FluentAssertions;
using Xunit;

namespace ExpenseControl.Tests.Application.Validators;

public class CreatePersonCommandValidatorTests
{
    private readonly CreatePersonCommandValidator _validator;

    public CreatePersonCommandValidatorTests()
    {
        _validator = new CreatePersonCommandValidator();
    }

    [Fact]
    public void Validator_ComandoValido_DevePassarNaValidacao()
    {
        // Arrange
        var command = new CreatePersonCommand
        {
            Name = "João Silva",
            Age = 25
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    public void Validator_NomeVazio_DeveFalharNaValidacao(string nomeInvalido)
    {
        // Arrange
        var command = new CreatePersonCommand
        {
            Name = nomeInvalido,
            Age = 25
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("obrigatório"));
    }

    [Fact]
    public void Validator_NomeNulo_DeveFalharNaValidacao()
    {
        // Arrange
        var command = new CreatePersonCommand
        {
            Name = null!,
            Age = 25
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public void Validator_NomeMuitoLongo_DeveFalharNaValidacao()
    {
        // Arrange
        var nomeMuitoLongo = new string('A', 201); 
        var command = new CreatePersonCommand
        {
            Name = nomeMuitoLongo,
            Age = 25
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("200"));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(18)]
    [InlineData(50)]
    [InlineData(120)]
    public void Validator_IdadeValida_DevePassarNaValidacao(int idadeValida)
    {
        // Arrange
        var command = new CreatePersonCommand
        {
            Name = "João Silva",
            Age = idadeValida
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Validator_IdadeZeroOuNegativa_DeveFalharNaValidacao(int idadeInvalida)
    {
        // Arrange
        var command = new CreatePersonCommand
        {
            Name = "João Silva",
            Age = idadeInvalida
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Age");
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("maior que 0")); 
    }

    [Theory]
    [InlineData(121)]
    [InlineData(150)]
    [InlineData(200)]
    public void Validator_IdadeMuitoAlta_DeveFalharNaValidacao(int idadeInvalida)
    {
        // Arrange
        var command = new CreatePersonCommand
        {
            Name = "João Silva",
            Age = idadeInvalida
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Age");
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("120"));
    }

    [Fact]
    public void Validator_MultiplosCamposInvalidos_DeveRetornarTodosErros()
    {
        // Arrange
        var command = new CreatePersonCommand
        {
            Name = "", 
            Age = 0  
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Count.Should().BeGreaterThanOrEqualTo(2);
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
        result.Errors.Should().Contain(e => e.PropertyName == "Age");
    }

    [Theory]
    [InlineData("A")]
    [InlineData("Ab")]
    [InlineData("João")]
    [InlineData("Maria")]
    public void Validator_NomesCurtos_DevePassarNaValidacao(string nome)
    {
        // Arrange
        var command = new CreatePersonCommand
        {
            Name = nome,
            Age = 25
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}