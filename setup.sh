#!/bin/bash

# Script de configuração do projeto ExpenseControl
# Este script cria toda a estrutura necessária do backend

echo "🚀 Iniciando configuração do projeto ExpenseControl..."

# Navegar para o diretório do backend
cd /home/claude/ExpenseControl/backend

# Criar estrutura de diretórios da API
echo "📁 Criando estrutura de diretórios da API..."
mkdir -p src/ExpenseControl.API/Controllers
mkdir -p src/ExpenseControl.API/Middleware
mkdir -p src/ExpenseControl.API/Extensions

echo "✅ Estrutura de diretórios criada com sucesso!"
echo ""
echo "📦 Estrutura do projeto:"
echo "ExpenseControl/"
echo "├── backend/"
echo "│   ├── src/"
echo "│   │   ├── ExpenseControl.API (Controllers, Middleware, Program.cs)"
echo "│   │   ├── ExpenseControl.Application (CQRS, DTOs, Validators)"
echo "│   │   ├── ExpenseControl.Domain (Entities, Interfaces, Enums)"
echo "│   │   └── ExpenseControl.Infrastructure (DbContext, Repositories)"
echo "│   └── ExpenseControl.sln"
echo "└── frontend/ (React + TypeScript)"
echo ""
echo "⚠️  PRÓXIMOS PASSOS:"
echo "1. Abrir o projeto no Visual Studio 2022"
echo "2. Configurar a connection string no appsettings.json"
echo "3. Executar as migrations: dotnet ef migrations add InitialCreate"
echo "4. Atualizar o banco: dotnet ef database update"
echo "5. Executar o projeto: dotnet run --project src/ExpenseControl.API"
echo ""
echo "✨ Setup concluído!"
