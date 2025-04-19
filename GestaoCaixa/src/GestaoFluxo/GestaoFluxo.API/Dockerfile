#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/GestaoFluxo/GestaoFluxo.API/GestaoFluxo.API.csproj", "src/GestaoFluxo/GestaoFluxo.API/"]
COPY ["src/GestaoFluxo/GestaoFluxo.Application/GestaoFluxo.Application.csproj", "src/GestaoFluxo/GestaoFluxo.Application/"]
COPY ["src/GestaoFluxo/GestaoFluxo.Domain/GestaoFluxo.Domain.csproj", "src/GestaoFluxo/GestaoFluxo.Domain/"]
COPY ["src/GestaoFluxo/GestaoFluxo.Infrastructure/GestaoFluxo.Infrastructure.csproj", "src/GestaoFluxo/GestaoFluxo.Infrastructure/"]
RUN dotnet restore "./src/GestaoFluxo/GestaoFluxo.API/GestaoFluxo.API.csproj"
COPY . .
WORKDIR "/src/src/GestaoFluxo/GestaoFluxo.API"
RUN dotnet build "./GestaoFluxo.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GestaoFluxo.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GestaoFluxo.API.dll"]