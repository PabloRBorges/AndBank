#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/AndBank.Console/AndBank.Console.csproj", "src/AndBank.Console/"]
COPY ["src/AndBank.Business/AndBank.Process.Business.csproj", "src/AndBank.Business/"]
COPY ["src/AndBank.Core/AndBank.Core.csproj", "src/AndBank.Core/"]
COPY ["src/AndBank.Process.Application/AndBank.Process.Application.csproj", "src/AndBank.Process.Application/"]
COPY ["src/AndBank.Process.Data/AndBank.Process.Data.csproj", "src/AndBank.Process.Data/"]
RUN dotnet restore "./src/AndBank.Console/AndBank.Console.csproj"
COPY . .
WORKDIR "/src/src/AndBank.Console"
RUN dotnet build "./AndBank.Console.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AndBank.Console.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AndBank.Console.dll"]