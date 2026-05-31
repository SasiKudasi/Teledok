FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Teledok.Api/Teledok.Api.csproj", "Teledok.Api/"]
COPY ["Teledok.Api.Shared/Teledok.Api.Shared.csproj", "Teledok.Api.Shared/"]
COPY ["Teledok.Domain/Teledok.Domain.csproj", "Teledok.Domain/"]
COPY ["Teledok.Contracts.Shared/Teledok.Contracts.Shared.csproj", "Teledok.Contracts.Shared/"]
COPY ["Teledok.Domain.Shared/Teledok.Domain.Shared.csproj", "Teledok.Domain.Shared/"]
COPY ["Teledok.Infrastructure/Teledok.Infrastructure.csproj", "Teledok.Infrastructure/"]
COPY ["Teledok.Application/Teledok.Application.csproj", "Teledok.Application/"]
COPY ["Teledok.Application.Shared/Teledok.Application.Shared.csproj", "Teledok.Application.Shared/"]
RUN dotnet restore "./Teledok.Api/Teledok.Api.csproj"
COPY . .
WORKDIR "/src/Teledok.Api"
RUN dotnet build "./Teledok.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Teledok.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Teledok.Api.dll"]