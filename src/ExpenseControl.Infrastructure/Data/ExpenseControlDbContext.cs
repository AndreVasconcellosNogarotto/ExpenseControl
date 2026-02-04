using ExpenseControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseControl.Infrastructure.Data;

public class ExpenseControlDbContext : DbContext
{
    public ExpenseControlDbContext(DbContextOptions<ExpenseControlDbContext> options)
        : base(options)
    {
    }

    public DbSet<Person> Persons { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExpenseControlDbContext).Assembly);
    }
}
