using ExpenseControl.Application.Commands.Category;
using ExpenseControl.Domain.Enums;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace ExpenseControl.Tests.Application.Validators;

/// <summary>
/// Testes unitários para CreateCategoryCommandValidator.
/// </summary>
public class CreateCategoryCommandValidatorTests
{
    private readonly CreateCategoryCommandValidator _validator;

    public CreateCategoryCommandValidatorTests()
    {
        _validator = new CreateCategoryCommandValidator();
    }

    [Fact]
    public void Validator_ComandoValido_DevePassarNaValidacao()
    {
        // Arrange
        var command = new CreateCategoryCommand
        {
            Description = "Alimentação",
            Purpose = CategoryPurpose.Ambas
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
    public void Validator_DescricaoVazia_DeveFalharNaValidacao(string descricaoInvalida)
    {
        // Arrange
        var command = new CreateCategoryCommand
        {
            Description = descricaoInvalida,
            Purpose = CategoryPurpose.Ambas
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Description");
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("obrigatória"));
    }

    [Fact]
    public void Validator_DescricaoMuitoLonga_DeveFalharNaValidacao()
    {
        // Arrange
        var descricaoMuitoLonga = new string('A', 401); // 401 caracteres
        var command = new CreateCategoryCommand
        {
            Description = descricaoMuitoLonga,
            Purpose = CategoryPurpose.Ambas
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Description");
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("400"));
    }

    [Theory]
    [InlineData(CategoryPurpose.Despesa)]
    [InlineData(CategoryPurpose.Receita)]
    [InlineData(CategoryPurpose.Ambas)]
    public void Validator_FinalidadeValida_DevePassarNaValidacao(CategoryPurpose finalidade)
    {
        // Arrange
        var command = new CreateCategoryCommand
        {
            Description = "Categoria Teste",
            Purpose = finalidade
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Validator_FinalidadeInvalida_DeveFalharNaValidacao()
    {
        // Arrange
        var command = new CreateCategoryCommand
        {
            Description = "Teste",
            Purpose = (CategoryPurpose)999 // Enum inválido
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Purpose");
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("inválida"));
    }

    [Theory]
    [InlineData("A")]
    [InlineData("Comida")]
    [InlineData("Transporte e Deslocamento")]
    public void Validator_DescricoesVariadas_DevePassarNaValidacao(string descricao)
    {
        // Arrange
        var command = new CreateCategoryCommand
        {
            Description = descricao,
            Purpose = CategoryPurpose.Ambas
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Validator_DescricaoComEspacosNasExtremidades_DevePassarNaValidacao()
    {
        // Arrange
        var command = new CreateCategoryCommand
        {
            Description = "  Categoria com espaços  ",
            Purpose = CategoryPurpose.Ambas
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}