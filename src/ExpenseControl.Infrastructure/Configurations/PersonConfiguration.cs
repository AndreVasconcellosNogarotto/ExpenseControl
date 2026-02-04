using ExpenseControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseControl.Infrastructure.Configurations;

/// <summary>
/// Configuração do Entity Framework para a entidade Person.
/// Define restrições, índices e relacionamentos usando Fluent API.
/// </summary>
public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        // Nome da tabela
        builder.ToTable("Persons");

        // Chave primária
        builder.HasKey(p => p.Id);

        // Propriedades
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Age)
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .IsRequired(false);

        // Relacionamentos
        // Uma pessoa pode ter muitas transações
        // Quando uma pessoa é deletada, suas transações também são deletadas (cascade)
        builder.HasMany(p => p.Transactions)
            .WithOne(t => t.Person)
            .HasForeignKey(t => t.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        // Índices
        builder.HasIndex(p => p.Name);
    }
}
