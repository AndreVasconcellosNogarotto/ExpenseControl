using ExpenseControl.Domain.Entities;
using ExpenseControl.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseControl.Infrastructure.Configurations;

/// <summary>
/// Configuração do Entity Framework para a entidade Category.
/// Define restrições, índices e relacionamentos usando Fluent API.
/// </summary>
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        // Nome da tabela
        builder.ToTable("Categories");

        // Chave primária
        builder.HasKey(c => c.Id);

        // Propriedades
        builder.Property(c => c.Description)
            .IsRequired()
            .HasMaxLength(400);

        builder.Property(c => c.Purpose)
            .IsRequired()
            .HasConversion<string>(); // Armazena o enum como string no banco

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .IsRequired(false);

        // Relacionamentos
        // Uma categoria pode ter muitas transações
        builder.HasMany(c => c.Transactions)
            .WithOne(t => t.Category)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Restrict); // Não permite deletar categoria com transações

        // Índices
        builder.HasIndex(c => c.Description);
    }
}
