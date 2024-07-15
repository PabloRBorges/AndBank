FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/AndBank.Api/AndBank.Api.csproj", "src/AndBank.Api/"]
COPY ["src/AndBank.Process.Data/AndBank.Process.Data.csproj", "src/AndBank.Process.Data/"]
COPY ["src/AndBank.Business/AndBank.Process.Business.csproj", "src/AndBank.Business/"]
COPY ["src/AndBank.Core/AndBank.Core.csproj", "src/AndBank.Core/"]
COPY ["src/AndBank.Process.Application/AndBank.Process.Application.csproj", "src/AndBank.Process.Application/"]
RUN dotnet restore "src/AndBank.Api/AndBank.Api.csproj"
COPY . .
WORKDIR /src
RUN dotnet build "src/AndBank.Api/AndBank.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./src/AndBank.Api/AndBank.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AndBank.Api.dll"]