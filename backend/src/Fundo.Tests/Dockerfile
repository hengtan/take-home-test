FROM mcr.microsoft.com/dotnet/sdk:9.0 AS test
WORKDIR /app

COPY . .

RUN dotnet restore "./backend/src/Fundo.Services.Tests/Fundo.Services.Tests.csproj"

WORKDIR /app/backend/src/Fundo.Services.Tests

RUN dotnet test Fundo.Services.Tests.csproj \
    --logger "console;verbosity=detailed" \
    --no-build \
    --results-directory /app/TestResults