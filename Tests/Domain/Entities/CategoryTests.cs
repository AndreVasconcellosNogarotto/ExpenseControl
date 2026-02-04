using ExpenseControl.Domain.Entities;
using ExpenseControl.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace ExpenseControl.Tests.Domain.Entities;

public class CategoryTests
{
    [Fact]
    public void Category_DeveSerCriadaComPropriedadesCorretas()
    {
        // Arrange
        var id = Guid.NewGuid();
        var description = "Alimentação";
        var purpose = CategoryPurpose.Ambas;
        var createdAt = DateTime.UtcNow;

        // Act
        var category = new Category
        {
            Id = id,
            Description = description,
            Purpose = purpose,
            CreatedAt = createdAt
        };

        // Assert
        category.Id.Should().Be(id);
        category.Description.Should().Be(description);
        category.Purpose.Should().Be(purpose);
        category.CreatedAt.Should().Be(createdAt);
        category.UpdatedAt.Should().BeNull();
    }

    [Theory]
    [InlineData(CategoryPurpose.Despesa)]
    [InlineData(CategoryPurpose.Receita)]
    [InlineData(CategoryPurpose.Ambas)]
    public void Category_DeveAceitarTodosTiposDeFinalidade(CategoryPurpose purpose)
    {
        // Arrange & Act
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Description = "Teste",
            Purpose = purpose,
            CreatedAt = DateTime.UtcNow
        };

        // Assert
        category.Purpose.Should().Be(purpose);
    }

    [Fact]
    public void Category_ComFinalidadeDespesa_DeveSerCompativel()
    {
        // Arrange
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Description = "Transporte",
            Purpose = CategoryPurpose.Despesa,
            CreatedAt = DateTime.UtcNow
        };

        // Act & Assert
        category.Purpose.Should().Be(CategoryPurpose.Despesa);
        category.Purpose.Should().NotBe(CategoryPurpose.Receita);
    }

    [Fact]
    public void Category_ComFinalidadeReceita_DeveSerCompativel()
    {
        // Arrange
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Description = "Salário",
            Purpose = CategoryPurpose.Receita,
            CreatedAt = DateTime.UtcNow
        };

        // Act & Assert
        category.Purpose.Should().Be(CategoryPurpose.Receita);
        category.Purpose.Should().NotBe(CategoryPurpose.Despesa);
    }

    [Fact]
    public void Category_ComFinalidadeAmbas_DeveSerCompativel()
    {
        // Arrange
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Description = "Diversos",
            Purpose = CategoryPurpose.Ambas,
            CreatedAt = DateTime.UtcNow
        };

        // Act & Assert
        category.Purpose.Should().Be(CategoryPurpose.Ambas);
    }

    [Fact]
    public void Category_DevePermitirAtualizacaoDeDescricao()
    {
        // Arrange
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Description = "Descrição Original",
            Purpose = CategoryPurpose.Ambas,
            CreatedAt = DateTime.UtcNow
        };

        var novaDescricao = "Descrição Atualizada";
        var dataAtualizacao = DateTime.UtcNow.AddMinutes(5);

        // Act
        category.Description = novaDescricao;
        category.UpdatedAt = dataAtualizacao;

        // Assert
        category.Description.Should().Be(novaDescricao);
        category.UpdatedAt.Should().Be(dataAtualizacao);
    }

    [Fact]
    public void Category_DevePermitirAtualizacaoDeFinalidade()
    {
        // Arrange
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Description = "Categoria Teste",
            Purpose = CategoryPurpose.Despesa,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        category.Purpose = CategoryPurpose.Receita;
        category.UpdatedAt = DateTime.UtcNow;

        // Assert
        category.Purpose.Should().Be(CategoryPurpose.Receita);
        category.UpdatedAt.Should().NotBeNull();
    }
}