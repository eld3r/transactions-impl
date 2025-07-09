FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Transactions.Api/Transactions.Api.csproj", "Transactions.Api/"]
COPY ["Transactions.Services/Transactions.Services.csproj", "Transactions.Services/"]
COPY ["Transactions.Services.Impl/Transactions.Services.Impl.csproj", "Transactions.Services.Impl/"]
COPY ["Transactions.Dal/Transactions.Dal.csproj", "Transactions.Dal/"]
COPY ["Transactions.Domain/Transactions.Domain.csproj", "Transactions.Domain/"]
COPY ["Transactions.Dal.PostgresEfCore/Transactions.Dal.PostgresEfCore.csproj", "Transactions.Dal.PostgresEfCore/"]
RUN dotnet restore "Transactions.Api/Transactions.Api.csproj"
COPY . .
WORKDIR "/src/Transactions.Api"
RUN dotnet build "./Transactions.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Transactions.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Transactions.Api.dll"]
