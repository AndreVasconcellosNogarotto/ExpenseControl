using ExpenseControl.Domain.Entities;
using ExpenseControl.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseControl.Infrastructure.Configurations;

/// <summary>
/// Configuração do Entity Framework para a entidade Transaction.
/// Define restrições, índices e relacionamentos usando Fluent API.
/// </summary>
public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        // Nome da tabela
        builder.ToTable("Transactions");

        // Chave primária
        builder.HasKey(t => t.Id);

        // Propriedades
        builder.Property(t => t.Description)
            .IsRequired()
            .HasMaxLength(400);

        builder.Property(t => t.Value)
            .IsRequired()
            .HasColumnType("decimal(18,2)"); // Precisão para valores monetários

        builder.Property(t => t.Type)
            .IsRequired()
            .HasConversion<string>(); // Armazena o enum como string no banco

        builder.Property(t => t.CreatedAt)
            .IsRequired();

        builder.Property(t => t.UpdatedAt)
            .IsRequired(false);

        // Relacionamentos já configurados nas outras entidades
        // mas podemos adicionar configurações adicionais se necessário

        // Índices para melhorar performance de consultas
        builder.HasIndex(t => t.PersonId);
        builder.HasIndex(t => t.CategoryId);
        builder.HasIndex(t => t.Type);
        builder.HasIndex(t => t.CreatedAt);
    }
}
