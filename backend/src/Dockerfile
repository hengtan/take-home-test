FROM mcr.microsoft.com/dotnet/sdk:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia os .csproj separadamente para otimizar cache
COPY ["Fundo.API/Fundo.API.csproj", "Fundo.API/"]
COPY ["Fundo.Application/Fundo.Application.csproj", "Fundo.Application/"]
COPY ["Fundo.Infrastructure/Fundo.Infrastructure.csproj", "Fundo.Infrastructure/"]
COPY ["Fundo.Domain/Fundo.Domain.csproj", "Fundo.Domain/"]

RUN dotnet restore "Fundo.API/Fundo.API.csproj"

# Copia o restante
COPY . .

WORKDIR "/src/Fundo.API"
RUN dotnet build "Fundo.API.csproj" -c Release -o /app/build

FROM build AS publish

RUN dotnet publish "Fundo.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Fundo.API.dll"]