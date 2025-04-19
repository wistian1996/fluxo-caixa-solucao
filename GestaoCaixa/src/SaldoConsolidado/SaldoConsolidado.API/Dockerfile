#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/SaldoConsolidado/SaldoConsolidado.API/SaldoConsolidado.API.csproj", "src/SaldoConsolidado/SaldoConsolidado.API/"]
COPY ["src/SaldoConsolidado/SaldoConsolidado.Infrastructure/SaldoConsolidado.Infrastructure.csproj", "src/SaldoConsolidado/SaldoConsolidado.Infrastructure/"]
COPY ["src/SaldoConsolidado/SaldoConsolidado.Application/SaldoConsolidado.Application.csproj", "src/SaldoConsolidado/SaldoConsolidado.Application/"]
COPY ["src/SaldoConsolidado/SaldoConsolidado.Domain/SaldoConsolidado.Domain.csproj", "src/SaldoConsolidado/SaldoConsolidado.Domain/"]
RUN dotnet restore "./src/SaldoConsolidado/SaldoConsolidado.API/SaldoConsolidado.API.csproj"
COPY . .
WORKDIR "/src/src/SaldoConsolidado/SaldoConsolidado.API"
RUN dotnet build "./SaldoConsolidado.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SaldoConsolidado.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SaldoConsolidado.API.dll"]