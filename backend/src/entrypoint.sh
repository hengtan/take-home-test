#!/bin/sh

echo "🔄 Waiting for SQL Server to start..."
until /opt/mssql-tools/bin/sqlcmd -S db -U sa -P 'Your_strong_Pass123' -Q "SELECT 1" &>/dev/null
do
  echo "⏳ Waiting for database..."
  sleep 2
done

echo "✅ SQL Server is available!"

echo "📦 Applying migrations..."
dotnet ef database update --project Fundo.Infrastructure --startup-project Fundo.API

echo "🚀 Starting the API..."
exec dotnet Fundo.API.dll