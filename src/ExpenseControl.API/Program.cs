using ExpenseControl.Application.Commands.Person;
using ExpenseControl.Application.Mappings;
using ExpenseControl.Domain.Interfaces;
using ExpenseControl.Infrastructure.Data;
using ExpenseControl.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configurar DbContext com SQL Server
builder.Services.AddDbContext<ExpenseControlDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar repositórios
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

// Registrar MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreatePersonCommand).Assembly));

// Registrar FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(CreatePersonCommand).Assembly);

// Adicionar controllers
builder.Services.AddControllers()
    .AddApplicationPart(typeof(Program).Assembly);

// SOLUÇÃO CORS - Com origem ESPECÍFICA do frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173",  // Vite dev server
                "http://localhost:3000",  // Create React App (alternativa)
                "http://127.0.0.1:5173"   // Endereço alternativo
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // Permite cookies se necessário
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Expense Control API",
        Version = "v1",
        Description = "API para controle de gastos residenciais"
    });
});

var app = builder.Build();

// Habilitar Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Expense Control API v1");
    c.RoutePrefix = "swagger";
});

var logger = app.Services.GetRequiredService<ILogger<Program>>();

if (app.Environment.IsDevelopment())
{
    logger.LogInformation("🔧 Modo de desenvolvimento ativado");
}

// Middleware de logging
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    var origin = context.Request.Headers["Origin"].ToString();
    await next();
});

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();